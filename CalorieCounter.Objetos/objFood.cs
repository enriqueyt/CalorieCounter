using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieCounter.BD;

namespace CalorieCounter.Objetos
{
    public class objFood
    {
        public int? id_food          {get; set;}

        public string description   {get; set;}

        public int groupID          {get; set;}

        public string token         { get; set; }

        public int id_user          { get; set; }

        public DateTime? date       { get; set; }

        public double? count         { get; set; }

        public int? scale            { get; set; }

        public string descScale { get; set; }

        public int meal             { get; set; }

    }

    public class objResumenDiario 
    {
        public string description { get; set; }

        public int id { get; set; }

        public double? cant { get; set; }

        public List<objFood> objfood { get; set; }
    }
}
