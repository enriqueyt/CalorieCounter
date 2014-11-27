using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objClient
    {
        public int idCliente        { get; set; }

        public int idUsuario        { get; set; }

        public string nombre        { get; set; }

        public string apellido      { get; set; }

        public string correo        { get; set; }

        public string tipoDocumento { get; set; }

        public string documento     { get; set; }

        public string telefono      { get; set; }
    }
}
