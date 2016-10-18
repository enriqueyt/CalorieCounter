using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.Objetos;
using CalorieCounter.ServicioBD;

namespace CalorieCounter.Controlador {
    public class loginController {
        objBasicResponse respuesta = null;
        public loginController() {
            respuesta = new objBasicResponse();
        }

        /// <summary>
        /// valida que exista el usuario, q que el usuario y la contraseña sea correcta y inicio sesion
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contrasena"></param>
        /// <returns></returns>
        public objBasicResponse Login(string usuario, string contrasena) {

            objSesion _objSesion = null;

            objLogin login = new objLogin {
                user = usuario,
                password = contrasena
            };

            loginService loginService = null;

            try {

                using(loginService = new loginService()) {
                    if(loginService.existeUsuario(login)) {
                        _objSesion = loginService.login(login) as objSesion;

                        if(_objSesion != null) {
                            respuesta.code = "200";
                            respuesta.sesion = _objSesion.sesion;
                        }
                        else {
                            respuesta.code = "103";
                        }
                    }
                    else {
                        respuesta.code = "102";
                        respuesta.message = "User doesn't exist";
                    }
                }
            }
            catch(Exception e) {
                respuesta.message = e.Message;
                respuesta.tarce = e.StackTrace;
            }

            return respuesta;
        }

        /// <summary>
        /// Cierra la sesion actual (usuario)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public objBasicResponse logOut(string token) {
            try {

                respuesta.obj = new loginService().logOut(token);
                respuesta.code = "200";
            }
            catch(Exception e) {
                respuesta.message = e.Message;
                respuesta.tarce = e.StackTrace;
            }
            return respuesta;
        }

        /// <summary>
        /// se logea con redes sociales, por lo momentos unicamento con faceBook y twiter
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="idFacebook"></param>
        /// <param name="idTwiter"></param>
        /// <param name="validateToken"></param>
        /// <param name="name"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        public objBasicResponse LoginSocial(string usuario, string idFacebook, string idTwiter, string validateToken, string name, string lastname) {

            objLogin login = new objLogin {
                user = usuario.ToLower(),
                userFacebook = idFacebook,
                userTwiter = idTwiter,
                validateToken = validateToken
            };

            objClient registro = new objClient {
                name = name.ToLower(),
                lastname = lastname.ToLower(),
                email = usuario.ToLower()
            };

            object res = null;
            loginService loginService = null;

            try {

                using(loginService = new loginService()) {
                    res = loginService.loginSocial(login, registro);
                    if(res != null)
                        respuesta.code = "100";
                    //respuesta.result = res;
                    else
                        respuesta.code = "105";
                }

            }
            catch(Exception e) {
                respuesta.message = e.Message;
                respuesta.tarce = e.StackTrace;
            }

            return respuesta;
        }

        /// <summary>
        /// registra al cliente e indica 104 pendiente; debo colocar las clases para enviar email; ojo debo subir el cliente o solo el id_client
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        public objBasicResponse Register(string user, string password, string name, string lastname, string currentWeight, string goalWeight, string height, string birthDate, string id_sexo, string id_activity, string id_measurementUnits) {

            try {

                objLogin login = new objLogin {
                    user = user.ToLower(),
                    password = password,
                    userMail = user.ToLower()
                };

                if(new loginService().existeUsuario(login)) {
                    respuesta.code = "205";
                    respuesta.message = "Existing user";
                }

                objClient registro = new objClient {
                    name = name.ToLower(),
                    lastname = lastname.ToLower(),
                    email = user.ToLower(),
                    currentWeight = Convert.ToInt32(currentWeight),
                    goalWeight = Convert.ToInt32(goalWeight),
                    height = Convert.ToInt32(height),
                    birthDate = Convert.ToDateTime(birthDate).Date,
                    id_sexo = Convert.ToInt32(id_sexo),
                    id_activity = Convert.ToInt32(id_activity),
                    id_measurementUnits = Convert.ToInt32(id_measurementUnits)
                };

                registro = new clientService().saveClient(registro, login);

                if(registro != null) {
                    respuesta.code = "200";
                    //respuesta.result = registro;
                }

                return respuesta;
            }
            catch(Exception e) {
                respuesta.message = e.Message;
                respuesta.tarce = e.StackTrace;
            }
            return respuesta;
        }

        /// <summary>
        /// lista el sexo
        /// </summary>
        /// <returns></returns>
        public List<objUtility> getListSexo() {

            try {
                return new loginService().getListSexo();
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
            return new loginService().getListMeasurementUnits();
        }

        /// <summary>
        /// lista las actividades
        /// </summary>
        /// <returns></returns>
        public List<objUtility> getListActivities() {
            return new loginService().getListActivities();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="to"></param>
        /// <param name="tittle"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool getPassword(string email, string to, string tittle, string content, int modo) {
            objClient _objClient = new loginService().getPassword(email);
            string ErrMess = "";
            bool result = false;

            if(modo == 0)
                content = new utilityController().DecryptBase64(content);

            if(_objClient != null)
                result = new utilityController().EnviarNotificacionEXT("", to, tittle, content.Replace("@password", _objClient.token), ref ErrMess);

            return result;

        }

    }
}
