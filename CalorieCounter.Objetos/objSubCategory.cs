using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos {
    public class objSubCategory {
        public int id_subCategory { get; set; }
        public string name { get; set; }
        public int id_category { get; set; }
        public int activo { get; set; }

        public List<objExercise> List_exercise { get; set; }

        public objCategory objcategory { get; set; }

    }
}
