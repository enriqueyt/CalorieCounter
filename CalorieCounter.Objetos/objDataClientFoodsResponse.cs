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

        public int id_meal { get; set; }

        public string meal { get; set; }
    }
}
