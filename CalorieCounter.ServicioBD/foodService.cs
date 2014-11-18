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
            vw_FoodDetail a = new vw_FoodDetail();
            a.FoodID = 0;
            a.FoodReference = 0;
            a.NDB = 0;
            a.UnitType = "";
            a.Val = 1.2;
            a.ValGroup = "";
            a.ValName = "";
            a.ValType = "";         
        }

        public class foodDetails 
        {
            public int foodId           { get; set; }
            public int foodI { get; set; }
            public int foodReference    { get; set; }

            public int? ndb             { get; set; }

            public string unitType      { get; set; }

            public double? val          { get; set; }

            public string valGroup      { get; set; }

            public string valName       { get; set; } 

            public string valType       { get; set; }
        }

    }
}
