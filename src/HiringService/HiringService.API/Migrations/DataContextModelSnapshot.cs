﻿// <auto-generated />
using System;
using HiringService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HiringService.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HiringService.Domain.Entities.Candidate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CV")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("NextStageTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("HiringService.Domain.Entities.HiringStage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CandidateId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("HiringStageNameId")
                        .HasColumnType("integer");

                    b.Property<int>("IntervierId")
                        .HasColumnType("integer");

                    b.Property<bool>("PassedSuccessfully")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("HiringStageNameId");

                    b.HasIndex("IntervierId");

                    b.ToTable("HiringStages");
                });

            modelBuilder.Entity("HiringService.Domain.Entities.HiringStageName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("HiringStageNames");
                });

            modelBuilder.Entity("HiringService.Domain.Entities.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("HiringService.Domain.Entities.HiringStage", b =>
                {
                    b.HasOne("HiringService.Domain.Entities.Candidate", "Candidate")
                        .WithMany("HiringStages")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HiringService.Domain.Entities.HiringStageName", "HiringStageName")
                        .WithMany("HiringStages")
                        .HasForeignKey("HiringStageNameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HiringService.Domain.Entities.Worker", "Intervier")
                        .WithMany("HiringStages")
                        .HasForeignKey("IntervierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("HiringStageName");

                    b.Navigation("Intervier");
                });

            modelBuilder.Entity("HiringService.Domain.Entities.Candidate", b =>
                {
                    b.Navigation("HiringStages");
                });

            modelBuilder.Entity("HiringService.Domain.Entities.HiringStageName", b =>
                {
                    b.Navigation("HiringStages");
                });

            modelBuilder.Entity("HiringService.Domain.Entities.Worker", b =>
                {
                    b.Navigation("HiringStages");
                });
#pragma warning restore 612, 618
        }
    }
}
