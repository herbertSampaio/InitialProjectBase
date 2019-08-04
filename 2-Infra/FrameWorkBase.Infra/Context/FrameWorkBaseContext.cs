using FrameWorkBase.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Infra.Context
{
    public class FrameWorkBaseContext:DbContext
    {
        public DbSet<UsuarioDomain> Usuario { get; set; }


        public FrameWorkBaseContext(DbContextOptions<FrameWorkBaseContext> options)
            : base(options)
        { }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=FrameWorkBase.db");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
