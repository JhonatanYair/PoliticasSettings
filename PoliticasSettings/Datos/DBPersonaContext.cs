
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PoliticasSettings.Datos.Configurations;
using PoliticasSettings.Models;
using System;
using System.Collections.Generic;
namespace PoliticasSettings.Datos
{
    public partial class DBPersonaContext : DbContext
    {
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<Hijo> Hijo { get; set; }
        public virtual DbSet<Padre> Padre { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }

        public DBPersonaContext(DbContextOptions<DBPersonaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.GeneroConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.HijoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PadreConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PersonaConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
