﻿// <auto-generated />
using System;
using GamesTournamentsWeb.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GamesTournamentsWeb.DataAccess.Migrations
{
    [DbContext(typeof(WebContext))]
    [Migration("20240419135408_TournamentPlayerExpectedWinner")]
    partial class TournamentPlayerExpectedWinner
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AccountTournament", b =>
                {
                    b.Property<int>("AdminTournamentsId")
                        .HasColumnType("int");

                    b.Property<int>("AdminsId")
                        .HasColumnType("int");

                    b.HasKey("AdminTournamentsId", "AdminsId");

                    b.HasIndex("AdminsId");

                    b.ToTable("AccountTournament", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Dashboard.Layout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Layout", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Dashboard.LayoutItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("LayoutId")
                        .HasColumnType("int");

                    b.Property<int>("ModuleId")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LayoutId");

                    b.HasIndex("ModuleId");

                    b.ToTable("LayoutItem", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Dashboard.Module", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Module", "enum");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "TournamentHistory"
                        });
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Games.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("Game", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Games.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Genre", "dbo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "MOBA"
                        },
                        new
                        {
                            Id = 2,
                            Name = "FPS"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Sport"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Simulator"
                        });
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Locale")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Currency", "dbo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "USD",
                            Locale = "en-US",
                            Name = "Dollar",
                            Symbol = "$"
                        },
                        new
                        {
                            Id = 2,
                            Code = "EUR",
                            Locale = "en-EU",
                            Name = "Euro",
                            Symbol = "€"
                        },
                        new
                        {
                            Id = 3,
                            Code = "GBP",
                            Locale = "en-GB",
                            Name = "Pound",
                            Symbol = "£"
                        },
                        new
                        {
                            Id = 4,
                            Code = "CZK",
                            Locale = "cs-CZ",
                            Name = "Czech crown",
                            Symbol = "Kč"
                        });
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset?>("EndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("FirstTeamId")
                        .HasColumnType("int");

                    b.Property<int?>("NextMatchId")
                        .HasColumnType("int");

                    b.Property<int?>("SecondTeamId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FirstTeamId")
                        .HasFilter("([FirstTeamId] IS NOT NULL)");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("FirstTeamId"), false);

                    b.HasIndex("NextMatchId")
                        .HasFilter("([NextMatchId] IS NOT NULL)");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("NextMatchId"), false);

                    b.HasIndex("SecondTeamId")
                        .HasFilter("([SecondTeamId] IS NOT NULL)");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("SecondTeamId"), false);

                    b.HasIndex("TournamentId");

                    b.HasIndex("WinnerId")
                        .HasFilter("([WinnerId] IS NOT NULL)");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("WinnerId"), false);

                    b.ToTable("Match", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Platform", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Platform", "enum");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "pc",
                            Name = "Pc"
                        },
                        new
                        {
                            Id = 2,
                            Code = "playstation",
                            Name = "Playstation"
                        },
                        new
                        {
                            Id = 3,
                            Code = "xbox",
                            Name = "Xbox"
                        });
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Prize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("Place")
                        .HasColumnType("int");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("TournamentId");

                    b.ToTable("Prize", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Region", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Region", "enum");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "europe",
                            Name = "Europe"
                        },
                        new
                        {
                            Id = 2,
                            Code = "north_america",
                            Name = "NorthAmerica"
                        },
                        new
                        {
                            Id = 3,
                            Code = "south_america",
                            Name = "SouthAmerica"
                        },
                        new
                        {
                            Id = 4,
                            Code = "asia",
                            Name = "Asia"
                        });
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Stream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Stream", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Team", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AnyoneCanJoin")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("EndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasMaxLength(-1)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaximumPlayers")
                        .HasColumnType("int");

                    b.Property<int>("MinimumPlayers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int");

                    b.Property<string>("Rules")
                        .IsRequired()
                        .HasMaxLength(-1)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("TeamSize")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlatformId");

                    b.ToTable("Tournament", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.TournamentComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(-1)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentComment", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.TournamentPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int?>("ExpectedWinnerId")
                        .HasColumnType("int");

                    b.Property<string>("GameUsername")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TeamId");

                    b.HasIndex("TournamentId");

                    b.ToTable("TournamentPlayer", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.TournamentPlayerStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("TournamentPlayerStatus", "enum");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Pending"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Accepted"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Rejected"
                        });
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Users.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("binary(64)")
                        .IsFixedLength();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("binary(128)")
                        .IsFixedLength();

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Account", "dbo");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Users.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Role", "enum");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "User"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("RegionTournament", b =>
                {
                    b.Property<int>("RegionsId")
                        .HasColumnType("int");

                    b.Property<int>("TournamentsId")
                        .HasColumnType("int");

                    b.HasKey("RegionsId", "TournamentsId");

                    b.HasIndex("TournamentsId");

                    b.ToTable("RegionTournament");
                });

            modelBuilder.Entity("AccountTournament", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", null)
                        .WithMany()
                        .HasForeignKey("AdminTournamentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Users.Account", null)
                        .WithMany()
                        .HasForeignKey("AdminsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Dashboard.Layout", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Users.Account", "Account")
                        .WithMany("Layouts")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Dashboard.LayoutItem", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Dashboard.Layout", "Layout")
                        .WithMany("Items")
                        .HasForeignKey("LayoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Dashboard.Module", "Module")
                        .WithMany("LayoutItems")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Layout");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Games.Game", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Games.Genre", "Genre")
                        .WithMany("Games")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Match", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Prize", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Currency", "Currency")
                        .WithMany("Prizes")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", "Tournament")
                        .WithMany("Prizes")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Stream", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", "Tournament")
                        .WithMany("Streams")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Games.Game", "Game")
                        .WithMany("Tournaments")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Platform", "Platform")
                        .WithMany("Tournaments")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.TournamentComment", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Users.Account", "Account")
                        .WithMany("TournamentComments")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", "Tournament")
                        .WithMany("Comments")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.TournamentPlayer", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Users.Account", "Account")
                        .WithMany("TournamentPlayers")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.TournamentPlayerStatus", "Status")
                        .WithMany("TournamentPlayers")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Team", null)
                        .WithMany("Players")
                        .HasForeignKey("TeamId");

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", "Tournament")
                        .WithMany("Players")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Status");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Users.Account", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Users.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("RegionTournament", b =>
                {
                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Region", null)
                        .WithMany()
                        .HasForeignKey("RegionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", null)
                        .WithMany()
                        .HasForeignKey("TournamentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Dashboard.Layout", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Dashboard.Module", b =>
                {
                    b.Navigation("LayoutItems");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Games.Game", b =>
                {
                    b.Navigation("Tournaments");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Games.Genre", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Currency", b =>
                {
                    b.Navigation("Prizes");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Platform", b =>
                {
                    b.Navigation("Tournaments");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Team", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.Tournament", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Matches");

                    b.Navigation("Players");

                    b.Navigation("Prizes");

                    b.Navigation("Streams");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Tournaments.TournamentPlayerStatus", b =>
                {
                    b.Navigation("TournamentPlayers");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Users.Account", b =>
                {
                    b.Navigation("Layouts");

                    b.Navigation("TournamentComments");

                    b.Navigation("TournamentPlayers");
                });

            modelBuilder.Entity("GamesTournamentsWeb.DataAccess.Models.Users.Role", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
