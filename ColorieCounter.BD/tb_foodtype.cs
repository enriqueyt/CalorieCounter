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
    
    public partial class tb_foodtype
    {
        public tb_foodtype()
        {
            this.tb_food = new HashSet<tb_food>();
        }
    
        public int id_foodtype { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
    
        public virtual ICollection<tb_food> tb_food { get; set; }
    }
}
