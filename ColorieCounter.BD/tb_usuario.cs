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
    
    public partial class tb_usuario
    {
        public tb_usuario()
        {
            this.tb_sesion = new HashSet<tb_sesion>();
            this.tb_usuarioPerfil = new HashSet<tb_usuarioPerfil>();
            this.tb_userFood = new HashSet<tb_userFood>();
            this.tb_favoriteFood = new HashSet<tb_favoriteFood>();
        }
    
        public int id_usuario { get; set; }
        public string usuario { get; set; }
        public string usuarioCorreo { get; set; }
        public string usuarioFacebook { get; set; }
        public string usuarioTwiter { get; set; }
        public string contrasena { get; set; }
        public System.DateTime fechaRegistro { get; set; }
        public int id_cliente { get; set; }
        public int activo { get; set; }
        public string validateToken { get; set; }
    
        public virtual tb_cliente tb_cliente { get; set; }
        public virtual ICollection<tb_sesion> tb_sesion { get; set; }
        public virtual ICollection<tb_usuarioPerfil> tb_usuarioPerfil { get; set; }
        public virtual ICollection<tb_userFood> tb_userFood { get; set; }
        public virtual ICollection<tb_favoriteFood> tb_favoriteFood { get; set; }
    }
}
