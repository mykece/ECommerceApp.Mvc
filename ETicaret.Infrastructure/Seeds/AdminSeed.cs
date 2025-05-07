using ETicaret.Domain.Entities;
using ETicaret.Domain.Enums;
using ETicaret.Infrastructure.AppContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ETicaret.UI.Seeds;

public class AdminSeed
{
	private const string adminEmail = "admin@bilgeadam.com";
	private const string adminPassword = "Password.1";
	// <summary>
	/// veritabanını başlatır ve gerekli roller ve admin kullanıcısını ekler.
	/// </summary>
	/// <param name="configuration"> yapılandırma bilgilerine erişir (örneğin, veritabanı bağlantı dizesi gibi).</param>
	/// <returns></returns>
	public static async Task SeedAsync(IConfiguration configuration)
	{
		var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
		dbContextBuilder.UseSqlServer(configuration.GetConnectionString("AppConnectionDev"));
		AppDbContext context = new AppDbContext(dbContextBuilder.Options);
		if (!context.Roles.Any(role => role.Name == "Admin"))
		{
			await AddRoles(context);
		}
		if (!context.Users.Any(user => user.Email == adminEmail))
		{
			await AddAdmin(context);
		}
	}

	/// <summary>
	/// admin kullanıcısını ve ona ait rolü ekler.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	private static async Task AddAdmin(AppDbContext context)
	{
		//IdentityUser sınıfı kullanılarak bir kullanıcı oluşturulur ve şifrelenmiş parolası belirlenir
		IdentityUser user = new IdentityUser()
		{
			Email = adminEmail,
			NormalizedEmail = adminEmail.ToUpperInvariant(),
			UserName = adminEmail,
			NormalizedUserName = adminEmail.ToUpperInvariant(),
			EmailConfirmed = true
		};
		user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, adminPassword);
		await context.Users.AddAsync(user);

		var adminRoleId = context.Roles.FirstOrDefault(role => role.Name == Roles.Admin.ToString())!.Id;
		await context.UserRoles.AddAsync(new IdentityUserRole<string>()
		{
			RoleId = adminRoleId,
			UserId = user.Id,
		});
		//Kullanıcıya "Admin" rolü atanır ve Admin varlığı (entity) eklenir.
		await context.Admins.AddAsync(new Admin()
		{
			FirstName = "Admin",
			LastName = "Admin",
			Email = adminEmail,
			IdentityId = user.Id
		});
		await context.SaveChangesAsync();
	}
	/// <summary>
	/// Roles enumundaki rolleri veritabanına ekler.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	private static async Task AddRoles(AppDbContext context)
	{
		string[] roles = Enum.GetNames(typeof(Roles));  // Enumda bulunan verilerin isimlerini dizi olarak döner
		for (int i = 0; i < roles.Length; i++)
		{
			//Enum'dan rolleri alır ve eğer veritabanında o rol yoksa ekler.
			if (await context.Roles.AnyAsync(role => role.Name == roles[i]))
			{
				continue;
			}
			IdentityRole role = new IdentityRole()
			{
				Name = roles[i],
				NormalizedName = roles[i].ToUpperInvariant()
			};
			await context.Roles.AddAsync(role);
			await context.SaveChangesAsync();
		}
	}
}
