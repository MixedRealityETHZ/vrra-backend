﻿using Microsoft.EntityFrameworkCore;
using RoomArrangementsBackend.Models;

namespace RoomArrangementsBackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Obj> Objects { get; set; }

        public DbSet<Model> Models { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Obj>().ToTable("Object");
            modelBuilder.Entity<Model>().ToTable("Model");
        }
    }
}