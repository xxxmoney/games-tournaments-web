﻿using System.ComponentModel;
using GamesTournamentsWeb.Common.Enums.Account;
using GamesTournamentsWeb.Common.Enums.Dashboard;
using GamesTournamentsWeb.Common.Enums.Tournament;
using GamesTournamentsWeb.Common.Helpers;
using GamesTournamentsWeb.DataAccess.Models.Dashboard;
using GamesTournamentsWeb.DataAccess.Models.Games;
using GamesTournamentsWeb.DataAccess.Models.Tournaments;
using GamesTournamentsWeb.DataAccess.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stream = GamesTournamentsWeb.DataAccess.Models.Tournaments.Stream;

namespace GamesTournamentsWeb.DataAccess.Contexts;

public class WebContext(IConfiguration configuration) : DbContext
{
    private readonly string connectionString = configuration.GetConnectionString(Constants.WebConnectionKey);

    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<TournamentComment> TournamentComments { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Prize> Prizes { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Stream> Streams { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TournamentPlayer> TournamentPlayers { get; set; }
    public DbSet<TournamentPlayerStatus> TournamentPlayerStatuses { get; set; }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Module> Modules { get; set; }
    public DbSet<Layout> Layouts { get; set; }
    public DbSet<LayoutItem> LayoutItems { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(game =>
        {
            game.ToTable(nameof(Game), Constants.DboSchema);
            
            game.HasOne(e => e.Genre)
                .WithMany(e => e.Games)
                .HasForeignKey(e => e.GenreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            game.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            game.Property(e => e.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(1000);

        });
        
        modelBuilder.Entity<Genre>(genre =>
        {
            genre.ToTable(nameof(Genre), Constants.DboSchema);
            
            genre.HasData(
                new Genre
                {
                    Id = 1,
                    Name = "MOBA"
                },
                new Genre
                {
                    Id = 2,
                    Name = "FPS"
                },
                new Genre
                {
                    Id = 3,
                    Name = "Sport"
                },
                new Genre
                {
                    Id = 4,
                    Name = "Simulator"
                }
            );
            
            genre.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Tournament>(tournament =>
        {
            tournament.ToTable(nameof(Tournament), Constants.DboSchema);
            
            tournament.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            tournament.HasOne(e => e.Game)
                .WithMany(e => e.Tournaments)
                .HasForeignKey(e => e.GameId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            tournament.HasOne(e => e.Platform)
                .WithMany(e => e.Tournaments)
                .HasForeignKey(e => e.PlatformId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            tournament.HasMany(e => e.Regions)
                .WithMany(e => e.Tournaments);

            tournament.Property(e => e.StartDate)
                .IsRequired();

            tournament.Property(e => e.EndDate)
                .IsRequired(false);
            
            tournament.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(-1);
            
            tournament.Property(e => e.Rules)
                .IsRequired()
                .HasMaxLength(-1);

            tournament.HasMany(e => e.Prizes)
                .WithOne(e => e.Tournament)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            tournament.HasMany(e => e.Matches)
                .WithOne(e => e.Tournament)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            tournament.HasMany(e => e.Streams)
                .WithOne(e => e.Tournament)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            tournament.HasMany(e => e.Players)
                .WithOne(e => e.Tournament)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            tournament.HasMany(e => e.Comments)
                .WithOne(e => e.Tournament)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            tournament.HasMany(e => e.Admins)
                .WithMany(e => e.AdminTournaments);
        });

        modelBuilder.Entity<TournamentComment>(tournamentComment =>
        {
            tournamentComment.ToTable(nameof(TournamentComment), Constants.DboSchema);
            
            tournamentComment.Property(e => e.Text)
                .IsRequired()
                .HasMaxLength(-1);

            tournamentComment.Property(e => e.CreateDate)
                .IsRequired();

            tournamentComment.HasOne(e => e.Account)
                .WithMany(e => e.TournamentComments)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        });
        
        modelBuilder.Entity<TournamentPlayer>(tournamentPlayer =>
        {
            tournamentPlayer.ToTable(nameof(TournamentPlayer), Constants.DboSchema);
            
            tournamentPlayer.HasOne(e => e.Tournament)
                .WithMany(e => e.Players)
                .HasForeignKey(e => e.TournamentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            tournamentPlayer.HasOne(e => e.Account)
                .WithMany(e => e.TournamentPlayers)
                .HasForeignKey(e => e.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            tournamentPlayer.Property(e => e.GameUsername)
                .IsRequired(false)
                .HasMaxLength(100);

            tournamentPlayer.HasOne(e => e.Status)
                .WithMany(e => e.TournamentPlayers)
                .HasForeignKey(e => e.StatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<TournamentPlayerStatus>(tournamentPlayerStatus =>
        {
            tournamentPlayerStatus.ToTable(nameof(TournamentPlayerStatus), Constants.EnumSchema);
            
            tournamentPlayerStatus.HasData(EnumHelper.SelectTo<TournamentPlayerStatusEnum, TournamentPlayerStatus>(info => new TournamentPlayerStatus
            {
                Id = info.Value,
                Name = info.Name
            }));
            
            tournamentPlayerStatus.Property(e => e.Id).ValueGeneratedNever();
            
            tournamentPlayerStatus.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<Prize>(prize =>
        {
            prize.ToTable(nameof(Prize), Constants.DboSchema);
            
            prize.Property(e => e.Place)
                .IsRequired();

            prize.Property(e => e.Amount)
                .IsRequired();

            prize.HasOne(e => e.Currency)
                .WithMany(e => e.Prizes)
                .HasForeignKey(e => e.CurrencyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            prize.HasOne(e => e.Tournament)
                .WithMany(e => e.Prizes)
                .HasForeignKey(e => e.TournamentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Stream>(stream =>
        {
            stream.ToTable(nameof(Stream), Constants.DboSchema);
            
            stream.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            stream.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(1000);

            stream.HasOne(e => e.Tournament)
                .WithMany(e => e.Streams)
                .HasForeignKey(e => e.TournamentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Team>(team =>
        {
            team.ToTable(nameof(Team), Constants.DboSchema);
            
            team.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<Match>(match =>
        {
            match.ToTable(nameof(Match), Constants.DboSchema);
            
            match.Property(e => e.StartDate)
                .IsRequired(false);

            match.Property(e => e.EndDate)
                .IsRequired(false);

            match.HasOne(e => e.Tournament)
                .WithMany(e => e.Matches)
                .HasForeignKey(e => e.TournamentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            match.HasIndex(e => e.NextMatchId)
                .IsUnique(false)
                .IsClustered(false)
                .HasFilter($"([{nameof(Match.NextMatchId)}] IS NOT NULL)");
            
            match.HasIndex(e => e.FirstTeamId)
                .IsUnique(false)
                .IsClustered(false)
                .HasFilter($"([{nameof(Match.FirstTeamId)}] IS NOT NULL)");
            
            match.HasIndex(e => e.SecondTeamId)
                .IsUnique(false)
                .IsClustered(false)
                .HasFilter($"([{nameof(Match.SecondTeamId)}] IS NOT NULL)");
            
            match.HasIndex(e => e.WinnerId)
                .IsUnique(false)
                .IsClustered(false)
                .HasFilter($"([{nameof(Match.WinnerId)}] IS NOT NULL)");
        });
        
        modelBuilder.Entity<Platform>(platform =>
        {
            platform.ToTable(nameof(Platform), Constants.EnumSchema);

            platform.HasData(EnumHelper.SelectTo<PlatformEnum, Platform>(info => new Platform
            {
                Id = info.Value,
                Name = info.Name,
                Code = ((DescriptionAttribute)info.Attribute).Description
            }, typeof(DescriptionAttribute)));
            
            platform.Property(e => e.Id).ValueGeneratedNever();
            
            platform.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            platform.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<Region>(region =>
        {
            region.ToTable(nameof(Region), Constants.EnumSchema);
            
            region.HasData(EnumHelper.SelectTo<RegionEnum, Region>(info => new Region
            {
                Id = info.Value,
                Name = info.Name,
                Code = ((DescriptionAttribute)info.Attribute).Description
            }, typeof(DescriptionAttribute)));
            
            region.Property(e => e.Id).ValueGeneratedNever();
            
            region.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            region.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<Account>(account =>
        {
            account.ToTable(nameof(Account), Constants.DboSchema);
            
            account.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);

            account.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(254);

            account.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(64)
                .IsFixedLength();

            account.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(128)
                .IsFixedLength();

            account.HasOne(e => e.Role)
                .WithMany(e => e.Accounts)
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            account.Property(e => e.CreatedAt)
                .IsRequired();

            account.Property(e => e.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(1000);
        });
        
        modelBuilder.Entity<Role>(role =>
        {
            role.ToTable(nameof(Role), Constants.EnumSchema);

            role.HasData(EnumHelper.SelectTo<RoleEnum, Role>(info => new Role
            {
                Id = info.Value,
                Name = info.Name
            }));
            
            role.Property(e => e.Id).ValueGeneratedNever();
            
            role.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<Currency>(currency =>
        {
            currency.ToTable(nameof(Currency), Constants.DboSchema);
            
            currency.HasData(
                new Currency
            {
                Id = 1,
                Name = "Dollar",
                Code = "USD",
                Symbol = "$",
                Locale = "en-US"
            }, new Currency
            {
                Id = 2,
                Name = "Euro",
                Code = "EUR",
                Symbol = "€",
                Locale = "en-EU"
            }, new Currency
            {
                Id = 3,
                Name = "Pound",
                Code = "GBP",
                Symbol = "£",
                Locale = "en-GB"
            }, new Currency
            {
                Id = 4,
                Name = "Czech crown",
                Code = "CZK",
                Symbol = "Kč",
                Locale = "cs-CZ"
            });
            
            currency.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            currency.Property(e => e.Code)  
                .IsRequired()
                .HasMaxLength(10);
            
            currency.Property(e => e.Locale)
                .IsRequired()
                .HasMaxLength(10);
            
            currency.Property(e => e.Symbol)
                .IsRequired()
                .HasMaxLength(10);
        });
        
        modelBuilder.Entity<Module>(module =>
        {
            module.ToTable(nameof(Module), Constants.EnumSchema);

            module.HasData(EnumHelper.SelectTo<ModuleEnum, Module>(info => new Module
            {
                Id = info.Value,
                Name = info.Name
            }));
            
            module.Property(e => e.Id).ValueGeneratedNever();
            
            module.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<Layout>(layout =>
        {
            layout.ToTable(nameof(Layout), Constants.DboSchema);
            
            layout.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });
        
        modelBuilder.Entity<LayoutItem>(layoutItem =>
        {
            layoutItem.ToTable(nameof(LayoutItem), Constants.DboSchema);
            
            layoutItem.HasOne(e => e.Module)
                .WithMany(e => e.LayoutItems)
                .HasForeignKey(e => e.ModuleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            
            layoutItem.HasOne(e => e.Layout)
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.LayoutId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        base.OnModelCreating(modelBuilder);
    }
}