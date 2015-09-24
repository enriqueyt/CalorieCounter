using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos {
    public class objFavoriteExercise {
        public int id_favoriteExercise { get; set; }
        public int id_user { get; set; }
        public int id_exercise { get; set; }

        public List<objExercise> list_exercise { get; set; }
    }
}
