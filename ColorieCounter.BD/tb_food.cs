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
    
    public partial class tb_food
    {
        public tb_food()
        {
            this.tb_classificationFood = new HashSet<tb_classificationFood>();
            this.tb_favoriteFood = new HashSet<tb_favoriteFood>();
            this.tb_userFood = new HashSet<tb_userFood>();
        }
    
        public int id_food { get; set; }
        public int id_foodtype { get; set; }
        public int id_reference { get; set; }
        public string description { get; set; }
        public System.DateTime datecreated { get; set; }
        public bool active { get; set; }
    
        public virtual ICollection<tb_classificationFood> tb_classificationFood { get; set; }
        public virtual ICollection<tb_favoriteFood> tb_favoriteFood { get; set; }
        public virtual tb_foodtype tb_foodtype { get; set; }
        public virtual ICollection<tb_userFood> tb_userFood { get; set; }
    }
}
