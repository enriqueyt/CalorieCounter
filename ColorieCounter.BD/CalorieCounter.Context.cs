﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CalorieCounterEntities : DbContext
    {
        public CalorieCounterEntities()
            : base("name=CalorieCounterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tb_classificationDetail> tb_classificationDetail { get; set; }
        public DbSet<tb_classificationFood> tb_classificationFood { get; set; }
        public DbSet<tb_cliente> tb_cliente { get; set; }
        public DbSet<tb_columnsFood> tb_columnsFood { get; set; }
        public DbSet<tb_detailFood> tb_detailFood { get; set; }
        public DbSet<tb_detailFoodColumn> tb_detailFoodColumn { get; set; }
        public DbSet<tb_food> tb_food { get; set; }
        public DbSet<tb_foodtype> tb_foodtype { get; set; }
        public DbSet<tb_measurementType> tb_measurementType { get; set; }
        public DbSet<tb_perfil> tb_perfil { get; set; }
        public DbSet<tb_sesion> tb_sesion { get; set; }
        public DbSet<tb_usuario> tb_usuario { get; set; }
        public DbSet<tb_usuarioPerfil> tb_usuarioPerfil { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<vw_FoodDetail> vw_FoodDetail { get; set; }
    }
}