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
    
    public partial class tb_classificationDetail
    {
        public int id_classificationDetail { get; set; }
        public int id_classificationFood { get; set; }
        public int id_detailFood { get; set; }
    
        public virtual tb_classificationFood tb_classificationFood { get; set; }
        public virtual tb_detailFood tb_detailFood { get; set; }
    }
}
