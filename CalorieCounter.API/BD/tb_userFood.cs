//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CalorieCounter.API.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_userFood
    {
        public int id_userFood { get; set; }
        public Nullable<int> id_user { get; set; }
        public Nullable<int> id_food { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<double> count { get; set; }
        public Nullable<int> id_scale { get; set; }
        public Nullable<int> id_meal { get; set; }
    
        public virtual tb_food tb_food { get; set; }
        public virtual tb_meal tb_meal { get; set; }
        public virtual tb_usuario tb_usuario { get; set; }
    }
}
