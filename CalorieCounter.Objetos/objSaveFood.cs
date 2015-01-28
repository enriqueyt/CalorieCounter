using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objSaveFood
    {
        public string token { get; set; }

        public int id_food { get; set; }

        public int id_user { get; set; }

        public DateTime? date { get; set; }

        public double amount { get; set; }

        public int scale { get; set; }

        public int meal { get; set; }
    }
}
