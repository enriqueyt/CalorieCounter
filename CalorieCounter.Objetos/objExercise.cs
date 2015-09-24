using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos {
    public class objExercise {

        public int id_exercise { get; set; }
        public int id_category { get; set; }
        public DateTime creation_date { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double value { get; set; }
        public int calculateType { get; set; }

        public List<objSubCategory> list_subCategory { get; set; }
        public List<objExerciseComment> list_exerciseComment { get; set; }
        public List<objExerciseImage> list_exerciseImage { get; set; }
        public List<objFavoriteExercise> list_favoriteExercise { get; set; }
        public List<objWorkoutLog> list_workoutLog { get; set; }

    }

}
