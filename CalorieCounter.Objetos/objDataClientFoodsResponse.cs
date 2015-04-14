using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objDataClientFoodsResponse : objBasicResponse
    {
        public List<objDataClientFoods> objDataClientFoods { get; set; }

        public List<objResumenDiario> objResumenDiario { get; set; }
        public int id_meal { get; set; }

        public double? total { get; set; }
        public string meal { get; set; }
    }
}
