﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RememberBoxVF.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class evento_wtcEntities10 : DbContext
    {
        public evento_wtcEntities10()
            : base("name=evento_wtcEntities10")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<clientesexpo> clientesexpo { get; set; }
        public virtual DbSet<contratos> contratos { get; set; }
        public virtual DbSet<evento> evento { get; set; }
        public virtual DbSet<jardin> jardin { get; set; }
        public virtual DbSet<servicios> servicios { get; set; }
        public virtual DbSet<usuarios> usuarios { get; set; }
    }
}