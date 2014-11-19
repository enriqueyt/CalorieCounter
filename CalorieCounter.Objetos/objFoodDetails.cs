using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objFoodDetails
    {
        public int foodId { get; set; }

        public int foodReference { get; set; }

        public int? ndb { get; set; }

        public string unitType { get; set; }

        public double? val { get; set; }

        public string valGroup { get; set; }

        public string valName { get; set; }

        public string valType { get; set; }
    }
}
