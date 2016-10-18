using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.Objetos;
using CalorieCounter.ServicioBD;

namespace CalorieCounter.Controlador {
    public class exerciseController {

        exerciseService ex = new exerciseService();

        /// <summary>
        /// obtiene una lista de categorias
        /// </summary>
        /// <returns></returns>
        public List<objCategory> getCategories() {
            try {
                return ex.getCategories();
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// obtiene lista de favoritos de un cliente
        /// </summary>
        /// <param name="token">guid</param>
        /// <returns></returns>
        public List<objFavoriteExercise> favoriteExercise(string token) {
            try {
                return ex.favoriteExercise(token);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// almacena la preferencia de actividades de un cliente
        /// </summary>
        /// <param name="token">guid</param>
        /// <param name="fe">obj proviene de una formulario</param>
        /// <returns></returns>
        public string saveFavoriteExercise(string token, objFavoriteExercise fe) {
            try {
                return ex.saveFavoriteExercise(token, fe).ToString();
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// elimina un ejercicio de la lista de favoritos
        /// </summary>
        /// <param name="token">guid</param>
        /// <returns></returns>
        public string deleteFavoriteExercise(string token, int id_exercise) {
            try {
                return ex.deleteFavoriteExercise(token, id_exercise).ToString();
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Almacena comentarios de un ejercicio
        /// </summary>
        /// <param name="ec">obj proviene de una formulario</param>
        /// <returns></returns>
        public string saveCommentExercise(objExerciseComment ec) {
            try {
                return ex.saveCommentExercise(ec).ToString();
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Almacena la actividad seleccionada por un cliente.
        /// </summary>
        /// <param name="token">guid</param>
        /// <param name="wlog">obj proviene de una formulario</param>
        /// <returns></returns>
        public string saveWorkoutLog(string token, objWorkoutLog wlog) {
            try {
                return ex.saveWorkoutLog(token, wlog).ToString();
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// obtiene una lista de trabajos de un user.
        /// </summary>
        /// <param name="token">guid</param>
        /// <param name="date">opcional, obtiene por fecha seleccionada</param>
        /// <returns></returns>
        public List<objWorkoutLog> getWorkoutLogs(string token, DateTime? date = null) {
            try {
                return ex.getWorkoutLogs(token, date);
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }


    }
}
