//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
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
    }
}
