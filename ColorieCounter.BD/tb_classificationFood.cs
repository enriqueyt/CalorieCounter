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
    
    public partial class tb_classificationFood
    {
        public tb_classificationFood()
        {
            this.tb_classificationDetail = new HashSet<tb_classificationDetail>();
        }
    
        public int id_classificationFood { get; set; }
        public int id_food { get; set; }
        public string descripcion { get; set; }
        public int activo { get; set; }
    
        public virtual ICollection<tb_classificationDetail> tb_classificationDetail { get; set; }
        public virtual tb_food tb_food { get; set; }
    }
}
