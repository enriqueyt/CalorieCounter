using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos {
    public class objExerciseImage {
        public int id_exerciseImage { get; set; }
        public int id_exercise { get; set; }
        public string image { get; set; }
        public int isMain { get; set; }
    }
}
