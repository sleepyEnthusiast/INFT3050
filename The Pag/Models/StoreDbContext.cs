﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace The_Pag.Models;

public partial class StoreDbContext : DbContext
{
    public StoreDbContext()
    {
    }

    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookGenre> BookGenres { get; set; }

    public virtual DbSet<BookGenreNew> BookGenreNews { get; set; }

    public virtual DbSet<GameGenre> GameGenres { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Patron> Patrons { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsInOrder> ProductsInOrders { get; set; }

    public virtual DbSet<Source> Sources { get; set; }

    public virtual DbSet<Stocktake> Stocktakes { get; set; }

    public virtual DbSet<To> Tos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.Property(e => e.GenreId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasOne(d => d.CustomerNavigation).WithMany(p => p.Orders).HasConstraintName("FK_Orders_TO");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.GenreNavigation).WithMany(p => p.Products).HasConstraintName("FK_Product_Genre");

            entity.HasOne(d => d.LastUpdatedByNavigation).WithMany(p => p.Products).HasConstraintName("FK_Product_Users");
        });

        modelBuilder.Entity<ProductsInOrder>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany().HasConstraintName("FK_ProductsInOrders_Orders");

            entity.HasOne(d => d.Produkt).WithMany().HasConstraintName("FK_ProductsInOrders_Stocktake");
        });

        modelBuilder.Entity<Source>(entity =>
        {
            entity.HasOne(d => d.GenreNavigation).WithMany(p => p.Sources).HasConstraintName("FK_Source_Genre");
        });

        modelBuilder.Entity<Stocktake>(entity =>
        {
            entity.HasOne(d => d.Product).WithMany(p => p.Stocktakes).HasConstraintName("FK_Stocktake_Product");

            entity.HasOne(d => d.Source).WithMany(p => p.Stocktakes).HasConstraintName("FK_Stocktake_Source");
        });

        modelBuilder.Entity<To>(entity =>
        {
            entity.HasOne(d => d.Patron).WithMany(p => p.Tos).HasConstraintName("FK_TO_Patrons");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserName).HasName("PK_Users");

            entity.Property(e => e.UserId).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
