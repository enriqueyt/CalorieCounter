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
        public Nullable<DateTime> reps { get; set; }

        public List<objExercise> list_exercise { get; set; }
        public objExercise objexercise { get; set; }        
    }
}
