using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD
{
    public class foodService
    {
        private CalorieCounterEntities calorieCounterBD = null;

        /// <summary>
        /// return a speciific food
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Description"></param>
        /// <param name="GroupID"></param>
        /// <returns></returns>
        public List<objFood> FoodSearch(int ID, string Description, int GroupID) {

            List<objFood> l_objFood = null;

            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    l_objFood = 
                        calorieCounterBD.tb_food
                            .Where(w => w.id_food == ID || w.description.Contains(Description) || w.id_foodtype == GroupID)
                            .Select(s => new objFood {
                                Id = s.id_food,
                                Description = s.description,
                                GroupID = s.id_foodtype
                            }).ToList<objFood>();
                }

                return l_objFood;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        /// <summary>
        /// return a list of type foods
        /// </summary>
        /// <returns></returns>
        public List<objFoodType> FoodTypes() 
        {
            List<objFoodType> l_fiddType = null;

            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    l_fiddType =
                        calorieCounterBD.tb_foodtype
                            .Select(s => new objFoodType{
                                Id = s.id_foodtype,
                                Description = s.description
                            }).ToList<objFoodType>();
                }

                return l_fiddType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public void FoodDetailsSearch() 
        {
            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

    }

    public class objFoodDetailsSearchResponse : objBasicResponse 
    {
        public List<objClassificationFood> list_classificationFood { get; set; }

        public int columnsCount { get; set; }

    }

    public class objClassificationFood 
    {
        public string name { get; set; }

        public List<objFoodDeteil> list_foodDetails { get; set; }

    }

    public class objFoodDeteil 
    {

    }

}
