using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.Objetos;
using CalorieCounter.ServicioBD;

namespace CalorieCounter.Controlador
{
    public class loginController
    {
        objBasicResponse respuesta = null;
        public loginController() 
        {
            respuesta = new objBasicResponse();
        }
        
        /// <summary>
        /// valida que exista el usuario, q que el usuario y la contraseña sea correcta y inicio sesion
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="contrasena"></param>
        /// <returns></returns>
        public objBasicResponse Login(string usuario, string contrasena)
        {

            objLogin login = new objLogin { usuario = usuario, contrasena = contrasena };

            loginService loginService = null;

            try 
            {

                loginService = new loginService();

                if (loginService.existeUsuario(login))
                {

                    if (loginService.login(login) != null)
                    {
                        respuesta.code = "100";
                        //respuesta.result = (object)(loginService.inicioSesion(login));
                    }
                    else 
                    {
                        respuesta.code = "103";
                    }
                }
                else 
                {
                    respuesta.code = "102";
                }
  
            }
            catch (Exception e) 
            {
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
        public objBasicResponse LoginSocial(string usuario, string idFacebook, string idTwiter, string validateToken, string name, string lastname) 
        {

            objLogin login          = new objLogin { usuario = usuario, usuarioFacebook = idFacebook, usuarioTwiter = idTwiter, validateToken = validateToken };
            objClient registro    = new objClient { nombre = name, apellido = lastname, correo = usuario };
            object res = null;
            loginService loginService = null;

            try
            {
                loginService = new loginService();

                res = loginService.loginSocial(login, registro);

                if (res != null)
                {
                    respuesta.code = "100";
                    //respuesta.result = res;
                }
                else 
                {
                    respuesta.code = "105";
                }

            }
            catch (Exception e)
            {
                respuesta.message = e.Message;
                respuesta.tarce = e.StackTrace;
            }

            return respuesta;
        }

        /// <summary>
        /// registra al cliente e indica 104 pendiente; debo colocar las clases para enviar correo; ojo debo subir el cliente o solo el idCliente
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        public objBasicResponse Register(string Username, string Password, string name, string lastname)
        {

            objLogin login          = new objLogin { usuario = Username, contrasena = Password, usuarioCorreo = Username };
            objClient registro    = new objClient { nombre = name, apellido = lastname, correo = Username };

            try
            {
               registro = new clientService().saveClient(registro, login);

               if (registro != null)
                {
                    respuesta.code = "104";
                    //respuesta.result = registro;
                }

                return respuesta;
            }
            catch (Exception e)
            {
                respuesta.message = e.Message;
                respuesta.tarce = e.StackTrace;
            }
            return respuesta;
        }
    }
}
