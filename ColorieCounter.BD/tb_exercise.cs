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
    
    public partial class tb_exercise
    {
        public tb_exercise()
        {
            this.tb_exerciseComment = new HashSet<tb_exerciseComment>();
            this.tb_exerciseImage = new HashSet<tb_exerciseImage>();
            this.tb_favoriteExercise = new HashSet<tb_favoriteExercise>();
            this.tb_workoutLog = new HashSet<tb_workoutLog>();
        }
    
        public int id_exercise { get; set; }
        public int id_category { get; set; }
        public System.DateTime creation_date { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double value { get; set; }
        public int calculateType { get; set; }
    
        public virtual tb_subCategory tb_subCategory { get; set; }
        public virtual ICollection<tb_exerciseComment> tb_exerciseComment { get; set; }
        public virtual ICollection<tb_exerciseImage> tb_exerciseImage { get; set; }
        public virtual ICollection<tb_favoriteExercise> tb_favoriteExercise { get; set; }
        public virtual ICollection<tb_workoutLog> tb_workoutLog { get; set; }
    }
}
