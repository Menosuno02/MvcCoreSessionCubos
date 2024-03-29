﻿using Microsoft.EntityFrameworkCore;
using MvcCoreSessionCubos.Models;

namespace MvcCoreSessionCubos.Context
{
    public class CubosContext : DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options)
            : base(options) { }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<Compra> Compras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compra>()
                .HasKey(pc => new { pc.IdCompra, pc.NombreCubo });
        }
    }
}
