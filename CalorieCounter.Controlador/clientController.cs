using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.ServicioBD;
using CalorieCounter.Objetos;

namespace CalorieCounter.Controlador {
    public class clientController {
        /// <summary>
        /// salva una comida selecionada previamente
        /// </summary>
        /// <returns></returns>
        public objBasicResponse saveFood(string token, int idFood, double count, int scale, int meal, bool favorite, string fecha) {
            try {

                using(foodService fs = new foodService()) {
                    if(fs.SaveFood(token, idFood, count, scale, meal, favorite, fecha))
                        return new objBasicResponse { code = "200", result = "true" };
                    else
                        return new objBasicResponse { code = "500", result = "false" };
                }


            }
            catch(Exception ex) {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        /// <summary>
        /// actualiza una comida selecionada previamente
        /// </summary>
        /// <returns></returns>
        public objBasicResponse updateFood(string token, int idFood, double count, int scale, int meal, bool favorite, string fecha) {
            try {

                using(foodService fs = new foodService()) {
                    if(fs.updateFood(token, idFood, count, scale, meal, favorite, fecha))
                        return new objBasicResponse { code = "200", result = "true" };
                    else
                        return new objBasicResponse { code = "500", result = "false" };
                }


            }
            catch(Exception ex) {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        /// <summary>
        /// elimina una comida selecionada previamente
        /// </summary>
        /// <returns></returns>
        public objBasicResponse deleteFood(string token, int idFood, string fecha) {
            try {
                using(foodService fs = new foodService()) {
                    if(fs.deleteFood(token, idFood, fecha))
                        return new objBasicResponse { code = "200", result = "true" };
                    else
                        return new objBasicResponse { code = "500", result = "false" };
                }
            }
            catch(Exception ex) {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        /// <summary>
        /// obtiene una lista de mis comidas diarias
        /// </summary>
        public objDataClientFoodsResponse GetListFoodClient(string token, string date) {
            try {
                return new clientService().GetListFoodClient(token, date);
            }
            catch(Exception ex) {
                return new objDataClientFoodsResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        /// <summary>
        /// obtiene el resumen de camidas agregadas por dia, funciona para la 
        /// pagina inicial (para esta solo se deben sumar los totales y listo.)
        /// y tambien para la pagina principal del historial que comidas en el dia
        /// para este si se encuentra desglosado por tipo de alimento (desayuno,
        /// almuerzo, merienditas y cena)
        /// </summary>
        /// <returns></returns>
        public objDataClientFoodsResponse GetRecordFood(string token, string date) {
            try {
                double? _total = 0;
                return new objDataClientFoodsResponse {
                    objResumenDiario = new clientService().GetRecordFood(token, out _total, date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date),
                    total = _total
                };
            }
            catch(Exception ex) {
                return new objDataClientFoodsResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        /// <summary>
        /// obtiene la comidas favoritas del usuario conectado
        /// </summary>
        /// <returns></returns>
        public objFoodSearchResponse GetFavoriteFood(string token) {
            try {
                return new objFoodSearchResponse {
                    Foods = new clientService().getFavoriteFood(token)
                };
            }
            catch(Exception ex) {
                return new objFoodSearchResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        /// <summary>
        /// retorna las calorias deseadas del cliente actual
        /// </summary>
        /// <param name="_objClient">objeto que posee la información del cliente actual</param>
        /// <param name="modo"></param>
        public objBasicResponse getGoalCalorie(string token, string date) {
            objGoalCalories _objGoalCalories = null;
            objBasicResponse _objBasicResponse = null;
            clientService cs = null;
            try {

                cs = new clientService();
                _objGoalCalories = cs.getGoalCalorie(token, date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date);

                _objBasicResponse = new objBasicResponse {
                    code = "200",
                    obj = _objGoalCalories as objGoalCalories,
                    result = "true"
                };

                return _objBasicResponse;
            }
            catch(Exception ex) {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace, code = "500", result = "false" };
            }
        }

        /// <summary>
        /// crea o actualiza el consumo de agua segun la fecha indicada
        /// </summary>
        public objWater actionWater(string token, string cups, string date) {
            objWater _objWater = null;
            clientService cs = null;
            try {

                cs = new clientService();

                _objWater = cs.actionWater(new objWater {
                    date = date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date,
                    token = token,
                    cups = Convert.ToInt32(cups)
                });

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            return _objWater;
        }

        /// <summary>
        /// crea o actualiza el peso segun la fecha indicada
        /// </summary>
        public objWeight actionWeight(string token, string weight, string date) {
            objWeight _objWeight = null;
            clientService cs = null;
            try {
                cs = new clientService();
                _objWeight = cs.actionWeight(new objWeight() {
                    token = token,
                    date = date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date,
                    weight = Convert.ToInt32(weight)
                });

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            return _objWeight;
        }

        /// <summary>
        /// obtenie una lista de agua consumida segun la fecha indicada 
        /// (es una lista pero hasta ahora posee solo un elemento)
        /// </summary>
        public List<objWater> getListWater(string token, string date) {
            List<objWater> lwater = null;
            clientService cs = null;
            try {
                cs = new clientService();
                lwater = cs.getListWater(token, date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            return lwater;
        }

        /// <summary>
        /// obtenie una lista de peso segun la fecha indicada 
        /// (es una lista pero hasta ahora posee solo un elemento)
        /// </summary>
        public List<objWeight> getListWeight(string token, string date) {
            List<objWeight> lobjWeight = null;
            clientService cs = null;
            try {
                cs = new clientService();
                lobjWeight = cs.getListWeight(token, date == "" ? DateTime.Now.Date : Convert.ToDateTime(date).Date);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
            return lobjWeight;
        }

        /// <summary>
        /// actualiza el peso deseado por el cliente
        /// </summary>
        /// <returns></returns>
        public objBasicResponse updateGoals(string token, string goalWeight, string currentWeight, string id_activity) {
            objBasicResponse _objBasicResponse = null;
            clientService cs = null;
            try {
                cs = new clientService();
                _objBasicResponse = new objBasicResponse();
                _objBasicResponse.obj = cs.updateGoals(token, new objClient { goalWeight = Convert.ToInt32(goalWeight), currentWeight = Convert.ToInt32(currentWeight), id_activity = Convert.ToInt32(id_activity) });
                _objBasicResponse.code = "200";
                _objBasicResponse.result = "true";

            }
            catch(Exception ex) {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace, code = "500", result = "false" };
            }

            return _objBasicResponse;
        }

        /// <summary>
        /// obtiene el peso deseado y actual de un cliente
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public objBasicResponse getClientGoals(string token) {
            objBasicResponse _objBasicResponse = null;
            clientService cs = null;

            try {
                cs = new clientService();
                _objBasicResponse = new objBasicResponse();
                _objBasicResponse.obj = cs.getClientGoals(token);
                _objBasicResponse.code = "200";
                _objBasicResponse.result = "true";

            }
            catch(Exception ex) {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace, code = "500", result = "false" };
            }

            return _objBasicResponse;
        }

    }
}
