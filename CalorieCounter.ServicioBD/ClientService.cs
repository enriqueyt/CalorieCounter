﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD
{
    public class clientService : IDisposable
    {

        private CalorieCounterEntities calorieCounterBD = null;

        /// <summary>
        /// Registra un nuevo cliente 
        /// </summary>
        /// <param name="registro">obj segun parametros recibidos</param>
        /// <param name="login">=</param>
        /// <returns></returns>
        public objClient saveClient(objClient registro, objLogin login, bool modo = true)
        {

            int aux = 0;

            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {

                    calorieCounterBD.Database.Connection.Open();

                    calorieCounterBD
                        .tb_cliente.Add
                            (
                                new tb_cliente
                                {
                                    nombre = registro.nombre.ToLower(),
                                    apellido = registro.apellido.ToLower(),
                                    correo = registro.correo.ToLower(),
                                    activo      = 1
                                }
                            );

                    aux = Convert.ToInt32(calorieCounterBD.tb_cliente.OrderByDescending(o => o.id_cliente).Select(s => s.id_cliente).FirstOrDefault()) + 1;

                    if (calorieCounterBD.tb_usuario.Any(a => a.usuario == login.usuario.ToLower()))
                    {
                        registro.message = "existing user";
                        return registro;
                    }
                        

                    tb_usuario tbUsuario =
                                (
                                    (modo)
                                    ?
                                        new tb_usuario
                                        {
                                            usuario = login.usuario.ToLower(),
                                            contrasena = login.contrasena.ToLower(),
                                            fechaRegistro   = DateTime.Now,
                                            activo          = 1,
                                            id_cliente      = aux
                                        }
                                    :
                                        new tb_usuario
                                        {
                                            usuario = login.usuario.ToLower(),
                                            contrasena = login.contrasena.ToLower(),
                                            usuarioFacebook = login.usuarioFacebook,
                                            usuarioTwiter   = login.usuarioTwiter,
                                            validateToken   = login.validateToken,
                                            fechaRegistro   = DateTime.Now,
                                            id_cliente      = aux,
                                            activo          = 1
                                        }
                                );

                    calorieCounterBD.tb_usuario.Add(tbUsuario);

                    calorieCounterBD.SaveChanges();

                    registro.idCliente = aux;

                    registro.idUsuario = calorieCounterBD.tb_usuario.Where(w => w.id_cliente == aux).Select(s => s.id_usuario).FirstOrDefault();
                }

                return registro;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// obtiene el cliente con solo pasarle el token de la sesion
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public objClient findClientebyToken(string token) 
        {
            objClient _objClient = null;
            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    _objClient = (
                                calorieCounterBD.tb_sesion
                                .Join(calorieCounterBD.tb_usuario, sesion => sesion.id_usuario, usuario => usuario.id_usuario, (sesion, usuario) => new { sesion, usuario })
                                .Where(w => w.sesion.sesion == token && w.sesion.activo == 1)
                                .Select(s => new objClient {
                                    idUsuario   = s.usuario.id_usuario,
                                    idCliente   = s.usuario.tb_cliente.id_cliente,
                                    nombre      = s.usuario.tb_cliente.nombre,
                                    apellido    = s.usuario.tb_cliente.apellido,
                                    correo      = s.usuario.tb_cliente.correo
                                }).FirstOrDefault()
                            );
                }           
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }

            return _objClient;
        }

        /// <summary>
        /// obtiene una lista de mis comidas diarias
        /// </summary>
        public objDataClientFoodsResponse GetListFoodClient(string token, string date = "")
        {

            objClient _objClient = null;
            objDataClientFoodsResponse _objDataClientFoodsResponse = null;
            DateTime auxDate = (date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date);
            try
            {
                _objClient = this.findClientebyToken(token);

                if (_objClient == null) throw new Exception("Usuario inexistente");

                using (calorieCounterBD = new CalorieCounterEntities())
                {

                    calorieCounterBD.Database.Connection.Open();

                    _objDataClientFoodsResponse = new objDataClientFoodsResponse
                    {
                        objDataClientFoods = calorieCounterBD.tb_userFood
                                .Where(w => w.id_user == _objClient.idUsuario && w.date == auxDate)
                                .Select(s => new objDataClientFoods
                                {
                                    id_food = s.id_food,
                                    description = s.tb_food.description,
                                    count = s.count,
                                    id_scale = s.id_scale,
                                    id_meal = s.tb_meal.id_meal,
                                    descMeal = s.tb_meal.description

                                }).AsEnumerable()
                                .GroupBy(g => new
                                {
                                    g.id_meal,
                                    g.id_food,
                                    g.description,
                                    g.count,
                                    g.id_scale,
                                    g.descMeal
                                })
                                .Select(se => new objDataClientFoods
                                {
                                    id_food = se.Key.id_food,
                                    description = se.Key.description,
                                    count = se.Key.count,
                                    id_scale = se.Key.id_scale,
                                    id_meal = se.Key.id_meal,
                                    descMeal = se.Key.descMeal

                                }).ToList()

                    };

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            return _objDataClientFoodsResponse;
        }

        /// <summary>
        /// obtiene el resumen de camidas agregadas por dia, funciona para la 
        /// pagina inicial (para esta solo se deben sumar los totales y listo.)
        /// y tambien para la pagina principal del historial que comidas en el dia
        /// para este si se encuentra desglosado por tipo de alimento (desayuno,
        /// almuerzo, merienditas y cena)
        /// </summary>
        /// <param name="dia"></param>
        /// <returns></returns>
        public List<objResumenDiario> GetRecordFood(string token, out double? total, DateTime? dia = null)
        {
            try
            {
                List<objResumenDiario> resumenDiario = null;
                objClient _objClient = this.findClientebyToken(token);
                if (_objClient == null) throw new Exception("Usuario inexistente");
                dia = (dia == null ? DateTime.Now.Date : dia);
                double? _total = 0;
                foodService _foodService = new foodService();

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    resumenDiario =
                        calorieCounterBD.tb_meal
                        .Select(s => new
                        {
                            description = s.description,
                            id = s.id_meal
                        }).AsEnumerable()
                        .Select(se => new objResumenDiario
                        {
                            description = se.description,
                            id = se.id,
                            objfood =
                               calorieCounterBD.tb_userFood
                               .Where(w => w.id_user == _objClient.idUsuario && w.id_meal == se.id && w.date == dia)
                               .Select(sel => new
                               {
                                   count = sel.count,
                                   date = sel.date,
                                   id_food = sel.id_food,
                                   description = sel.tb_food.description,
                                   scale = sel.id_scale,
                                   id_meal = sel.id_meal
                               })
                               .AsEnumerable()
                               .Select(sele => new objFood
                               {
                                   count = sele.count,
                                   date = sele.date,
                                   id_food = sele.id_food,
                                   description = sele.description,
                                   descScale = calorieCounterBD.tb_columnsFood.Where(wh => wh.id_columnsfood == sele.scale).Select(selec => selec.descripcion).FirstOrDefault(),
                                   cantCalories = _foodService.GetGramoskalorias(sele.id_food),
                                   scale = sele.scale,
                                   meal = sele.id_meal,
                                   favorite = calorieCounterBD.tb_favoriteFood.Any(a => a.id_food == sele.id_food && a.id_user == _objClient.idCliente)
                               }).ToList()
                        })
                        .ToList();


                    double? aux = 0;
                    if (resumenDiario != null)
                        foreach (var item in resumenDiario) {
                            aux = 0;
                            if (item.objfood.Count > 0) {
                                foreach (var item1 in item.objfood) {
                                    aux += item1.count;
                                    
                                }
                            }
                            item.cant = aux;
                            _total += item.cant;
                        }
                            

                }
                total = _total;
                return resumenDiario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// obtiene la comidas favoritas del usuario conectado
        /// </summary>
        /// <returns></returns>
        public List<objFood> getFavoriteFood(string token){
            try
            {
                List<objFood> lfood = null;

                objClient _objClient = this.findClientebyToken(token);

                if (_objClient == null) throw new Exception("Usuario inexistente");

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    lfood =
                    calorieCounterBD.tb_usuario
                        .Join(calorieCounterBD.tb_favoriteFood, user => user.id_usuario, ffood => ffood.id_user, (user, ffood) => new { user, ffood })
                        .Join(calorieCounterBD.tb_food, aux => aux.ffood.id_food, food => food.id_food, (aux, food) => new { aux, food })
                        .Where(w => w.aux.user.id_usuario == _objClient.idUsuario)
                        .Select(s => new
                        {
                            id_food     = s.food.id_food,
                            description = s.food.description,
                            groupID     = s.food.id_foodtype
                        }).AsEnumerable()
                        .Select(se => new objFood
                        {
                            id_food     = se.id_food,
                            description = se.description,
                            groupID     = se.groupID
                        }).ToList<objFood>();
                }

                return lfood;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        protected void Dispose(Boolean free)
        {
            if (free)
            {
                if (this.calorieCounterBD != null)
                {

                    this.calorieCounterBD.Dispose();
                    this.calorieCounterBD = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~clientService()
        {
            Dispose();
        }

    }
}
