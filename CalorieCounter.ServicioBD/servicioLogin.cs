using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD
{
    public class servicioLogin
    {
        CalorieCounterEntities calorieCounterBD = null;
        public servicioLogin() 
        {
            calorieCounterBD = new CalorieCounterEntities();
        }

        /// <summary>
        /// Valida si el cliente existe o no
        /// </summary>
        public bool existeUsuario(objLogin login) 
        {
            try 
	        {
                return calorieCounterBD.tb_usuario.Any(a => a.usuario == login.usuario);
	        }
	        catch (Exception ex)
	        {
		        throw new Exception(ex.Message);
	        }
        }

        /// <summary>
        /// culmina la sesion del cliente
        /// </summary>
        /// <param name="_objSesion"></param>
        /// <returns></returns>
        public bool cerrarSesion(objSesion _objSesion)
        {

            tb_sesion sesion = null;

            try
            {
                sesion = calorieCounterBD.tb_sesion.Where(w => w.id_usuario == _objSesion.idUsuario && w.sesion == _objSesion.sesion && w.activo == 1).FirstOrDefault();

                if (sesion != null)
                {
                    sesion.sesion = "";
                    sesion.fechaUlOper = DateTime.Now;
                    sesion.activo = 0;

                    calorieCounterBD.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Valida que el usuario exisa y inicia sesion 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public object login(objLogin login) 
        {
            try
            {
                return this._login(login);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// crea una sesion, si son redes actualiza la tabla sino registra al cliente y  inicia sesion
        /// </summary>
        /// <param name="login"></param>
        /// <param name="registro"></param>
        /// <returns></returns>
        public object loginSocial(objLogin login, objRegistro registro) 
        {
            tb_usuario usuario = null;

            try
            {
                usuario = calorieCounterBD.tb_usuario.Where(w => w.usuario == login.usuario && (w.usuarioFacebook == login.usuarioFacebook || w.usuarioTwiter == login.usuarioTwiter) ).FirstOrDefault();

                if (usuario != null && registro == null)
                {
                    usuario.usuarioFacebook = login.usuarioFacebook;
                    usuario.usuarioTwiter   = login.usuarioTwiter;
                    usuario.validateToken   = login.validateToken;
                    calorieCounterBD.SaveChanges();

                    login.idUsuario = usuario.id_usuario;
                }
                else 
                {
                    registro        = this.registrarCliente(registro, login, false);
                    login.idUsuario = registro.idUsuario;
                }

                return (object)this.inicioSesion(login);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Registra un nuevo cliente 
        /// </summary>
        /// <param name="regitro">obj segun parametros recibidos</param>
        /// <param name="login">=</param>
        /// <returns></returns>
        public objRegistro registrarCliente(objRegistro regitro, objLogin login, bool modo = true) 
        {

            int aux = 0;

            try
            {

                calorieCounterBD
                    .tb_cliente.Add
                        (
                            new tb_cliente
                            {
                                nombre      = regitro.nombre,
                                apellido    = regitro.apellido,
                                correo      = regitro.correo,
                                activo = 1
                            }
                        );

                aux = Convert.ToInt32(calorieCounterBD.tb_cliente.OrderByDescending(o => o.id_cliente).Select(s => s.id_cliente).FirstOrDefault()) + 1;

                tb_usuario a =
                            (
                                (modo)
                                ?
                                    new tb_usuario
                                    {
                                        usuario         = login.usuario,
                                        contrasena      = login.contrasena,
                                        fechaRegistro   = DateTime.Now,
                                        activo          = 2,
                                        id_cliente      = aux
                                    }
                                :
                                    new tb_usuario
                                    {
                                        usuario         = login.usuario,
                                        contrasena      = login.contrasena,
                                        usuarioFacebook = login.usuarioFacebook,
                                        usuarioTwiter   = login.usuarioTwiter,
                                        validateToken   = login.validateToken,
                                        fechaRegistro   = DateTime.Now,
                                        id_cliente      = aux,
                                        activo = 1
                                    }
                            );

                calorieCounterBD.tb_usuario.Add(a);

                calorieCounterBD.SaveChanges();

                regitro.idCliente = aux;

                regitro.idUsuario = calorieCounterBD.tb_usuario.Where(w => w.id_cliente == aux).Select(s => s.id_usuario).FirstOrDefault();

                return regitro;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Inicio la sesion del usuario y devuelve un objSesion
        /// </summary>
        /// <param name="login"></param>
        public objSesion inicioSesion(objLogin login) 
        {

            objSesion _objSesion = null;

            try
            {

                _objSesion =
                    new objSesion
                    {
                        idUsuario       = login.idUsuario,
                        sesion          = Guid.NewGuid().ToString(),
                        fechaInicio     = DateTime.Now,
                        fechaUltimaOp   = DateTime.Now
                    };

                tb_sesion sesion = calorieCounterBD.tb_sesion.Where(w => w.id_usuario == _objSesion.idUsuario && w.activo == 1).FirstOrDefault();

                if (sesion != null) 
                {
                    sesion.activo = 0;
                    sesion.sesion = "";
                    calorieCounterBD.SaveChanges();
                }

                calorieCounterBD
                    .tb_sesion.Add
                        (
                            new tb_sesion 
                            {
                                id_usuario  = _objSesion.idUsuario,
                                sesion      = _objSesion.sesion,
                                fechaInicio = _objSesion.fechaInicio,
                                fechaUlOper = _objSesion.fechaUltimaOp,
                                activo      = 1
                            }
                        );

                calorieCounterBD.SaveChanges();

                return _objSesion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Actualiza la sesion actual del cliente
        /// </summary>
        /// <param name="_objSesion">recibe un objSesion de la sesion del WS ojo</param>
        /// <returns></returns>
        public objSesion actualizarSesion(objSesion _objSesion) 
        {

            tb_sesion sesion = null;

            try
            {
                sesion = calorieCounterBD.tb_sesion.Where(w => w.id_usuario == _objSesion.idUsuario && w.sesion == _objSesion.sesion && w.activo == 1).FirstOrDefault();

                if (sesion != null)
                {
                    _objSesion.sesion           = Guid.NewGuid().ToString();
                    _objSesion.fechaUltimaOp    = DateTime.Now;

                    sesion.sesion       = _objSesion.sesion;
                    sesion.fechaUlOper  = _objSesion.fechaUltimaOp;

                    calorieCounterBD.SaveChanges();
                    
                }

                return _objSesion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Unicamente logea usuario sin faceBook, podria hacerlo
        /// </summary>
        /// <param name="login"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        private objSesion _login(objLogin login, int modo = 0)
        {
            tb_usuario usuario = null;
            objSesion sesion = null;

            usuario =
                    (modo == 0)
                    ?
                        calorieCounterBD.tb_usuario.Where(w => w.usuario == login.usuario && w.contrasena == login.contrasena).FirstOrDefault()
                    :
                        null;

            if (usuario != null)
            {
                login.idUsuario = usuario.id_usuario;

                sesion = this.inicioSesion(login);
            }

            return sesion;
        }

    }
}
