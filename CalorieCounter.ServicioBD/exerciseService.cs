using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;
using CalorieCounter.Objetos;

namespace CalorieCounter.ServicioBD
{
    public class exerciseService{

        private CalorieCounterEntities calorieCounterBD = null;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<objCategory> getCategories() {
            try {

                List<objCategory> objCategory = new List<objCategory>();

                using(calorieCounterBD = new CalorieCounterEntities()) {

                    calorieCounterBD.Database.Connection.Open();

                    objCategory = calorieCounterBD
                                        .tb_category
                                        .Select(s => new {
                                            id_category = s.id_category,
                                            name = s.name
                                        }).AsEnumerable()
                                        .Select(s => new objCategory {
                                            id_category = s.id_category,
                                            name = s.name,
                                            list_subCategory = (
                                                    calorieCounterBD.tb_subCategory
                                                        .Where(w => s.id_category == w.id_subCategory)
                                                        .Select(se => new {
                                                            se.id_subCategory,
                                                            se.id_category,
                                                            se.name
                                                        }).AsEnumerable()
                                                        .Select(se => new objSubCategory {
                                                            id_subCategory = se.id_subCategory,
                                                            id_category = se.id_category,
                                                            name = se.name,
                                                            List_exercise = (
                                                                calorieCounterBD.tb_exercise
                                                                    .Where(w => se.id_category == w.id_category)
                                                                    .Select(sel => new {
                                                                        id_exercise = sel.id_exercise,
                                                                        name = sel.name,
                                                                        creation_date = sel.creation_date,
                                                                        description = sel.description,
                                                                        value = sel.value,
                                                                        calculateType = sel.calculateType
                                                                    }).AsEnumerable()
                                                                    .Select(sel => new objExercise {
                                                                        id_exercise = sel.id_exercise,
                                                                        name = sel.name,
                                                                        creation_date = sel.creation_date,
                                                                        description = sel.description,
                                                                        value = sel.value,
                                                                        calculateType = sel.calculateType,
                                                                        list_exerciseComment = (
                                                                            calorieCounterBD.tb_exerciseComment
                                                                                .Where(w => sel.id_exercise == w.id_exercise)
                                                                                .Select(sele => new objExerciseComment {
                                                                                    id_exerciseComment = sele.id_exerciseComment,
                                                                                    comment = sele.comment
                                                                                }).ToList()
                                                                            ).ToList(),
                                                                        list_exerciseImage = (
                                                                            calorieCounterBD.tb_exerciseImage
                                                                                .Where(w => w.id_exercise == sel.id_exercise)
                                                                                .Select(sele => new objExerciseImage {
                                                                                    id_exerciseImage = sele.id_exerciseImage,
                                                                                    image = sele.image,
                                                                                    isMain = sele.isMain
                                                                                }).ToList()
                                                                        ).ToList()
                                                                    }).ToList()
                                                                ).ToList()
                                                        }).ToList()
                                                    ).ToList()
                                        }).ToList();
                    return objCategory;
                }

            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<objFavoriteExercise> favoriteExercise(int id) {

            List<objFavoriteExercise> objfavoriteExercise = null;
            try {

                using(calorieCounterBD = new CalorieCounterEntities()) {
                    
                    objfavoriteExercise =
                        calorieCounterBD.tb_favoriteExercise
                        .Join(calorieCounterBD.tb_exercise, fe => fe.id_exercise, e => e.id_exercise, (fe, e) => new { fe, e })
                        .Where(w => w.fe.id_user == id)
                        .Select(s => new {
                            s.fe.id_exercise,
                            s.fe.id_favoriteExercise,
                            s.fe.id_user
                        }).AsEnumerable()
                        .Select(s => new objFavoriteExercise {
                            id_exercise = s.id_exercise,
                            id_favoriteExercise = s.id_favoriteExercise,
                            id_user = s.id_user,
                            list_exercise =
                                calorieCounterBD.tb_exercise
                                    .Where(w => w.id_exercise == s.id_exercise)
                                    .Select(se => new {
                                        se.name,
                                        se.id_category,
                                        se.description,
                                        se.creation_date,
                                        se.calculateType
                                    }).AsEnumerable()
                                    .Select(se => new objExercise {
                                        id_exercise = se.id_category,
                                        description = se.description,
                                        creation_date = se.creation_date,
                                        calculateType = se.calculateType,
                                        id_category = se.id_category,
                                        list_subCategory = 
                                            calorieCounterBD.tb_subCategory
                                                .Where(w => w.id_subCategory == se.id_category)
                                                .Select(sel => new {
                                                    sel.id_subCategory,
                                                    sel.id_category,
                                                    sel.name,
                                                    sel.activo
                                                }).AsEnumerable()
                                                .Select(sel => new objSubCategory {
                                                    id_subCategory = sel.id_subCategory,
                                                    id_category = sel.id_category,
                                                    name = sel.name,
                                                    objcategory = 
                                                        calorieCounterBD.tb_category
                                                            .Where(w => w.id_category == sel.id_category)
                                                            .Select(sele => new objCategory {
                                                                id_category = sele.id_category,
                                                                name = sele.name
                                                            }).FirstOrDefault()
                                                }).ToList()                                        
                                    }).ToList()
                        }).ToList();

                    return objfavoriteExercise;       
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fe"></param>
        /// <returns></returns>
        public int saveFavoriteExercise(string token, objFavoriteExercise fe) {
            try {

                objClient _objClient = new clientService().findClientebyToken(token);

                using(calorieCounterBD = new CalorieCounterEntities()) {

                    calorieCounterBD.Database.Connection.Open();

                    var favoriteExercise =
                        calorieCounterBD
                        .tb_favoriteExercise.Add(
                            new tb_favoriteExercise {
                                id_exercise = fe.id_exercise,
                                id_user = _objClient.idUsuario
                            });

                    calorieCounterBD.SaveChanges();

                    return favoriteExercise.id_user;
                }

            }
            catch(Exception ex)  {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool deleteFavoriteExercise(int id) {
            try {

                using(calorieCounterBD = new CalorieCounterEntities()) {

                    calorieCounterBD.Database.Connection.Open();

                    tb_favoriteExercise fe = calorieCounterBD.tb_favoriteExercise.Where(w => w.id_user == id).FirstOrDefault();

                    if(fe != null) {
                        calorieCounterBD.tb_favoriteExercise.Remove(fe);
                        calorieCounterBD.SaveChanges();
                        return true;
                    }

                    return false;

                }
                
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ec"></param>
        /// <returns></returns>
        public int saveCommentExercise(objExerciseComment ec) {
            try {
                using(calorieCounterBD = new CalorieCounterEntities())
	            {

                    calorieCounterBD.Database.Connection.Open();

                    tb_exerciseComment _ec =
                        calorieCounterBD
                        .tb_exerciseComment.Add(new tb_exerciseComment {
                            id_exercise = ec.id_exercise,
                            comment = ec.comment
                        });

                    calorieCounterBD.SaveChanges();

                    return _ec.id_exerciseComment;
	            }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="wlog"></param>
        /// <returns></returns>
        public bool saveWorkoutLog(string token, objWorkoutLog wlog) {
            try {

                objClient _objClient = new clientService().findClientebyToken(token);

                wlog.id_user = _objClient.idUsuario;

                using(calorieCounterBD = new CalorieCounterEntities()) {

                    calorieCounterBD.Database.Connection.Open();

                    calorieCounterBD.tb_workoutLog.Add(new tb_workoutLog {
                        id_exercise = wlog.id_exercise,
                        id_user = wlog.id_user,
                        date = DateTime.Now,
                        reps = wlog.reps
                    });

                    calorieCounterBD.SaveChanges();

                    return true;
                }
                
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<objWorkoutLog> getWorkoutLogs(string token, DateTime? date = null) {
            try {

                objClient _objClient = null;
                List<objWorkoutLog> wlog = null;

                _objClient = new clientService().findClientebyToken(token);

                date = (date == null ? DateTime.Now.Date : date);

                using(calorieCounterBD = new CalorieCounterEntities()) {

                    wlog = calorieCounterBD.tb_workoutLog
                            .Where(w => w.id_user == _objClient.idUsuario && w.date == date)
                            .Select(s => new {
                                s.id_workoutLog,
                                s.id_exercise,
                                s.id_user,
                                s.reps,
                                s.date
                            }).AsEnumerable()
                            .Select(s => new objWorkoutLog {
                                id_workoutLog = s.id_workoutLog,
                                id_user = s.id_user,
                                date = s.date,
                                id_exercise = s.id_exercise,
                                list_exercise =
                                    calorieCounterBD.tb_exercise
                                        .Where(w => w.id_exercise == s.id_exercise)
                                        .Select(se => new objExercise {
                                            id_exercise = se.id_exercise,
                                            id_category = se.id_category,
                                            description = se.description,
                                            creation_date = se.creation_date,
                                            name = se.name,
                                            value = se.value,
                                            calculateType = se.calculateType
                                        }).ToList<objExercise>()

                            }).ToList<objWorkoutLog>();

                    return wlog;
                }
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }
}
