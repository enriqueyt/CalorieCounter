using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Objetos
{
    public class objSesion
    {
        public int idUsuario            { get; set; }

        public string sesion            { get; set; }

        public DateTime fechaInicio     { get; set; }

        public DateTime fechaUltimaOp   { get; set; }
    }
}
