﻿// <auto-generated />
using System;
using GameEngine.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GameEngine.Migrations
{
    [DbContext(typeof(LudoDbContext))]
    partial class LudoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameEngine.DbModels.GameMember", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GameId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GameMembers");
                });

            modelBuilder.Entity("GameEngine.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int?>("NextToRollDiceUserId")
                        .HasColumnType("int");

                    b.HasKey("GameId");

                    b.HasIndex("NextToRollDiceUserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GameEngine.Models.GamePosition", b =>
                {
                    b.Property<int>("GamePositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GameId")
                        .HasColumnType("int");

                    b.Property<double>("Position")
                        .HasColumnType("float");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GamePositionId");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("GamePositions");
                });

            modelBuilder.Entity("GameEngine.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GamesLost")
                        .HasColumnType("int");

                    b.Property<int?>("GamesWon")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GameEngine.DbModels.GameMember", b =>
                {
                    b.HasOne("GameEngine.Models.Game", "Game")
                        .WithMany("GameMembers")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameEngine.Models.User", "User")
                        .WithMany("GameMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameEngine.Models.Game", b =>
                {
                    b.HasOne("GameEngine.Models.User", "NextToRollDice")
                        .WithMany()
                        .HasForeignKey("NextToRollDiceUserId");

                    b.Navigation("NextToRollDice");
                });

            modelBuilder.Entity("GameEngine.Models.GamePosition", b =>
                {
                    b.HasOne("GameEngine.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("GameEngine.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameEngine.Models.Game", b =>
                {
                    b.Navigation("GameMembers");
                });

            modelBuilder.Entity("GameEngine.Models.User", b =>
                {
                    b.Navigation("GameMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
