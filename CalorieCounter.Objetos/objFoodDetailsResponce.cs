using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objFoodDetailsResponce : objBasicResponse
    {
        public List<objFoodDetails> listFoodDetail  { get; set; }
        public List<objColumnFood> listColumnFood   { get; set; }
    }
}
