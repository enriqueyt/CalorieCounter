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
        /// <summary>
        /// login sin ninguna red social
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [WebMethod]
        public objBasicResponse Login(string user, string password)
        {
            return responseController.CheckJson(new loginController().Login(user, password)) as objBasicResponse;
        }

        /// <summary>
        /// realiza el login con lguna red social
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idFacebook"></param>
        /// <param name="idTwiter"></param>
        /// <param name="validateToken"></param>
        /// <param name="name"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        [WebMethod]
        public objBasicResponse LoginSocial(string user, string idFacebook, string idTwiter, string validateToken, string name, string lastname)
        {
            return responseController.CheckJson(new loginController().LoginSocial(user, idFacebook, idTwiter, validateToken, name, lastname)) as objBasicResponse;
        }

        /// <summary>
        /// registra un nuevo usuario en el app
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        [WebMethod]
        public objBasicResponse Register(string user, string password, string name, string lastname)
        {
            return responseController.CheckJson(new loginController().Register(user, password, name, lastname)) as objBasicResponse;
        }

        /// <summary>
        /// busca algun tipo de comida segun su parametro 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Description"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// obtiene una losta de tipos de comida
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public objFoodTypesResponse GetFoodTypes()
        {
            return responseController.CheckJson(new foodController().GetFoodTypes()) as objFoodTypesResponse;
        }

        /// <summary>
        /// obtiene todo el detalle de alguna comida
        /// </summary>
        /// <param name="idFood"></param>
        /// <returns></returns>
        [WebMethod]
        public objFoodDetailsSearchResponse GetFoodDetailsSearch(string idFood)
        {
            return responseController.CheckJson(new foodController().GetFoodDetailsSearch(idFood) ) as objFoodDetailsSearchResponse;
        }

        /// <summary>
        /// lista tipo de mela ej, desayuno, almuerzo, cena y merienda
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public objUtilitiResponse GetMealType() 
        {
            return responseController.CheckJson(new foodController().GetMealType()) as objUtilitiResponse;
        }

        /// <summary>
        /// guarda una comida en especifica
        /// </summary>
        /// <param name="token"></param>
        /// <param name="idFood"></param>
        /// <param name="count"></param>
        /// <param name="scale"></param>
        /// <param name="meal"></param>
        /// <param name="favorite"></param>
        /// <returns></returns>
        [WebMethod]
        public objBasicResponse SaveFood(string token, int idFood, double count, int scale, int meal, bool favorite)
        {
            return responseController.CheckJson(new clientController().saveFood(token, idFood, count, scale, meal, favorite)) as objBasicResponse;
        }
        
        /// <summary>
        /// obtiene la lista de comida segun el usuario
        /// </summary>
        /// <param name="token"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [WebMethod]
        public objDataClientFoodsResponse GetListFoodClient(string token, string date)
        {
            return responseController.CheckJson(new clientController().GetListFoodClient(token, date)) as objDataClientFoodsResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [WebMethod]
        public objDataClientFoodsResponse GetRecordFood(string token, string date)
        {
            return responseController.CheckJson(new clientController().GetRecordFood(token, date)) as objDataClientFoodsResponse;
        }

        /// <summary>
        /// obtiene la lista de escalas de una comida
        /// </summary>
        /// <param name="id_food"></param>
        /// <returns></returns>
        [WebMethod]
        public objUtilitiResponse GetListScale(int id_food)
        {
            return responseController.CheckJson(new foodController().GetListScale(id_food)) as objUtilitiResponse;
        }
    }
}
