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
    
    public partial class tb_exerciseImage
    {
        public int id_exerciseImage { get; set; }
        public int id_exercise { get; set; }
        public string image { get; set; }
        public int isMain { get; set; }
    
        public virtual tb_exercise tb_exercise { get; set; }
    }
}
