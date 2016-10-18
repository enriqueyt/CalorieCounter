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
        public objBasicResponse Login(string user, string password) {
            return responseController.CheckJson(new loginController().Login(user, password)) as objBasicResponse;
        }


        /// <summary>
        /// cierr sesion actual
        /// </summary>
        /// <param name="token">sesion actual</param>
        /// <returns></returns>
        [WebMethod]
        public objBasicResponse LogOut(string token) {
            return responseController.CheckJson(new loginController().logOut(token)) as objBasicResponse;
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
        public objBasicResponse LoginSocial(string user, string idFacebook, string idTwiter, string validateToken, string name, string lastname) {
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
        public objBasicResponse Register(string user, string password, string name, string lastname, string currentWeight, string goalWeight, string height, string birthDate, string id_sexo, string id_activity, string id_measurementUnits) {
            return responseController.CheckJson(new loginController().Register(user, password, name, lastname, currentWeight, goalWeight, height, birthDate, id_sexo, id_activity, id_measurementUnits)) as objBasicResponse;
        }

        /// <summary>
        /// busca algun tipo de comida segun su parametro 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Description"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        [WebMethod]
        public objFoodSearchResponse FoodSearch(string id, string Description, string GroupID) {
            return responseController.CheckJson(
                            new foodController().FoodSearch(
                                String.IsNullOrEmpty(id) ? -1 : Convert.ToInt32(id),
                                Description,
                                String.IsNullOrEmpty(GroupID) ? -1 : Convert.ToInt32(GroupID)
                           )) as objFoodSearchResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [WebMethod]
        public objFoodSearchResponse getfavoriteFood(string token) {
            return responseController.CheckJson(new clientController().GetFavoriteFood(token)) as objFoodSearchResponse;
        }

        /// <summary>
        /// obtiene una losta de tipos de comida
        /// </summary>
        /// <returns></returns>s
        [WebMethod]
        public objFoodTypesResponse GetFoodTypes() {
            return responseController.CheckJson(new foodController().GetFoodTypes()) as objFoodTypesResponse;
        }

        /// <summary>
        /// obtiene todo el detalle de alguna comida
        /// </summary>
        /// <param name="idFood"></param>
        /// <returns></returns>
        [WebMethod]
        public objFoodDetailsSearchResponse GetFoodDetailsSearch(string idFood) {
            return responseController.CheckJson(new foodController().GetFoodDetailsSearch(idFood)) as objFoodDetailsSearchResponse;
        }

        /// <summary>
        /// lista tipo de mela ej, desayuno, almuerzo, cena y merienda
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public objUtilitiResponse GetMealType() {
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
        public objBasicResponse SaveFood(string token, int idFood, double count, int scale, int meal, bool favorite, string fecha) {
            return responseController.CheckJson(new clientController().saveFood(token, idFood, count, scale, meal, favorite, fecha)) as objBasicResponse;
        }

        /// <summary>
        /// actualiza la comida seleccionada segun parametros
        /// </summary>
        /// <param name="token"></param>
        /// <param name="idFood"></param>
        /// <param name="count"></param>
        /// <param name="scale"></param>
        /// <param name="meal"></param>
        /// <param name="favorite"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [WebMethod]
        public objBasicResponse updateFood(string token, int idFood, double count, int scale, int meal, bool favorite, string fecha) {
            return responseController.CheckJson(new clientController().updateFood(token, idFood, count, scale, meal, favorite, fecha)) as objBasicResponse;
        }

        /// <summary>
        /// elimina la comida seleccinada segun parametross
        /// </summary>
        /// <param name="token"></param>
        /// <param name="idFood"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [WebMethod]
        public objBasicResponse removeFood(string token, int idFood, string fecha) {
            return responseController.CheckJson(new clientController().deleteFood(token, idFood, fecha)) as objBasicResponse;
        }

        /// <summary>
        /// obtiene la lista de comida segun el usuario
        /// </summary>
        /// <param name="token"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [WebMethod]
        public objDataClientFoodsResponse GetListFoodClient(string token, string date) {
            return responseController.CheckJson(new clientController().GetListFoodClient(token, date)) as objDataClientFoodsResponse;
        }

        /// <summary>
        /// obtiene el resumen diario del usuario 
        /// (solo funciona para la pag home y se debe hacer a cada rato 
        /// xD por si existe un cambio)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [WebMethod]
        public objDataClientFoodsResponse GetRecordFood(string token, string date) {
            return responseController.CheckJson(new clientController().GetRecordFood(token, date)) as objDataClientFoodsResponse;
        }


        [WebMethod]
        public objBasicResponse getGoalCalorie(string token, string date) {
            return responseController.CheckJson(new clientController().getGoalCalorie(token, date)) as objBasicResponse;
        }

        /// <summary>
        /// obtiene la lista de escalas de una comida
        /// </summary>
        /// <param name="id_food"></param>
        /// <returns></returns>
        [WebMethod]
        public objUtilitiResponse GetListScale(int id_food) {
            return responseController.CheckJson(new foodController().GetListScale(id_food)) as objUtilitiResponse;
        }

        /// <summary>
        /// crea o actualiza el consumo de agua segun la fecha indicada
        /// </summary>
        [WebMethod]
        public objWater actionWater(string token, string cups, string date) {
            return responseController.CheckJson(new clientController().actionWater(token, cups, date)) as objWater;
        }

        /// <summary>
        /// crea o actualiza el peso segun la fecha indicada
        /// </summary>
        [WebMethod]
        public objWeight actionWeight(string token, string weight, string date) {
            return responseController.CheckJson(new clientController().actionWeight(token, weight, date)) as objWeight;
        }

        /// <summary>
        /// obtenie una lista de agua consumida segun la fecha indicada 
        /// (es una lista pero hasta ahora posee solo un elemento)
        /// </summary>
        [WebMethod]
        public List<objWater> getListWater(string token, string date) {
            return responseController.CheckJson(new clientController().getListWater(token, date)) as List<objWater>;
        }

        /// <summary>
        /// obtenie una lista de peso segun la fecha indicada 
        /// (es una lista pero hasta ahora posee solo un elemento)
        /// </summary>
        [WebMethod]
        public List<objWeight> getListWeight(string token, string date) {
            return responseController.CheckJson(new clientController().getListWeight(token, date)) as List<objWeight>;
        }

        /// <summary>
        /// lista el sexo
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<objUtility> getListSexo() {
            return responseController.CheckJson(new loginController().getListSexo()) as List<objUtility>;
        }

        /// <summary>
        /// lista las unidades de medición
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<objUtility> getListMeasurementUnits() {
            return responseController.CheckJson(new loginController().getListMeasurementUnits()) as List<objUtility>;
        }

        /// <summary>
        /// lista las actividades
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<objUtility> getListActivities() {
            return responseController.CheckJson(new loginController().getListActivities()) as List<objUtility>;
        }

        /// <summary>
        /// 
        /// </summary>
        [WebMethod]
        public objBasicResponse getClientGoals(string token) {
            return responseController.CheckJson(new clientController().getClientGoals(token)) as objBasicResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        [WebMethod]
        public objBasicResponse updateGoals(string token, string goalWeight, string currentWeight, string id_activity) {
            return responseController.CheckJson(new clientController().updateGoals(token, goalWeight, currentWeight, id_activity)) as objBasicResponse;
        }

        [WebMethod]
        public objBasicResponse sendMessageByEmail(string email, string tittle, string content, int modo) {
            return responseController.CheckJson(new loginController().getPassword(email, email, tittle, content, modo)) as objBasicResponse;
        }

        //
        // GET: /Exercise/
        [WebMethod]
        public List<objCategory> getCategories() {
            return responseController.CheckJson(new exerciseController().getCategories()) as List<objCategory>;
        }

        //
        // GET:/Exercise/favoriteExercise/token
        [WebMethod]
        public List<objFavoriteExercise> favoriteExercise(string token) {
            return responseController.CheckJson(new exerciseController().favoriteExercise(token)) as List<objFavoriteExercise>;
        }

        //
        // POST:/Exercise/saveFavoriteExercise
        [WebMethod]
        public string saveFavoriteExercise(string token, int id_exercise) {
            return responseController.CheckJson(new exerciseController().saveFavoriteExercise(token, new objFavoriteExercise {id_exercise = id_exercise })) as string;
        }

        //
        // POST:/Exercise/deleteFavoriteExercise
        [WebMethod]
        public string deleteFavoriteExercise(string token, int id_exercise) {
            return responseController.CheckJson(new exerciseController().deleteFavoriteExercise(token, id_exercise)) as string;
        }

        //
        // POST:/Exercise/saveCommentExercise
        [WebMethod]
        public string saveCommentExercise(int id_exercise, string comment) {
            return responseController.CheckJson(new exerciseController().saveCommentExercise(new objExerciseComment { id_exercise = id_exercise, comment = comment })) as string;
        }

        //
        // POST:/Exercise/saveWorkoutLog
        [WebMethod]
        public string saveWorkoutLog(string token, int id_exercise, double minutes, int sets, int reps, double weight, int favorito) {
            return responseController.CheckJson(new exerciseController().saveWorkoutLog(token, new objWorkoutLog {
                id_exercise = id_exercise,
                minutes = minutes,
                sets = sets,
                reps = reps,
                weight = weight,
                favorito = Convert.ToByte(favorito)
            })) as string;
        }

        [WebMethod]
        public List<objWorkoutLog> getWorkoutLogs(string token, string date) {
            return responseController.CheckJson(new exerciseController().getWorkoutLogs(token, date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date)) as List<objWorkoutLog>;
        }

    
    }
}
