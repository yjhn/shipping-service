﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using shipping_service.Persistence.Database;

#nullable disable

namespace shipping_service.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220425080402_remove Name from Courier, autogenerate IDs")]
    partial class removeNamefromCourierautogenerateIDs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("shipping_service.Persistence.Entities.Courier", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<byte[]>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("shipping_service.Persistence.Entities.PostMachine", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PostMachines");
                });

            modelBuilder.Entity("shipping_service.Persistence.Entities.Sender", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<byte[]>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Senders");
                });

            modelBuilder.Entity("shipping_service.Persistence.Entities.Shipment", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("CourierId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(1000)");

                    b.Property<decimal>("DestinationMachineId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("SenderId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("SourceMachineId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CourierId");

                    b.HasIndex("DestinationMachineId");

                    b.HasIndex("SenderId");

                    b.HasIndex("SourceMachineId");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("shipping_service.Persistence.Entities.Shipment", b =>
                {
                    b.HasOne("shipping_service.Persistence.Entities.Courier", "Courier")
                        .WithMany("CurrentShipments")
                        .HasForeignKey("CourierId");

                    b.HasOne("shipping_service.Persistence.Entities.PostMachine", "DestinationMachine")
                        .WithMany("ShipmentsWithThisDestination")
                        .HasForeignKey("DestinationMachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("shipping_service.Persistence.Entities.Sender", "Sender")
                        .WithMany("Shipments")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("shipping_service.Persistence.Entities.PostMachine", "SourceMachine")
                        .WithMany("ShipmentsWithThisSource")
                        .HasForeignKey("SourceMachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Courier");

                    b.Navigation("DestinationMachine");

                    b.Navigation("Sender");

                    b.Navigation("SourceMachine");
                });

            modelBuilder.Entity("shipping_service.Persistence.Entities.Courier", b =>
                {
                    b.Navigation("CurrentShipments");
                });

            modelBuilder.Entity("shipping_service.Persistence.Entities.PostMachine", b =>
                {
                    b.Navigation("ShipmentsWithThisDestination");

                    b.Navigation("ShipmentsWithThisSource");
                });

            modelBuilder.Entity("shipping_service.Persistence.Entities.Sender", b =>
                {
                    b.Navigation("Shipments");
                });
#pragma warning restore 612, 618
        }
    }
}
