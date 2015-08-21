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
            return responseController.CheckJson(new loginController().Login(user, password)) as objBasicResponse;
        }

        [WebMethod]
        public objBasicResponse LoginSocial(string user, string idFacebook, string idTwiter, string validateToken, string name, string lastname)
        {
            return responseController.CheckJson(new loginController().LoginSocial(user, idFacebook, idTwiter, validateToken, name, lastname)) as objBasicResponse;
        }

        [WebMethod]
        public objBasicResponse Register(string user, string password, string name, string lastname)
        {
            return responseController.CheckJson(new loginController().Register(user, password, name, lastname)) as objBasicResponse;
        }

        [WebMethod]
        public objFoodSearchResponse FoodSearch(string ID, string Description, string GroupID) {
            return 
               responseController.CheckJson(
                            new foodController().FoodSearch(
                                String.IsNullOrEmpty(ID)? -1 : Convert.ToInt32(ID),
                                Description, 
                                String.IsNullOrEmpty(GroupID) ? -1 : Convert.ToInt32(GroupID)
                           )) as objFoodSearchResponse;
        }

        [WebMethod]
        public objFoodTypesResponse GetFoodTypes()
        {
            return responseController.CheckJson(new foodController().GetFoodTypes()) as objFoodTypesResponse;
        }

        [WebMethod]
        public objFoodDetailsSearchResponse GetFoodDetailsSearch(string idFood)
        {
            return 
                responseController.CheckJson(new foodController().GetFoodDetailsSearch(idFood) ) as objFoodDetailsSearchResponse;
        }

        [WebMethod]
        public objUtilitiResponse GetMealType() 
        {
            return responseController.CheckJson(new foodController().GetMealType()) as objUtilitiResponse;
        }

        [WebMethod]
        public objBasicResponse SaveFood(string token, int idFood, double count, int scale, int meal, bool favorite)
        {
            return responseController.CheckJson(new clientController().saveFood(token, idFood, count, scale, meal, favorite)) as objBasicResponse;
        }
        
        [WebMethod]
        public objDataClientFoodsResponse GetListFoodClient(string token, string date)
        {
            return responseController.CheckJson(new clientController().GetListFoodClient(token, date)) as objDataClientFoodsResponse;
        }

        [WebMethod]
        public objDataClientFoodsResponse GetRecordFood(string token, string date)
        {
            return responseController.CheckJson(new clientController().GetRecordFood(token, date)) as objDataClientFoodsResponse;
        }

        [WebMethod]
        public objUtilitiResponse GetListScale(int id_food)
        {
            return responseController.CheckJson(new foodController().GetListScale(id_food)) as objUtilitiResponse;
        }
    }
}
