﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Praca_dyplomowa.Context;

namespace Praca_dyplomowa.Migrations
{
    [DbContext(typeof(ProgramContext))]
    partial class ProgramContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Praca_dyplomowa.Entities.Place", b =>
                {
                    b.Property<int>("PlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Latitude")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Longitude")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PlaceName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PlaceType")
                        .HasColumnType("int");

                    b.Property<int?>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("PlaceId");

                    b.HasIndex("RegionId");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.Region", b =>
                {
                    b.Property<int>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("RegionName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RegionId");

                    b.HasIndex("UserId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Accomplish")
                        .HasColumnType("int");

                    b.Property<int>("DescentPosition")
                        .HasColumnType("int");

                    b.Property<int>("HeightDifference")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Material")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("PlaceId")
                        .HasColumnType("int");

                    b.Property<string>("Rating")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Rings")
                        .HasColumnType("int");

                    b.Property<string>("RouteName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("RouteType")
                        .HasColumnType("int");

                    b.Property<int>("Scale")
                        .HasColumnType("int");

                    b.HasKey("RouteId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.Training", b =>
                {
                    b.Property<int>("TrainingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActivityTime")
                        .HasColumnType("int");

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<string>("EndTime")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("RouteId")
                        .HasColumnType("int");

                    b.Property<string>("StartTime")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TrainingDescription")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TrainingName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TrainingType")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TrainingId");

                    b.HasIndex("RouteId");

                    b.HasIndex("UserId");

                    b.ToTable("Trainings");
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("longblob");

                    b.Property<string>("UserGoogleId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.Place", b =>
                {
                    b.HasOne("Praca_dyplomowa.Entities.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId");
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.Region", b =>
                {
                    b.HasOne("Praca_dyplomowa.Entities.User", null)
                        .WithMany("UserRegions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.Route", b =>
                {
                    b.HasOne("Praca_dyplomowa.Entities.Place", "Place")
                        .WithMany("Routes")
                        .HasForeignKey("PlaceId");
                });

            modelBuilder.Entity("Praca_dyplomowa.Entities.Training", b =>
                {
                    b.HasOne("Praca_dyplomowa.Entities.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");

                    b.HasOne("Praca_dyplomowa.Entities.User", "User")
                        .WithMany("UserTrainings")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
