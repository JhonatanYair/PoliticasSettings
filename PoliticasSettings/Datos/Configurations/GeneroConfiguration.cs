﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoliticasSettings.Datos;
using PoliticasSettings.Models;
using System;
using System.Collections.Generic;

namespace PoliticasSettings.Datos.Configurations
{
    public partial class GeneroConfiguration : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> entity)
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.Genero1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Genero");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Genero> entity);
    }
}
