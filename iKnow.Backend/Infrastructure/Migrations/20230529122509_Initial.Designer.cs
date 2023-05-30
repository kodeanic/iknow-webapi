﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230529122509_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Domain.Entities.Constellations.Constellation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SubtopicId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubtopicId");

                    b.ToTable("Constellations");
                });

            modelBuilder.Entity("Domain.Entities.Constellations.Line", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ConstellationId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("StarLeftNumber")
                        .HasColumnType("int");

                    b.Property<int>("StarRightNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConstellationId");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("Domain.Entities.Constellations.Star", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("ConstellationId")
                        .HasColumnType("int");

                    b.Property<bool>("IsClicked")
                        .HasColumnType("bit");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ConstellationId");

                    b.ToTable("Stars");
                });

            modelBuilder.Entity("Domain.Entities.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Options")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SubtopicId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubtopicId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Domain.Entities.Progress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CompletedExercises")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<int>("SubtopicId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubtopicId");

                    b.HasIndex("UserId");

                    b.ToTable("Progresses");
                });

            modelBuilder.Entity("Domain.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Domain.Entities.Subtopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Theory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TopicId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Subtopics");
                });

            modelBuilder.Entity("Domain.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PictureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.UserRefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRefreshTokens");
                });

            modelBuilder.Entity("Domain.Entities.Constellations.Constellation", b =>
                {
                    b.HasOne("Domain.Entities.Subtopic", null)
                        .WithMany("Constellations")
                        .HasForeignKey("SubtopicId");
                });

            modelBuilder.Entity("Domain.Entities.Constellations.Line", b =>
                {
                    b.HasOne("Domain.Entities.Constellations.Constellation", null)
                        .WithMany("Lines")
                        .HasForeignKey("ConstellationId");
                });

            modelBuilder.Entity("Domain.Entities.Constellations.Star", b =>
                {
                    b.HasOne("Domain.Entities.Constellations.Constellation", null)
                        .WithMany("Stars")
                        .HasForeignKey("ConstellationId");
                });

            modelBuilder.Entity("Domain.Entities.Exercise", b =>
                {
                    b.HasOne("Domain.Entities.Subtopic", null)
                        .WithMany("Exercises")
                        .HasForeignKey("SubtopicId");
                });

            modelBuilder.Entity("Domain.Entities.Progress", b =>
                {
                    b.HasOne("Domain.Entities.Subtopic", "Subtopic")
                        .WithMany()
                        .HasForeignKey("SubtopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subtopic");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Subtopic", b =>
                {
                    b.HasOne("Domain.Entities.Topic", null)
                        .WithMany("Subtopics")
                        .HasForeignKey("TopicId");
                });

            modelBuilder.Entity("Domain.Entities.Topic", b =>
                {
                    b.HasOne("Domain.Entities.Subject", null)
                        .WithMany("Topics")
                        .HasForeignKey("SubjectId");
                });

            modelBuilder.Entity("Domain.Entities.UserRefreshToken", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Constellations.Constellation", b =>
                {
                    b.Navigation("Lines");

                    b.Navigation("Stars");
                });

            modelBuilder.Entity("Domain.Entities.Subject", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("Domain.Entities.Subtopic", b =>
                {
                    b.Navigation("Constellations");

                    b.Navigation("Exercises");
                });

            modelBuilder.Entity("Domain.Entities.Topic", b =>
                {
                    b.Navigation("Subtopics");
                });
#pragma warning restore 612, 618
        }
    }
}