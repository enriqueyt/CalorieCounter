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
    
    public partial class tb_columnsFood
    {
        public tb_columnsFood()
        {
            this.tb_detailFoodColumn = new HashSet<tb_detailFoodColumn>();
        }
    
        public int id_columnsfood { get; set; }
        public string descripcion { get; set; }
        public int activo { get; set; }
    
        public virtual ICollection<tb_detailFoodColumn> tb_detailFoodColumn { get; set; }
    }
}