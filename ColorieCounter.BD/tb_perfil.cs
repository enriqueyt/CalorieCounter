//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CalorieCounter.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_perfil
    {
        public tb_perfil()
        {
            this.tb_usuarioPerfil = new HashSet<tb_usuarioPerfil>();
        }
    
        public int id_perfil { get; set; }
        public string perfil { get; set; }
        public string descripcion { get; set; }
        public System.DateTime fechaRegistro { get; set; }
        public int activo { get; set; }
    
        public virtual ICollection<tb_usuarioPerfil> tb_usuarioPerfil { get; set; }
    }
}
