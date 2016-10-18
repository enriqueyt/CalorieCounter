using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD {
    public class loginService : IDisposable {
        CalorieCounterEntities calorieCounterBD = null;
        public loginService() {
            calorieCounterBD = new CalorieCounterEntities();
            calorieCounterBD.Database.Connection.Open();
        }

        /// <summary>
        /// Valida si el cliente existe o no
        /// </summary>
        public bool existeUsuario(objLogin login) {
            try {
                return calorieCounterBD.tb_usuario.Any(a => a.usuario == login.user);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// culmina la sesion del cliente
        /// </summary>
        /// <param name="_objSesion"></param>
        /// <returns></returns>
        public bool logOut(string token) {

            tb_sesion sesion = null;

            try {
                sesion = calorieCounterBD.tb_sesion.Where(w => w.sesion == token && w.activo == 1).FirstOrDefault();

                if(sesion != null) {
                    sesion.sesion = "";
                    sesion.fechaUlOper = DateTime.Now;
                    sesion.activo = 0;

                    calorieCounterBD.SaveChanges();

                    return true;
                }

                return false;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Valida que el usuario exisa y inicia sesion 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public object login(objLogin login) {
            try {
                return this._login(login);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// crea una sesion, si son redes actualiza la tabla sino registra al cliente y  inicia sesion
        /// </summary>
        /// <param name="login"></param>
        /// <param name="registro"></param>
        /// <returns></returns>
        public object loginSocial(objLogin login, objClient registro) {
            tb_usuario usuario = null;

            try {
                usuario = calorieCounterBD.tb_usuario.Where(w => w.usuario == login.user && (w.usuarioFacebook == login.userFacebook || w.usuarioTwiter == login.userTwiter)).FirstOrDefault();

                if(usuario != null && registro == null) {
                    usuario.usuarioFacebook = login.userFacebook;
                    usuario.usuarioTwiter = login.userTwiter;
                    usuario.validateToken = login.validateToken;
                    calorieCounterBD.SaveChanges();

                    login.id_user = usuario.id_usuario;
                }
                else {
                    registro = new clientService().saveClient(registro, login, false);
                    login.id_user = registro.id_usuario;
                }

                return (object)this.inicioSesion(login);

            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Inicio la sesion del usuario y devuelve un objSesion
        /// </summary>
        /// <param name="login"></param>
        public objSesion inicioSesion(objLogin login) {

            objSesion _objSesion = null;

            try {

                _objSesion =
                    new objSesion {
                        idUsuario = login.id_user,
                        sesion = Guid.NewGuid().ToString(),
                        fechaInicio = DateTime.Now,
                        fechaUltimaOp = DateTime.Now
                    };

                tb_sesion sesion = calorieCounterBD.tb_sesion.Where(w => w.id_usuario == _objSesion.idUsuario && w.activo == 1).FirstOrDefault();

                if(sesion != null) {
                    sesion.activo = 0;
                    sesion.sesion = "";
                    calorieCounterBD.SaveChanges();
                }

                calorieCounterBD
                    .tb_sesion.Add
                        (
                            new tb_sesion {
                                id_usuario = _objSesion.idUsuario,
                                sesion = _objSesion.sesion,
                                fechaInicio = _objSesion.fechaInicio,
                                fechaUlOper = _objSesion.fechaUltimaOp,
                                activo = 1
                            }
                        );

                calorieCounterBD.SaveChanges();

                return _objSesion;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Actualiza la sesion actual del cliente
        /// </summary>
        /// <param name="_objSesion">recibe un objSesion de la sesion del WS ojo</param>
        /// <returns></returns>
        public objSesion actualizarSesion(objSesion _objSesion) {

            tb_sesion sesion = null;

            try {
                sesion = calorieCounterBD.tb_sesion.Where(w => w.id_usuario == _objSesion.idUsuario && w.sesion == _objSesion.sesion && w.activo == 1).FirstOrDefault();

                if(sesion != null) {
                    _objSesion.sesion = Guid.NewGuid().ToString();
                    _objSesion.fechaUltimaOp = DateTime.Now;

                    sesion.sesion = _objSesion.sesion;
                    sesion.fechaUlOper = _objSesion.fechaUltimaOp;

                    calorieCounterBD.SaveChanges();

                }

                return _objSesion;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }
        }

        /// <summary>
        /// Unicamente logea usuario sin faceBook, podria hacerlo
        /// </summary>
        /// <param name="login"></param>
        /// <param name="modo"></param>
        /// <returns></returns>
        private objSesion _login(objLogin login, int modo = 0) {
            tb_usuario usuario = null;
            objSesion sesion = null;

            usuario =
                    (modo == 0)
                    ?
                        calorieCounterBD.tb_usuario.Where(w => w.usuario == login.user && w.contrasena == login.password).FirstOrDefault()
                    :
                        null;

            if(usuario != null) {
                login.id_user = usuario.id_usuario;

                sesion = this.inicioSesion(login);
            }

            return sesion;
        }

        /// <summary>
        /// lista el sexo
        /// </summary>
        /// <returns></returns>
        public List<objUtility> getListSexo() {

            try {
                
                return calorieCounterBD.tb_sexo.Select(s => new objUtility() {
                    id = s.id_sexo,
                    description = s.description
                }).ToList<objUtility>();
                
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// lista las unidades de medición
        /// </summary>
        /// <returns></returns>
        public List<objUtility> getListMeasurementUnits() {
            List<objUtility> list = null;

            try {

                list = calorieCounterBD.tb_measurementUnits.Select(s => new objUtility {
                    id = s.id_measurementUnits,
                    description = s.description
                }).ToList();

                return list;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            } 
            
        }

        /// <summary>
        /// lista las actividades
        /// </summary>
        /// <returns></returns>
        public List<objUtility> getListActivities() {
            try {

                return calorieCounterBD.tb_activityType.Select(s => new objUtility() {
                    id = s.id_activityType,
                    description = s.description
                }).ToList<objUtility>();

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            } 
        }

        public objClient getPassword(string email) {
            objClient _objClient = null;
            try {
               
                _objClient = calorieCounterBD.tb_client
                    .Where(w => w.email == email)
                    .Select(s =>
                        new objClient {
                            token = s.tb_usuario
                                .Where(wh => wh.id_cliente == s.id_client)
                                .Select(se => se.contrasena).FirstOrDefault()
                        }).FirstOrDefault();
                
            }
            catch(Exception ex) {
                throw new Exception(ex.Message + " " + ex.StackTrace, ex.InnerException);
            }

            return _objClient;
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

        ~loginService() {
            Dispose();
        }

    }
}
