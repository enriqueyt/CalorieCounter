using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objClient
    {
        public int id_client        { get; set; }

        public int id_usuario        { get; set; }

        public string name        { get; set; }

        public string lastname      { get; set; }

        public string email        { get; set; }

        public string tipoDocumento { get; set; }

        public string documento     { get; set; }

        public string telefono      { get; set; }

        public int? currentWeight { get; set; }

        public int? goalWeight { get; set; }

        public int? height { get; set; }

        public DateTime birthDate { get; set; }

        public int id_sexo { get; set; }

        public int id_activity { get; set; }

        public double activityValue { get; set; }

        public int id_measurementUnits { get; set; }

        public string token { get; set; }

        public string message { get; set; }
       
    }
}
