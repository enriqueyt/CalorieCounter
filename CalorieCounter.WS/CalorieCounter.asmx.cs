using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CalorieCounter.Objetos;
using CalorieCounter.Controlador;

namespace CalorieCounter.WS
{
    /// <summary>
    /// Descripción breve de CalorieCounter
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class CalorieCounter : System.Web.Services.WebService
    {

        [WebMethod]
        public objBasicResponse Login(string user, string password)
        {
            return new ControladorLogin().Login(user, password);
        }

        [WebMethod]
        public objBasicResponse LoginSocial(string user, string idFacebook, string idTwiter, string validateToken, string name, string lastname)
        {
            return new ControladorLogin().LoginSocial(user, idFacebook, idTwiter, validateToken, name, lastname);
        }

        [WebMethod]
        public objBasicResponse Register(string user, string password, string name, string lastname)
        {
            return new ControladorLogin().Register(user, password, name, lastname);
        }

        [WebMethod]
        public objFoodSearchResponse FoodSearch(string ID, string Description, string GroupID) {
            return new ControllerFood().FoodSearch(
                String.IsNullOrEmpty(ID)?-1:Convert.ToInt32(ID),
                Description, 
                String.IsNullOrEmpty(GroupID) ? -1 : Convert.ToInt32(GroupID));
        }

        [WebMethod]
        public objFoodTypesResponse GetFoodTypes()
        {
            return new ControllerFood().GetFoodTypes();
        }

    }
}
