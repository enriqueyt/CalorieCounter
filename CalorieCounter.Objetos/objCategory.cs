using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos {
    public class objCategory {
        public int id_category { get; set; }
        public string name { get; set; }
        public int activo { get; set; }

        public virtual List<objSubCategory> list_subCategory { get; set; }
    }
}
