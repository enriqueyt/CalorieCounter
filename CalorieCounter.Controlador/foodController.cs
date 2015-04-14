using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.Objetos;
using CalorieCounter.ServicioBD;

namespace CalorieCounter.Controlador
{
    public class foodController
    {

        objBasicResponse respuesta = null;
        public foodController() 
        {
            respuesta = new objBasicResponse();
        }

        public objFoodSearchResponse FoodSearch(int ID, string Description, int GroupID) {
            var R = new objFoodSearchResponse();
            try {

                using (foodService fs = new foodService())
                {
                    R.Foods = fs.FoodSearch(ID, Description, GroupID);
                    R.code = "OK";
                }
            }
            catch (Exception e){
                R.message = e.Message;
                R.tarce = e.StackTrace;
                R.code = "ERROR";
            }
            return R;
        }

        public objFoodTypesResponse GetFoodTypes()
        {
            objFoodTypesResponse _objFoodTypesResponse = null;

            try
            {
                using (foodService fs = new foodService())
                {
                    _objFoodTypesResponse = new objFoodTypesResponse
                    {
                        FoodTypes = fs.FoodTypes(),
                        code = "OK"
                    };
                }
               
            }
            catch (Exception ex)
            {
                _objFoodTypesResponse = new objFoodTypesResponse{
                    message = ex.Message,
                    tarce   = ex.StackTrace,
                    code    = "500"
                };
            }
            return _objFoodTypesResponse;
        }

        public objFoodDetailsSearchResponse GetFoodDetailsSearch(string idFood) 
        {

            objFoodDetailsSearchResponse res = null;

            try
            {
                using (foodService fs = new foodService())
                {
                    res = fs.FoodDetailsSearch(Convert.ToInt32(idFood));
                }
               
            }
            catch (Exception e)
            {
                res = new objFoodDetailsSearchResponse {
                    message = e.Message,
                    tarce   = e.StackTrace,
                    code    = "ERROR"
                };
               
            }

            return res;
            
        }

        public objUtilitiResponse GetMealType() 
        {
            objUtilitiResponse _objUtilitiResponse = null;

            try
            {
                using (foodService fs = new foodService())
                {
                    _objUtilitiResponse = new objUtilitiResponse
                    {
                        utiliti = fs.GetMealType()
                    };
                }
                
            }
            catch (Exception ex)
            {
                _objUtilitiResponse = new objUtilitiResponse
                {
                     message = ex.Message,
                     tarce = ex.StackTrace,
                     code = "500"
                };
            }

            return _objUtilitiResponse;

        }

        public objUtilitiResponse GetListScale(int id_food) {

            objUtilitiResponse _objUtilitiResponse = null;

            try
            {
                using (foodService fs = new foodService())
                {
                    _objUtilitiResponse = new objUtilitiResponse
                    {
                        utiliti = fs.GetListScale(id_food)
                    };
                }
                
            }
            catch (Exception ex)
            {
                _objUtilitiResponse = new objUtilitiResponse
                {
                    message = ex.Message,
                    tarce = ex.StackTrace,
                    code = "500"
                };
            }

            return _objUtilitiResponse;
        }
        public objUtilitiResponse SaveFood(string token, int idFood, double amount, int scale, int meal, bool favorite) 
        {
            objUtilitiResponse _objUtilitiResponse = null;

            try
            {
                using (foodService fs = new foodService())
                {
                    _objUtilitiResponse = new objUtilitiResponse
                    {
                        flag = fs.SaveFood(token, idFood, amount, scale, meal, favorite)
                    };
                }
                
            }
            catch (Exception ex)
            {
                _objUtilitiResponse = new objUtilitiResponse
                {
                    message = ex.Message,
                    tarce = ex.StackTrace,
                    code = "500"
                };
            }

            return _objUtilitiResponse;
        }
            

    }
}
