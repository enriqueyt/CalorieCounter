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
    
    public partial class tb_category
    {
        public tb_category()
        {
            this.tb_subCategory = new HashSet<tb_subCategory>();
        }
    
        public int id_category { get; set; }
        public string name { get; set; }
        public int activo { get; set; }
    
        public virtual ICollection<tb_subCategory> tb_subCategory { get; set; }
    }
}