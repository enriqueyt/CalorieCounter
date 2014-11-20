using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.Objetos;
using CalorieCounter.ServicioBD;

namespace CalorieCounter.Controlador
{
    public class ControllerFood
    {

        objBasicResponse respuesta = null;
        public ControllerFood() 
        {
            respuesta = new objBasicResponse();
        }

        public objFoodSearchResponse FoodSearch(int ID, string Description, int GroupID) {
            var R = new objFoodSearchResponse();
            try {
                R.Foods = new foodService().FoodSearch(ID,Description,GroupID);
                R.code = "OK";
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
            var R = new objFoodTypesResponse();
            try
            {
                R.FoodTypes = new foodService().FoodTypes();
                R.code = "OK";
            }
            catch (Exception e)
            {
                R.message = e.Message;
                R.tarce = e.StackTrace;
                R.code = "ERROR";
            }
            return R;
        }

        public objFoodDetailsSearchResponse GetFoodDetailsSearch(string idFood) 
        {

            objFoodDetailsSearchResponse res = null;

            try
            {
                res = new foodService().FoodDetailsSearch(Convert.ToInt32(idFood));
            }
            catch (Exception e)
            {
                res.message = e.Message;
                res.tarce = e.StackTrace;
                res.code = "ERROR";
            }

            return res;
            
        }

    }
}
