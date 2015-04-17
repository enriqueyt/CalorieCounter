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
        public objBasicResponse saveFood(string token, int idFood, double count, int scale, int meal, bool favorite, string fecha) 
        {
            try
            {

                using (foodService fs = new foodService())
                {
                    if (fs.SaveFood(token, idFood, count, scale, meal, favorite, fecha))
                        return new objBasicResponse { code = "200", result = "true" };
                    else
                        return new objBasicResponse { code = "500", result = "false" };
                }

                
            }
            catch (Exception ex)
            {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        public objBasicResponse updateFood(string token, int idFood, double count, int scale, int meal, bool favorite, string fecha)
        {
            try
            {

                using (foodService fs = new foodService())
                {
                    if (fs.updateFood(token, idFood, count, scale, meal, favorite, fecha))
                        return new objBasicResponse { code = "200", result = "true" };
                    else
                        return new objBasicResponse { code = "500", result = "false" };
                }


            }
            catch (Exception ex)
            {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

        public objBasicResponse deleteFood(string token, int idFood, string fecha)
        {
            try
            {

                using (foodService fs = new foodService())
                {
                    if (fs.deleteFood(token, idFood, fecha))
                        return new objBasicResponse { code = "200", result = "true" };
                    else
                        return new objBasicResponse { code = "500", result = "false" };
                }


            }
            catch (Exception ex)
            {
                return new objBasicResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        } 

        public objDataClientFoodsResponse GetListFoodClient(string token, string date ) 
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

        public objDataClientFoodsResponse  GetRecordFood(string token, string date)
        {
            try
            {
                double? _total = 0;
                return new objDataClientFoodsResponse { objResumenDiario = new clientService().GetRecordFood(token, out _total, date ==""?DateTime.Now.Date:Convert.ToDateTime(date).Date), total = _total };
            }
            catch (Exception ex)
            {
                return new objDataClientFoodsResponse { message = ex.Message, tarce = ex.StackTrace };
            }
        }

    }
}
