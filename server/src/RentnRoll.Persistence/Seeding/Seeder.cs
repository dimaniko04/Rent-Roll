using System.Text.Json;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using RentnRoll.Application.Contracts.Categories;
using RentnRoll.Application.Contracts.Genres;
using RentnRoll.Application.Contracts.Mechanics;
using RentnRoll.Domain.Constants;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.BusinessGames;
using RentnRoll.Domain.Entities.Categories;
using RentnRoll.Domain.Entities.Games;
using RentnRoll.Domain.Entities.Genres;
using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.Entities.Mechanics;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;
using RentnRoll.Domain.Entities.Stores;
using RentnRoll.Domain.ValueObjects;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Identity;
using RentnRoll.Persistence.Settings;

namespace RentnRoll.Persistence.Seeding;

public class Seeder
{
    private readonly RentnRollDbContext _context;
    private readonly AdminSettings _adminSettings;
    private readonly UserManager<User> _userManager;

    private readonly string _basePath = Path.Combine(AppContext.BaseDirectory, "data");

    public Seeder(
        RentnRollDbContext context,
        UserManager<User> userManager,
        IOptions<AdminSettings> adminSettings)
    {
        _context = context;
        _userManager = userManager;
        _adminSettings = adminSettings.Value;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();

        await SeedGenres();
        await SeedMechanics();
        await SeedCategories();
        await SeedGames();
        await SeedBusinesses();
        await SeedLockers();
    }

    private async Task SeedRolesAsync()
    {
        if (await _context.Roles.AnyAsync())
            return;

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = Roles.User,
                NormalizedName = Roles.User.ToUpper()
            },
            new IdentityRole
            {
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpper(),
            },
            new IdentityRole
            {
                Name = Roles.Business,
                NormalizedName = Roles.Business.ToUpper(),
            }
        };

        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        var user = await _userManager.FindByEmailAsync(_adminSettings.Email);

        if (user != null)
            return;

        var adminUser = new User
        {
            Email = _adminSettings.Email,
            UserName = _adminSettings.Email,
            FullName = _adminSettings.FullName
        };

        var result = await _userManager
            .CreateAsync(adminUser, _adminSettings.Password);

        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }

    private async Task SeedGenres()
    {
        if (await _context.Set<Genre>().AnyAsync())
            return;

        var genresPath = Path.Combine(_basePath, "genres.json");
        var genresJson = await File.ReadAllTextAsync(genresPath);

        var genres = JsonSerializer
            .Deserialize<List<CreateGenreRequest>>(genresJson)
            ?.Select(g => new Genre
            {
                Id = Guid.NewGuid(),
                Name = g.Name,
            }).ToList();

        if (genres == null || genres.Count == 0)
        {
            return;
        }

        _context.Set<Genre>().AddRange(genres);
        await _context.SaveChangesAsync();
    }

    private async Task SeedMechanics()
    {
        if (await _context.Set<Mechanic>().AnyAsync())
            return;

        var mechanicsPath = Path.Combine(_basePath, "mechanics.json");
        var mechanicsJson = await File.ReadAllTextAsync(mechanicsPath);

        var mechanics = JsonSerializer
            .Deserialize<List<CreateMechanicRequest>>(mechanicsJson)
            ?.Select(g => new Mechanic
            {
                Id = Guid.NewGuid(),
                Name = g.Name,
            }).ToList();

        if (mechanics == null || mechanics.Count == 0)
        {
            return;
        }

        _context.Set<Mechanic>().AddRange(mechanics);
        await _context.SaveChangesAsync();
    }

    private async Task SeedCategories()
    {
        if (await _context.Set<Category>().AnyAsync())
            return;

        var categoriesPath = Path.Combine(_basePath, "categories.json");
        var categoriesJson = await File.ReadAllTextAsync(categoriesPath);

        var categories = JsonSerializer
            .Deserialize<List<CreateCategoryRequest>>(categoriesJson)
            ?.Select(g => new Category
            {
                Id = Guid.NewGuid(),
                Name = g.Name,
            }).ToList();

        if (categories == null || categories.Count == 0)
        {
            return;
        }

        _context.Set<Category>().AddRange(categories);
        await _context.SaveChangesAsync();
    }

    private record JsonGame(
        string Name,
        string Description,
        DateTime PublishedAt,
        int MinPlayers,
        int MaxPlayers,
        int Age,
        int AveragePlayTime,
        int ComplexityScore,
        List<string> Genres,
        List<string> Mechanics,
        List<string> Categories,
        string ThumbnailUrl,
        List<string> Images
    );

    private async Task SeedGames()
    {
        if (await _context.Set<Game>().AnyAsync())
            return;

        var gamesPath = Path.Combine(_basePath, "games.json");
        var gamesJson = await File.ReadAllTextAsync(gamesPath);

        var deserialized = JsonSerializer
            .Deserialize<List<JsonGame>>(gamesJson);
        var games = deserialized?.Select(g => new Game
        {
            Id = Guid.NewGuid(),
            Name = g.Name,
            Description = g.Description,
            PublishedAt = g.PublishedAt,
            MinPlayers = g.MinPlayers,
            MaxPlayers = g.MaxPlayers,
            Age = g.Age,
            AveragePlayTime = g.AveragePlayTime,
            ComplexityScore = g.ComplexityScore,
            Genres = g.Genres is not null
                    ? _context.Set<Genre>()
                        .Where(genre =>
                            g.Genres.Contains(genre.Name))
                        .ToList()
                    : [],
            Mechanics = g.Mechanics is not null
                    ? _context.Set<Mechanic>()
                        .Where(mechanic =>
                            g.Mechanics.Contains(mechanic.Name))
                        .ToList()
                    : [],
            Categories = g.Categories is not null
                    ? _context.Set<Category>()
                        .Where(category =>
                            g.Categories.Contains(category.Name))
                        .ToList()
                    : [],
            ThumbnailUrl = g.ThumbnailUrl,
            Images = g.Images is not null
                    ? g.Images
                        .Select(url => new Image { Url = url })
                        .ToList()
                    : [],
        }).ToList();

        if (games == null || games.Count == 0)
        {
            return;
        }

        _context.Set<Game>().AddRange(games);
        await _context.SaveChangesAsync();
    }

    private record JsonUser(
        string FullName,
        string Email,
        string Password
    );

    private record JsonBusinessGame(
        int Quantity,
        string GameName
    );

    private record JsonPolicyItem(
        string GameName,
        int Price
    );

    private record JsonPricingPolicy(
        string Name,
        string BusinessName,
        string? Description,
        int PricePercent,
        string TimeUnit,
        int UnitCount,
        List<JsonPolicyItem> Items
    );

    private record JsonStoreItem(
        string GameName,
        int Quantity
    );

    private record JsonStore(
        string Name,
        string Description,
        Address Address,
        JsonPricingPolicy Policy,
        List<JsonStoreItem> Items
    );

    private record JsonBusiness(
        string Name,
        string Description,
        JsonUser Owner,
        List<JsonBusinessGame> Games,
        List<JsonStore> Stores
    );

    private async Task SeedBusinesses()
    {
        if (await _context.Set<Business>().AnyAsync())
            return;

        var businessesPath = Path.Combine(_basePath, "businesses.json");
        var businessesJson = await File.ReadAllTextAsync(businessesPath);

        var deserialized = JsonSerializer
            .Deserialize<List<JsonBusiness>>(businessesJson);

        if (deserialized == null)
        {
            return;
        }

        List<Business> businesses = [];

        foreach (var item in deserialized)
        {
            var owner = new User
            {
                Email = item.Owner.Email,
                UserName = item.Owner.Email,
                FullName = item.Owner.FullName,
            };
            await _userManager.CreateAsync(owner, item.Owner.Password);
            await _userManager.AddToRoleAsync(owner, Roles.Business);

            var business = new Business
            {
                Id = Guid.NewGuid(),
                Name = item.Name,
                Description = item.Description,
                OwnerId = owner.Id
            };
            businesses.Add(business);

            foreach (var game in item.Games)
            {
                var gameEntity = await _context.Set<Game>()
                    .FirstOrDefaultAsync(g => g.Name == game.GameName);

                if (gameEntity == null)
                {
                    continue;
                }

                var businessGame = new BusinessGame
                {
                    Id = Guid.NewGuid(),
                    Game = gameEntity,
                    Business = business,
                    Quantity = game.Quantity,
                };
                business.Games.Add(businessGame);
            }

            foreach (var store in item.Stores)
            {
                var storeEntity = new Store
                {
                    Id = Guid.NewGuid(),
                    Name = store.Name,
                    Address = new Address(
                        store.Address.Street,
                        store.Address.City,
                        store.Address.State,
                        store.Address.Country,
                        store.Address.ZipCode
                    ),
                    Business = business,
                };

                var pricingPolicy = new PricingPolicy
                {
                    Id = Guid.NewGuid(),
                    Business = business,
                    Name = store.Policy.Name,
                    Description = store.Policy.Description,
                    PricePercent = store.Policy.PricePercent,
                    TimeUnit = Enum.Parse<TimeUnit>(store.Policy.TimeUnit),
                    UnitCount = store.Policy.UnitCount,
                };

                foreach (var itemPolicy in store.Policy.Items)
                {
                    var gameEntity = business.Games
                        .FirstOrDefault(bg => bg.Game.Name == itemPolicy.GameName);

                    if (gameEntity == null)
                    {
                        continue;
                    }

                    var policyItem = new PricingPolicyItem
                    {
                        Game = gameEntity,
                        Price = itemPolicy.Price,
                        Policy = pricingPolicy
                    };
                    pricingPolicy.Items.Add(policyItem);
                }
                storeEntity.Policy = pricingPolicy;

                foreach (var itemStore in store.Items)
                {
                    var gameEntity = business.Games
                        .FirstOrDefault(bg => bg.Game.Name == itemStore.GameName);

                    if (gameEntity == null)
                    {
                        continue;
                    }

                    var storeItem = new StoreAsset
                    {
                        BusinessGame = gameEntity,
                        Quantity = itemStore.Quantity,
                        Store = storeEntity
                    };
                    storeEntity.Assets.Add(storeItem);
                }
                business.Stores.Add(storeEntity);
            }
        }

        _context.Set<Business>().AddRange(businesses);
        await _context.SaveChangesAsync();
    }

    private record JsonCell(
        string IotDeviceId,
        string BusinessName,
        string BusinessGameName
    );

    private record JsonLocker(
        string Name,
        Address Address,
        List<JsonCell> Cells,
        List<JsonPricingPolicy> PricingPolicies
    );

    private async Task SeedLockers()
    {
        if (await _context.Set<Locker>().AnyAsync())
            return;

        var lockersPath = Path.Combine(_basePath, "lockers.json");
        var lockersJson = await File.ReadAllTextAsync(lockersPath);

        var deserialized = JsonSerializer
            .Deserialize<List<JsonLocker>>(lockersJson);

        if (deserialized == null)
        {
            return;
        }

        List<Locker> lockers = [];

        foreach (var item in deserialized)
        {
            var locker = new Locker
            {
                Id = Guid.NewGuid(),
                Name = item.Name,
                Address = new Address(
                    item.Address.Street,
                    item.Address.City,
                    item.Address.State,
                    item.Address.Country,
                    item.Address.ZipCode
                ),
            };

            foreach (var cell in item.Cells)
            {
                var businessGame = await _context.Set<BusinessGame>()
                    .Include(bg => bg.Game)
                    .Include(bg => bg.Business)
                    .FirstOrDefaultAsync(bg =>
                        bg.Game.Name == cell.BusinessGameName &&
                        bg.Business.Name == cell.BusinessName);

                if (businessGame == null)
                {
                    continue;
                }

                var lockerCell = new Cell
                {
                    Id = Guid.NewGuid(),
                    IotDeviceId = cell.IotDeviceId,
                    BusinessGame = businessGame,
                    Business = businessGame.Business,
                    Locker = locker
                };
                locker.Cells.Add(lockerCell);
            }

            foreach (var policy in item.PricingPolicies)
            {
                var business = await _context.Set<Business>()
                    .FirstOrDefaultAsync(b => b.Name == policy.BusinessName);

                if (business == null)
                {
                    continue;
                }

                var pricingPolicy = new PricingPolicy
                {
                    Id = Guid.NewGuid(),
                    Business = business,
                    Name = policy.Name,
                    Description = policy.Description,
                    PricePercent = policy.PricePercent,
                    TimeUnit = Enum.Parse<TimeUnit>(policy.TimeUnit),
                    UnitCount = policy.UnitCount,
                };

                foreach (var itemPolicy in policy.Items)
                {
                    var businessGame = await _context.Set<BusinessGame>()
                        .Include(bg => bg.Game)
                        .FirstOrDefaultAsync(bg => bg.Game.Name == itemPolicy.GameName);

                    if (businessGame == null)
                    {
                        continue;
                    }

                    var policyItem = new PricingPolicyItem
                    {
                        Game = businessGame,
                        Price = itemPolicy.Price,
                        Policy = pricingPolicy
                    };
                    pricingPolicy.Items.Add(policyItem);
                }
                locker.PricingPolicies.Add(pricingPolicy);
            }
            lockers.Add(locker);
        }

        _context.Set<Locker>().AddRange(lockers);
        await _context.SaveChangesAsync();
    }
}