using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.ServicioBD;
using CalorieCounter.Objetos;

namespace CalorieCounter.Controlador
{
    public class clientController
    {
        public objBasicResponse saveFood(string token, int idFood, double count, string scale, int meal) 
        {
            try
            {
                if (new foodService().SaveFood(token, idFood, count, scale, meal))
                    return new objBasicResponse { code = "200", result = "true" };
                else
                    return new objBasicResponse { code = "500", result = "false" };
            }
            catch (Exception ex)
            {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        public objDataClientFoodsResponse GetListFoodClient(string token, DateTime? date = null) 
        {
            try
            {
                return new clientService().GetListFoodClient(token, date);
            }
            catch (Exception ex)
            {
                return new objDataClientFoodsResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }
    }
}
