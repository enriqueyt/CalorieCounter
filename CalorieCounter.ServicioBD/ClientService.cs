using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD
{
    public class clientService
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
                    calorieCounterBD
                        .tb_cliente.Add
                            (
                                new tb_cliente
                                {
                                    nombre = registro.nombre,
                                    apellido = registro.apellido,
                                    correo = registro.correo,
                                    activo = 1
                                }
                            );

                    aux = Convert.ToInt32(calorieCounterBD.tb_cliente.OrderByDescending(o => o.id_cliente).Select(s => s.id_cliente).FirstOrDefault()) + 1;

                    tb_usuario tbUsuario =
                                (
                                    (modo)
                                    ?
                                        new tb_usuario
                                        {
                                            usuario = login.usuario,
                                            contrasena = login.contrasena,
                                            fechaRegistro = DateTime.Now,
                                            activo = 2,
                                            id_cliente = aux
                                        }
                                    :
                                        new tb_usuario
                                        {
                                            usuario = login.usuario,
                                            contrasena = login.contrasena,
                                            usuarioFacebook = login.usuarioFacebook,
                                            usuarioTwiter = login.usuarioTwiter,
                                            validateToken = login.validateToken,
                                            fechaRegistro = DateTime.Now,
                                            id_cliente = aux,
                                            activo = 1
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

        public objClient findClientebyToken(string token) 
        {
            objClient _objClient = null;
            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    _objClient = (
                                calorieCounterBD.tb_sesion
                                .Join(
                                    calorieCounterBD.tb_usuario, sesion => sesion.id_usuario, usuario => usuario.id_usuario, (sesion, usuario) => new { sesion, usuario }
                                )
                                .Where(w => w.sesion.sesion == token && w.sesion.activo == 1)
                                .Select(s => new objClient
                                {
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
    
    }
}
