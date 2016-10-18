using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos {
    public class objWorkoutLog {
        public int id_workoutLog { get; set; }
        public int id_user { get; set; }
        public int id_exercise { get; set; }
        public DateTime date { get; set; }
        public Nullable<int> reps { get; set; }
        public double minutes { get; set; }
        public Nullable<int> sets { get; set; }
        public Nullable<double> weight { get; set; }
        public byte favorito { get; set; }

        public List<objExercise> list_exercise { get; set; }
        public objExercise objexercise { get; set; }        
    }
}
