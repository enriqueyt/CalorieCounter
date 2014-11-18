using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objFoodTypesResponse : objBasicResponse
    {
        public List<objFoodType> FoodTypes = new List<objFoodType>();
    }
}
