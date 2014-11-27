using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objUtilitiResponse : objBasicResponse
    {
        public List<objUtiliti> utiliti { get; set; }
    }

    public class objUtiliti 
    {
        public int id { get; set; }
        public string description { get; set; }
    }
}
