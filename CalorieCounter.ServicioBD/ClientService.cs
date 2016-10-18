using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD {
    public class clientService : IDisposable {

        private CalorieCounterEntities calorieCounterBD = null;

        /// <summary>
        /// Registra un nuevo cliente 
        /// </summary>
        /// <param name="registro">obj segun parametros recibidos</param>
        /// <param name="login">=</param>
        /// <returns></returns>
        public objClient saveClient(objClient registro, objLogin login, bool modo = true) {

            int aux = 0;

            try {

                using(calorieCounterBD = new CalorieCounterEntities()) {

                    calorieCounterBD.Database.Connection.Open();

                    calorieCounterBD
                        .tb_client.Add
                            (
                                new tb_client {
                                    name = registro.name.ToLower(),
                                    lastname = registro.lastname.ToLower(),
                                    email = registro.email.ToLower(),
                                    currentWeight = registro.currentWeight,
                                    goalWeight = registro.goalWeight,
                                    height = registro.height,
                                    birthDate = registro.birthDate,
                                    id_Sexo = registro.id_sexo,
                                    id_activityType = registro.id_activity,
                                    id_measurementUnits = registro.id_measurementUnits,
                                    activo = 1
                                }
                            );

                    aux = Convert.ToInt32(calorieCounterBD.tb_client.OrderByDescending(o => o.id_client).Select(s => s.id_client).FirstOrDefault()) + 1;

                    tb_usuario tbUsuario =
                            (
                                (modo)
                                ?
                                    new tb_usuario {
                                        usuario = login.user.ToLower(),
                                        contrasena = login.password.ToLower(),
                                        fechaRegistro = DateTime.Now,
                                        activo = 1,
                                        id_cliente = aux
                                    }
                                :
                                    new tb_usuario {
                                        usuario = login.user.ToLower(),
                                        contrasena = login.password.ToLower(),
                                        usuarioFacebook = login.userFacebook,
                                        usuarioTwiter = login.userTwiter,
                                        validateToken = login.validateToken,
                                        fechaRegistro = DateTime.Now,
                                        id_cliente = aux,
                                        activo = 1
                                    }
                            );

                    calorieCounterBD.tb_usuario.Add(tbUsuario);

                    calorieCounterBD.SaveChanges();

                    registro.id_client = aux;

                    registro.id_usuario = calorieCounterBD.tb_usuario.Where(w => w.id_cliente == aux).Select(s => s.id_usuario).FirstOrDefault();
                }

                return registro;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// obtiene el cliente con solo pasarle el token de la sesion
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public objClient findClientebyToken(string token) {
            objClient _objClient = null;
            try {
                using(calorieCounterBD = new CalorieCounterEntities()) {
                    calorieCounterBD.Database.Connection.Open();

                    _objClient = (
                                calorieCounterBD.tb_sesion
                                .Join(calorieCounterBD.tb_usuario, sesion => sesion.id_usuario, usuario => usuario.id_usuario, (sesion, usuario) => new { sesion, usuario })
                                .Where(w => w.sesion.sesion == token && w.sesion.activo == 1)
                                .Select(s => new objClient {
                                    id_usuario = s.usuario.id_usuario,
                                    id_client = s.usuario.tb_client.id_client,
                                    name = s.usuario.tb_client.name,
                                    lastname = s.usuario.tb_client.lastname,
                                    email = s.usuario.tb_client.email,
                                    birthDate = s.usuario.tb_client.birthDate,
                                    currentWeight = s.usuario.tb_client.currentWeight,
                                    goalWeight = s.usuario.tb_client.goalWeight,
                                    height = s.usuario.tb_client.height,
                                    id_measurementUnits = s.usuario.tb_client.id_measurementUnits,
                                    id_sexo = s.usuario.tb_client.id_Sexo,
                                    id_activity = s.usuario.tb_client.id_activityType,
                                    activityValue = s.usuario.tb_client.tb_activityType.value
                                }).FirstOrDefault()
                            );
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }

            return _objClient;
        }

        /// <summary>
        /// obtiene una lista de mis comidas diarias
        /// </summary>
        public objDataClientFoodsResponse GetListFoodClient(string token, string date = "") {

            objClient _objClient = null;
            objDataClientFoodsResponse _objDataClientFoodsResponse = null;
            DateTime auxDate = (date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date);
            try {
                _objClient = this.findClientebyToken(token);

                if(_objClient == null) throw new Exception("Usuario inexistente");

                using(calorieCounterBD = new CalorieCounterEntities()) {

                    calorieCounterBD.Database.Connection.Open();

                    _objDataClientFoodsResponse = new objDataClientFoodsResponse {
                        objDataClientFoods = calorieCounterBD.tb_userFood
                                .Where(w => w.id_user == _objClient.id_usuario && w.date == auxDate)
                                .Select(s => new objDataClientFoods {
                                    id_food = s.id_food,
                                    description = s.tb_food.description,
                                    count = s.count,
                                    id_scale = s.id_scale,
                                    id_meal = s.tb_meal.id_meal,
                                    descMeal = s.tb_meal.description

                                }).AsEnumerable()
                                .GroupBy(g => new {
                                    g.id_meal,
                                    g.id_food,
                                    g.description,
                                    g.count,
                                    g.id_scale,
                                    g.descMeal
                                })
                                .Select(se => new objDataClientFoods {
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
            catch(Exception ex) {
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
        /// <returns></returns>
        public List<objResumenDiario> GetRecordFood(string token, out double? total, DateTime? dia = null) {
            try {
                List<objResumenDiario> resumenDiario = null;
                objClient _objClient = this.findClientebyToken(token);
                if(_objClient == null) throw new Exception("Usuario inexistente");
                dia = (dia == null ? DateTime.Now.Date : dia);
                double? _total = 0;
                foodService _foodService = new foodService();

                using(calorieCounterBD = new CalorieCounterEntities()) {
                    calorieCounterBD.Database.Connection.Open();

                    resumenDiario =
                        calorieCounterBD.tb_meal
                        .Select(s => new {
                            description = s.description,
                            id = s.id_meal
                        }).AsEnumerable()
                        .Select(se => new objResumenDiario {
                            description = se.description,
                            id = se.id,
                            objfood =
                               calorieCounterBD.tb_userFood
                               .Where(w => w.id_user == _objClient.id_usuario && w.id_meal == se.id && w.date == dia)
                               .Select(sel => new {
                                   count = sel.count,
                                   date = sel.date,
                                   id_food = sel.id_food,
                                   description = sel.tb_food.description,
                                   scale = sel.id_scale,
                                   id_meal = sel.id_meal
                               })
                               .AsEnumerable()
                               .Select(sele => new objFood {
                                   count = sele.count,
                                   date = sele.date,
                                   id_food = sele.id_food,
                                   description = sele.description,
                                   descScale = calorieCounterBD.tb_columnsFood.Where(wh => wh.id_columnsfood == sele.scale).Select(selec => selec.descripcion).FirstOrDefault(),
                                   cantCalories = _foodService.GetGramoskalorias(sele.id_food),
                                   scale = sele.scale,
                                   meal = sele.id_meal,
                                   favorite = calorieCounterBD.tb_favoriteFood.Any(a => a.id_food == sele.id_food && a.id_user == _objClient.id_client)
                               }).ToList(),
                            objWlog = new exerciseService().getWorkoutLogs(token, dia)
                        })
                        .ToList();


                    double? aux = 0;
                    if(resumenDiario != null)
                        foreach(var item in resumenDiario) {
                            aux = 0;
                            if(item.objfood.Count > 0) {
                                foreach(var item1 in item.objfood) {
                                    aux += item1.cantCalories;
                                }
                            }
                            item.cant = aux;
                            _total += item.cant;
                        }


                }
                total = _total;

                return resumenDiario;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// obtiene la comidas favoritas del usuario conectado
        /// </summary>
        /// <returns></returns>
        public List<objFood> getFavoriteFood(string token) {
            try {
                List<objFood> lfood = null;

                objClient _objClient = this.findClientebyToken(token);

                if(_objClient == null) throw new Exception("Usuario inexistente");

                using(calorieCounterBD = new CalorieCounterEntities()) {
                    calorieCounterBD.Database.Connection.Open();

                    lfood =
                    calorieCounterBD.tb_usuario
                        .Join(calorieCounterBD.tb_favoriteFood, user => user.id_usuario, ffood => ffood.id_user, (user, ffood) => new { user, ffood })
                        .Join(calorieCounterBD.tb_food, aux => aux.ffood.id_food, food => food.id_food, (aux, food) => new { aux, food })
                        .Where(w => w.aux.user.id_usuario == _objClient.id_usuario)
                        .Select(s => new {
                            id_food = s.food.id_food,
                            description = s.food.description,
                            groupID = s.food.id_foodtype
                        }).AsEnumerable()
                        .Select(se => new objFood {
                            id_food = se.id_food,
                            description = se.description,
                            groupID = se.groupID
                        }).ToList<objFood>();
                }

                return lfood;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// retorna las calorias deseadas del cliente actual
        /// </summary>
        /// <param name="_objClient">objeto que posee la información del cliente actual</param>
        /// <param name="modo"></param>
        public objGoalCalories getGoalCalorie(string token, DateTime date) {
            double? tmb = 0;
            objClient _objClient = null;

            _objClient = this.findClientebyToken(token);

            int? edad = (DateTime.Now.Subtract(_objClient.birthDate).Days / 365);

            double? _total = 0;

            objGoalCalories _objGoalCalories = null;

            try {


                if(_objClient != null) {

                    _objGoalCalories = new objGoalCalories();

                    if(_objClient.id_sexo == 1) {
                        tmb = 655 + (9.6 * _objClient.currentWeight) + (1.8 * _objClient.height) - (4.7 * edad);
                        _objGoalCalories.maxCalories = (tmb * _objClient.activityValue).ToString();
                        tmb = 655 + (9.6 * _objClient.goalWeight) + (1.8 * _objClient.height) - (4.7 * edad);
                        _objGoalCalories.minCalories = (tmb * _objClient.activityValue).ToString();
                    }
                    else {
                        tmb = 66 + (13.7 * _objClient.currentWeight) + (5 * _objClient.height) - (6.8 * edad);
                        _objGoalCalories.maxCalories = (tmb * _objClient.activityValue).ToString();
                        tmb = 66 + (13.7 * _objClient.goalWeight) + (5 * _objClient.height) - (6.8 * edad);
                        _objGoalCalories.minCalories = (tmb * _objClient.activityValue).ToString();
                    }

                    var aux = this.GetRecordFood(token, out _total, date);
                    _objGoalCalories.total = _total == null ? 0 : Convert.ToDouble(_total);
                }

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

            return _objGoalCalories;
        }

        /// <summary>
        /// actionWater safe/update
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public objWater actionWater(objWater _objWater) {

            objClient _objClient = null;
            DateTime? auxDate = (_objWater.date == null ? DateTime.Now.Date : _objWater.date);
            tb_water _tb_water = null;

            try {
                _objClient = new clientService().findClientebyToken(_objWater.token);

                if(_objWater != null) {

                    using(calorieCounterBD = new CalorieCounterEntities()) {
                        _tb_water = calorieCounterBD.tb_water.Where(w => w.id_usuario == _objClient.id_usuario && w.date == auxDate).FirstOrDefault();

                        if(_tb_water != null) {
                            _tb_water.cups = _objWater.cups;
                        }
                        else {
                            calorieCounterBD.tb_water.Add(new tb_water {
                                cups = _objWater.cups,
                                date = DateTime.Now.Date,
                                id_usuario = _objClient.id_usuario
                            });
                        }

                        calorieCounterBD.SaveChanges();
                        _objWater.id_usuario = _objClient.id_usuario;
                        /*
                        _objWater.token = new loginService().actualizarSesion(new objSesion {
                            id_usuario = _objClient.id_usuario,
                            sesion = _objWater.token
                        }).sesion;*/
                    }

                }

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

            return _objWater;
        }

        /// <summary>
        /// obtiene una lista de agua consumida segun el dia deseado
        /// </summary>
        /// <param name="token">sesion actual</param>
        /// <param name="date">fecha </param>
        /// <returns></returns>
        public List<objWater> getListWater(string token, DateTime date) {

            List<objWater> lobjWater = null;
            objClient _objClient = null;

            try {
                _objClient = new clientService().findClientebyToken(token);

                if(_objClient != null) {

                    using(calorieCounterBD = new CalorieCounterEntities()) {

                        lobjWater =
                            calorieCounterBD.tb_water
                            .Where(w => w.id_usuario == _objClient.id_usuario && w.date == date)
                            .Select(s => new objWater {
                                id_usuario = s.id_usuario,
                                id_water = s.id_water,
                                cups = s.cups,
                                date = s.date
                            }).ToList<objWater>();
                    }
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            return lobjWater;
        }

        /// <summary>
        /// actionWeight safe/update /*actualiza y salva el peso por dia*/
        /// </summary>
        /// <param name="_objWeight"></param>
        /// <returns></returns>
        public objWeight actionWeight(objWeight _objWeight) {
            objClient _objClient = null;
            DateTime auxDate = (_objWeight.date == null ? DateTime.Now.Date : _objWeight.date);
            tb_weight _tb_weight = null;

            try {
                _objClient = new clientService().findClientebyToken(_objWeight.token);

                if(_objClient != null) {
                    using(calorieCounterBD = new CalorieCounterEntities()) {
                        _tb_weight = calorieCounterBD.tb_weight.Where(w => w.id_usuario == _objWeight.id_usuario && w.date == auxDate).FirstOrDefault();

                        if(_tb_weight != null) {
                            _tb_weight.weight = _objWeight.weight;
                        }
                        else {
                            calorieCounterBD.tb_weight.Add(
                                    new tb_weight {
                                        id_usuario = _objWeight.id_usuario,
                                        weight = _objWeight.weight,
                                        date = auxDate
                                    }
                                );
                        }

                        calorieCounterBD.SaveChanges();
                        _objWeight.id_usuario = _objClient.id_usuario;

                        _objWeight.token = new loginService().actualizarSesion(new objSesion {
                            idUsuario = _objClient.id_usuario,
                            sesion = _objWeight.token
                        }).sesion;
                    }
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

            return _objWeight;
        }

        /// <summary>
        /// devuelve una  lista de peso segun el dia
        /// </summary>
        /// <param name="_objWeight"></param>
        /// <returns></returns>
        public List<objWeight> getListWeight(string token, DateTime date) {
            List<objWeight> lobjWeight = null;
            objClient _objClient = null;

            try {
                _objClient = new clientService().findClientebyToken(token);

                if(_objClient != null) {

                    using(calorieCounterBD = new CalorieCounterEntities()) {

                        lobjWeight =
                            calorieCounterBD.tb_weight
                            .Where(w => w.id_usuario == _objClient.id_usuario && w.date == date)
                            .Select(s => new objWeight {
                                id_usuario = s.id_usuario,
                                id_weiht = s.id_weight,
                                weight = s.weight,
                                date = s.date
                            }).ToList<objWeight>();
                    }
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            return lobjWeight;
        }

        /// <summary>
        /// obtiene el peso deseado y actual de un cliente
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public objClient getClientGoals(string token) {
            objClient _objClient = null;
            try {
                _objClient = new clientService().findClientebyToken(token);

                _objClient = new objClient {
                    currentWeight = _objClient.currentWeight,
                    goalWeight = _objClient.goalWeight,
                    id_activity = _objClient.id_activity
                };
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

            return _objClient;
        }

        /// <summary>
        /// actualiza el peso deseado por el cliente
        /// </summary>
        /// <param name="token"></param>
        /// <param name="objClient"></param>
        /// <returns></returns>
        public bool updateGoals(string token, objClient objClient) {
            objClient _objClient = null;
            bool flag = false;
            try {
                _objClient = new clientService().findClientebyToken(token);
                using(calorieCounterBD = new CalorieCounterEntities()) {
                    calorieCounterBD.Database.Connection.Open();

                    var aux = calorieCounterBD.tb_client.Where(w => w.id_client == _objClient.id_client).FirstOrDefault();

                    if(objClient.currentWeight > 0)
                        aux.currentWeight = objClient.currentWeight;
                    if(objClient.goalWeight > 0)
                        aux.goalWeight = objClient.goalWeight;
                    if(objClient.id_activity > 0)
                        objClient.id_activity = objClient.id_activity;

                    calorieCounterBD.SaveChanges();

                    flag = true;
                }

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            return flag;
        }

        protected void Dispose(Boolean free) {
            if(free) {
                if(this.calorieCounterBD != null) {

                    this.calorieCounterBD.Dispose();
                    this.calorieCounterBD = null;
                }
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~clientService() {
            Dispose();
        }

    }
}
