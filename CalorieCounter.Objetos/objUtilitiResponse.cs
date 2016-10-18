using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objUtilitiResponse : objBasicResponse
    {
        public List<objUtility> utility { get; set; }

        public bool flag { get; set; }
    }

    public class objUtility {
        public int id { get; set; }

        public string description { get; set; }

        public string value { get; set; }

        public double? value1 { get; set; }

    }
}
