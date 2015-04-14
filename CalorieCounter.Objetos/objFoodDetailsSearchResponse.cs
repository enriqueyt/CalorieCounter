using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objFoodDetailsSearchResponse : objBasicResponse
    {
        public List<objClassificationFood> list_classificationFood { get; set; }

        public List<objClasificationDef> _objClasificationDef { get; set; }

        public List<objUtiliti> listScale { get; set; }

        public int columnsCount { get; set; }

    }


    public class objClasificationDef
    {
        public string name { get; set; }
        public List<List<objFoodDetail>> listObjFoodDetail { get; set; }

    }

    public class objClassificationFood
    {
        public int id { get; set; }

        public string name { get; set; }

        public List<objFoodDetail> list_foodDetails { get; set; }

    }

    public class objFoodDetail
    {
        public int count { get; set; }
        public string descripcion { get; set; }
        public string unit { get; set; }

        public List<objFoodDetailValues> valuesFoodDetail { get; set; }

    }

    public class objFoodDetailValues
    {
        public double? value { get; set; }
    }


}
