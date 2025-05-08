using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using RentnRoll.Domain.Constants;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Identity;
using RentnRoll.Persistence.Settings;

namespace RentnRoll.Persistence.Seeding;

public class Seeder
{
    private readonly RentnRollDbContext _context;
    private readonly AdminSettings _adminSettings;
    private readonly UserManager<User> _userManager;

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
            PhoneNumber = _adminSettings.PhoneNumber,
            FirstName = _adminSettings.FirstName,
            LastName = _adminSettings.LastName,
        };

        var result = await _userManager
            .CreateAsync(adminUser, _adminSettings.Password);

        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }
}