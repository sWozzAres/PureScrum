﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Scrum.Api.Domain.Infrastructure;

#nullable disable

namespace Scrum.Api.Domain.Infrastructure.ScrumDb
{
    [DbContext(typeof(ScrumDbContext))]
    [Migration("20230822094201_Initial2")]
    partial class Initial2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Scrum.Api.Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("Owner")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "IX_Products_Name")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Scrum.Api.Domain.ProductBacklogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("DeliveryDate")
                        .HasColumnType("date");

                    b.Property<int>("EstimatedDays")
                        .HasColumnType("int");

                    b.Property<bool>("IsFixedDeliveryDate")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ProductBacklogItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProductIncrementId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Roi")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("Value")
                        .HasColumnType("int");

                    b.Property<string>("ValueDescription")
                        .IsRequired()
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductBacklogItemId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductIncrementId");

                    b.ToTable("ProductBacklogItems");
                });

            modelBuilder.Entity("Scrum.Api.Domain.ProductIncrement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("ExpectedDeliveryDate")
                        .HasColumnType("date");

                    b.Property<Guid>("Owner")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SprintGoal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProductIncrements");
                });

            modelBuilder.Entity("Scrum.Api.Domain.SprintBacklogItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDone")
                        .HasColumnType("bit");

                    b.Property<Guid>("ProductBacklogItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SprintBacklogItemId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProductBacklogItemId");

                    b.HasIndex("SprintBacklogItemId");

                    b.ToTable("SprintBacklogItems");
                });

            modelBuilder.Entity("Scrum.Api.Domain.ProductBacklogItem", b =>
                {
                    b.HasOne("Scrum.Api.Domain.ProductBacklogItem", null)
                        .WithMany("DependsOn")
                        .HasForeignKey("ProductBacklogItemId");

                    b.HasOne("Scrum.Api.Domain.Product", "Product")
                        .WithMany("BacklogItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Scrum.Api.Domain.ProductIncrement", "ProductIncrement")
                        .WithMany("BacklogItems")
                        .HasForeignKey("ProductIncrementId");

                    b.Navigation("Product");

                    b.Navigation("ProductIncrement");
                });

            modelBuilder.Entity("Scrum.Api.Domain.SprintBacklogItem", b =>
                {
                    b.HasOne("Scrum.Api.Domain.ProductBacklogItem", "ProductBacklogItem")
                        .WithMany("SprintBacklogItems")
                        .HasForeignKey("ProductBacklogItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Scrum.Api.Domain.SprintBacklogItem", null)
                        .WithMany("DependsOn")
                        .HasForeignKey("SprintBacklogItemId");

                    b.Navigation("ProductBacklogItem");
                });

            modelBuilder.Entity("Scrum.Api.Domain.Product", b =>
                {
                    b.Navigation("BacklogItems");
                });

            modelBuilder.Entity("Scrum.Api.Domain.ProductBacklogItem", b =>
                {
                    b.Navigation("DependsOn");

                    b.Navigation("SprintBacklogItems");
                });

            modelBuilder.Entity("Scrum.Api.Domain.ProductIncrement", b =>
                {
                    b.Navigation("BacklogItems");
                });

            modelBuilder.Entity("Scrum.Api.Domain.SprintBacklogItem", b =>
                {
                    b.Navigation("DependsOn");
                });
#pragma warning restore 612, 618
        }
    }
}