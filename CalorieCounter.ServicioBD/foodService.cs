using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD
{
    public class foodService : IDisposable
    {
        private CalorieCounterEntities calorieCounterBD = null;

        /// <summary>
        /// return a speciific food
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="description"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<objFood> FoodSearch(int ID, string Description, int GroupID) {

            List<objFood> l_objFood = null;

            try
            {
                using (calorieCounterBD = new CalorieCounterEntities())
                {

                    calorieCounterBD.Database.Connection.Open();

                    if (ID != -1) 
                    {
                        l_objFood =
                           calorieCounterBD.tb_food
                               .Where(w => w.id_food == ID)
                               .Select(s => new objFood
                               {
                                   id_food = s.id_food,
                                   description = s.description,
                                   groupID = s.id_foodtype
                               }).ToList<objFood>();
                    }
                    else if (GroupID != -1) 
                    {
                        l_objFood =
                           calorieCounterBD.tb_food
                               .Where(w => w.id_foodtype == GroupID)
                               .Select(s => new objFood
                               {
                                   id_food = s.id_food,
                                   description = s.description,
                                   groupID = s.id_foodtype
                               }).ToList<objFood>();
                    }
                    else if (!string.IsNullOrEmpty(Description)) 
                    {
                        l_objFood =
                           calorieCounterBD.tb_food
                               .Where(w => w.description.Contains(Description))
                               .Select(s => new objFood
                               {
                                   id_food = s.id_food,
                                   description = s.description,
                                   groupID = s.id_foodtype
                               }).ToList<objFood>();
                    }

                   
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
                    calorieCounterBD.Database.Connection.Open();

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
                    calorieCounterBD.Database.Connection.Open();

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
                        _objClasificationDef    = clasifications,
                        listScale               = this.GetListScale(idFood),
                        columnsCount            = clasifications[0].listObjFoodDetail[0].FirstOrDefault().count 
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
                    calorieCounterBD.Database.Connection.Open();

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
        public bool SaveFood(string token, int idFood, double amount, int scale, int meal, bool favorite, string fecha) 
        {

            objSaveFood _objSaveFood    = null;
            objClient _objClient        = null;
            tb_userFood _tb_userFood    = null;
            DateTime auxDate = (fecha == "" ? DateTime.Now.Date : Convert.ToDateTime(fecha).Date);
            bool ok = false;

            try
            {
                
                _objSaveFood = new objSaveFood {
                    token   = token,
                    id_food = idFood,
                    amount = amount,
                    scale   = scale,
                    meal    = meal
                };

                _objClient = new clientService().findClientebyToken(token);

                if (_objClient == null) throw new Exception("No existe la sesion");

                _objSaveFood.id_user = _objClient.idUsuario;

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    if (_tb_userFood == null)
                    {

                        calorieCounterBD.tb_userFood.Add(
                            new tb_userFood
                            {
                                id_user     = _objSaveFood.id_user,
                                id_food     = _objSaveFood.id_food,
                                count       = _objSaveFood.amount,
                                date        = auxDate,
                                id_scale    = _objSaveFood.scale,
                                id_meal     = _objSaveFood.meal
                            }
                        );

                        if (favorite) 
                        {
                            if (!calorieCounterBD.tb_favoriteFood.Any(a => a.id_food == _objSaveFood.id_food && a.id_user == _objSaveFood.id_user)) {
                                calorieCounterBD.tb_favoriteFood.Add(
                                       new tb_favoriteFood
                                       {
                                           id_user = _objSaveFood.id_user,
                                           id_food = _objSaveFood.id_food
                                       }
                                   );
                            }
                           
                        }

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

        /// <summary>
        /// actualiza la comida seleccionada segun parametros
        /// </summary>
        /// <param name="token"></param>
        /// <param name="idFood"></param>
        /// <param name="amount"></param>
        /// <param name="scale"></param>
        /// <param name="meal"></param>
        /// <param name="favorite"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public bool updateFood(string token, int idFood, double amount, int scale, int meal, bool favorite, string fecha)
        {

            objSaveFood _objSaveFood = null;
            tb_favoriteFood _tb_favoriteFood = null;
            tb_userFood _tb_userFood = null;
            DateTime auxDate = (fecha == "" ? DateTime.Now.Date : Convert.ToDateTime(fecha).Date);
            try
            {
                _objSaveFood = new objSaveFood {
                    token   = token,
                    id_food = idFood,
                    amount = amount,
                    scale   = scale,
                    meal    = meal,
                    id_user = new clientService().findClientebyToken(token).idUsuario
                };

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    _tb_userFood = calorieCounterBD.tb_userFood.Where(w => w.id_food == _objSaveFood.id_food && w.id_user == _objSaveFood.id_user && w.date == auxDate).FirstOrDefault();

                    if (_tb_userFood!=null){
                        _tb_userFood.count = _objSaveFood.amount;
                        _tb_userFood.id_scale = _objSaveFood.scale;
                        _tb_userFood.id_meal = _tb_userFood.id_meal;

                        _tb_favoriteFood = calorieCounterBD.tb_favoriteFood.Where(w => w.id_food == _objSaveFood.id_food && w.id_user == _objSaveFood.id_user).FirstOrDefault();

                        if (favorite)
                        {

                            if (_tb_favoriteFood == null)
                            {
                                calorieCounterBD.tb_favoriteFood.Add(
                                        new tb_favoriteFood
                                        {
                                            id_user = _objSaveFood.id_user,
                                            id_food = _objSaveFood.id_food
                                        }
                                    );
                            }
                        }
                        else {
                            if (_tb_favoriteFood != null){
                                calorieCounterBD.tb_favoriteFood.Remove(_tb_favoriteFood);
                            }
                        }
                        calorieCounterBD.SaveChanges();
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return false;
        }

        /// <summary>
        /// elimina la comida seleccinada segun parametros
        /// </summary>
        /// <param name="token"></param>
        /// <param name="idFood"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public bool deleteFood(string token, int idFood, string fecha)
        {
            objClient _objClient = null;
            tb_userFood _tb_userFood = null;
            DateTime auxDate = (fecha == "" ? DateTime.Now.Date : Convert.ToDateTime(fecha).Date);
            try
            {
                _objClient = new clientService().findClientebyToken(token);

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    _tb_userFood = calorieCounterBD.tb_userFood.Where(w => w.id_food == idFood && w.id_user == _objClient.idUsuario && w.date == auxDate).FirstOrDefault();

                    if (_tb_userFood != null) {
                        calorieCounterBD.tb_userFood.Remove(_tb_userFood);
                        calorieCounterBD.SaveChanges();
                        return true;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// obtiene la scala y los valores en gramos
        /// </summary>
        /// <param name="id_food"></param>
        /// <returns></returns>
        public List<objUtiliti> GetListScale(int id_food) 
        {
            try
            {
                List<objUtiliti> resp = null;

                double? aux = this.GetGramoskalorias(id_food);

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    resp =
                        calorieCounterBD.tb_food
                        .Join(calorieCounterBD.tb_classificationFood, food => food.id_food, claFood => claFood.id_food, (food, claFood) => new { food, claFood })
                        .Join(calorieCounterBD.tb_classificationDetail, tbClaFo => tbClaFo.claFood.id_classificationFood, tbClaDe => tbClaDe.id_classificationFood, (tbClaFo, tbClaDe) => new { tbClaFo, tbClaDe })
                        .Join(calorieCounterBD.tb_detailFood, claDe_detFood => claDe_detFood.tbClaDe.id_detailFood, detFood => detFood.id_detailFood, (claDe_detFood, detFood) => new { claDe_detFood, detFood })
                        .Join(calorieCounterBD.tb_detailFoodColumn, detFood_detFoodCol => detFood_detFoodCol.detFood.id_detailFood, detFoodCol => detFoodCol.id_detailFood, (detFood_detFoodCol, detFoodCol) => new { detFood_detFoodCol, detFoodCol })
                        .Join(calorieCounterBD.tb_columnsFood, detFoodCol_colFood => detFoodCol_colFood.detFoodCol.id_columnsfood, colFood => colFood.id_columnsfood, (detFoodCol_colFood, colFood) => new { detFoodCol_colFood, colFood })
                        .Where(w => w.detFoodCol_colFood.detFood_detFoodCol.claDe_detFood.tbClaFo.food.id_food == id_food)
                        .GroupBy(g => g.colFood.descripcion)
                        .Select(s => new { description = s.Select(se => se.colFood.descripcion).FirstOrDefault(), id = s.Select(sel=> sel.colFood.id_columnsfood).FirstOrDefault() })
                        .AsEnumerable()
                        .Select(s => new objUtiliti
                        {
                            id  = s.id,
                            description = new Regex(@"-?[0-9]*(\.|)[0-9]+(\s|)g", RegexOptions.IgnoreCase).Replace(s.description, "").Trim(),
                            value = new Regex(@"-?[0-9]*(\.|)[0-9]+(\s|)g", RegexOptions.IgnoreCase).Match(s.description).Value,
                            value1 = ((Double.Parse((new Regex(@"-?[0-9]*(\.|)[0-9]+(\s|)g", RegexOptions.IgnoreCase).Match(s.description).Value).Replace("g", ""), CultureInfo.InvariantCulture) * (aux)) / 100),

                        }).ToList();

                    return resp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// obtiene las calorias por cada 100 gramos de cada comida
        /// </summary>
        /// <param name="id_food"></param>
        /// <returns></returns>
        public double? GetGramoskalorias(int? id_food) 
        {
            try
            {
                double? value = 0;

                using (calorieCounterBD = new CalorieCounterEntities())
                {
                    calorieCounterBD.Database.Connection.Open();

                    value =
                        calorieCounterBD.tb_classificationFood
                        .Join(calorieCounterBD.tb_classificationDetail, a => a.id_classificationFood, b => b.id_classificationFood, (a, b) => new { a, b })
                        .Join(calorieCounterBD.tb_detailFood, c => c.b.id_detailFood, d => d.id_detailFood, (c, d) => new { c, d })
                        .Join(calorieCounterBD.tb_detailFoodColumn, e => e.d.id_detailFood, f => f.id_detailFood, (e, f) => new { e, f })
                        .Where(w => w.e.c.a.id_food == id_food && w.e.c.a.descripcion.Contains("Proximates") && w.e.d.descripcion.Contains("Energy"))
                        .Select(s =>  s.f.foodValue).FirstOrDefault();

                }
                return value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        protected void Dispose(Boolean free)
        {
            if (free)
            {
                if (this.calorieCounterBD != null)
                {
                    this.calorieCounterBD.Dispose();
                    this.calorieCounterBD = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~foodService()
        {
            Dispose();
        }

    }

}



    
