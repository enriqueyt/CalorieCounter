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

        /// <summary>
        /// Busca un detalle de una comida
        /// </summary>
        /// <param name="idFood"></param>
        public objFoodDetailsSearchResponse FoodDetailsSearch(int idFood) 
        {

            List<objClassificationFood> _objClassificationFood = null;
            List<objClasificationDef> clasifications = new List<objClasificationDef>();

            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    _objClassificationFood =
                        calorieCounterBD.tb_classificationFood
                            .Where(w  => w.id_food == idFood)
                            .Select(s => new
                            {
                                name    = s.descripcion,
                                id      = s.id_classificationFood
                            }).AsEnumerable()
                            .Select(se => new objClassificationFood
                            {
                                name    = se.name,
                                id      = se.id,
                                list_foodDetails =
                                (
                                    from cd in calorieCounterBD.tb_classificationDetail
                                    join df in calorieCounterBD.tb_detailFood on cd.id_detailFood equals df.id_detailFood
                                    join mt in calorieCounterBD.tb_measurementType on df.id_measurementType equals mt.id_measurementType
                                    where cd.id_classificationFood == se.id
                                    select new 
                                    {
                                        descripcion = df.descripcion,
                                        unit        = mt.unit,
                                        id          = df.id_detailFood
                                    }
                                 ).AsEnumerable()
                                    .Select(sel => new objFoodDetail {
                                        descripcion = sel.descripcion,
                                        unit        = sel.unit,
                                        count       = 
                                                calorieCounterBD.tb_detailFoodColumn
                                                    .Join(calorieCounterBD.tb_columnsFood, a => a.id_columnsfood, b => b.id_columnsfood, (a,b) => new {a,b})
                                                    .Where(w=>w.a.id_detailFood == sel.id)
                                                    .Select(s=>s.a.id_detailFood).Count(),
                                        valuesFoodDetail =
                                            calorieCounterBD.tb_detailFoodColumn
                                                .Where(w  => w.id_detailFood == sel.id)
                                                .Select(s => new objFoodDetailValues {
                                                    value = s.foodValue
                                                }).ToList()
                                    }).ToList()
                            }).ToList();

                    if (_objClassificationFood.Count == 0) return new objFoodDetailsSearchResponse {code = "100"};

                    _objClassificationFood.ForEach(f => {

                        objClasificationDef _objClasificationDef = clasifications.Find(fin => fin.name == f.name);

                        if (_objClasificationDef == null)
                        {

                            clasifications.Add(new objClasificationDef
                            {
                                name                = f.name,
                                listObjFoodDetail   = new List<List<objFoodDetail>>()
                            });

                            clasifications.Find(fin => fin.name == f.name).listObjFoodDetail.Add(f.list_foodDetails);
                        
                        }
                        else
                        {
                            _objClasificationDef.listObjFoodDetail.Add(f.list_foodDetails);
                        }

                    });

                    return new objFoodDetailsSearchResponse {   
                        _objClasificationDef = clasifications, 
                        columnsCount = clasifications[0].listObjFoodDetail[0].FirstOrDefault().count 
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// obtiene meal (breakfast, lunch, etc)
        /// </summary>
        /// <returns></returns>
        public List<objUtiliti> GetMealType() 
        {

            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {
                   return  calorieCounterBD.tb_meal.Select(s => new objUtiliti { id = s.id_meal, description = s.description }).ToList<objUtiliti>();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        /// <summary>
        /// salva una comida seleccionada
        /// </summary>
        /// <param name="token">sesion</param>
        /// <returns></returns>
        public bool SaveFood(string token, int idFood, double count, string scale, int meal) 
        {

            objSaveFood _objSaveFood    = null;
            objClient _objClient        = null;
            tb_userFood _tb_userFood    = null;
            bool ok = false;

            try
            {
                _objSaveFood = new objSaveFood {
                    token   = token,
                    id_food = idFood,
                    count   = count,
                    scale   = scale,
                    meal    = meal
                };

                _objClient = new clientService().findClientebyToken(token);

                _objSaveFood.id_user = _objClient.idUsuario;

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    _tb_userFood = calorieCounterBD.tb_userFood.Where(w => w.id_food == _objSaveFood.id_food).FirstOrDefault();

                    if (_tb_userFood == null)
                    {
                        _tb_userFood = new tb_userFood 
                        {
                            id_user = _objSaveFood.id_user,
                            id_food = _objSaveFood.id_food,
                            count   = _objSaveFood.count,
                            date    = DateTime.Now,
                            scale   = _objSaveFood.scale,
                            meal    = _objSaveFood.meal
                        };

                        calorieCounterBD.SaveChanges();

                        ok = true;
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            return ok;
        }

       
    }

}



    
