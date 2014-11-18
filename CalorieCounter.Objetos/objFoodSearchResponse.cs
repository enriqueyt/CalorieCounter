using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.Objetos;

namespace CalorieCounter.Objetos
{
    public class objFoodSearchResponse : objBasicResponse
    {
        public List<objFood> Foods = new List<objFood>();
    }
}
