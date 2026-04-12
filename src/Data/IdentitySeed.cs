using Microsoft.AspNetCore.Identity;

namespace Raiz.Data;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = { "Gerente", "Operador" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        await CriarOuResetarUsuario(userManager, "gerente@merceariaraiz.com.br", "Raiz123!", "Gerente");

        await CriarOuResetarUsuario(userManager, "operador@merceariaraiz.com.br", "Raiz123!", "Operador");
    }

    private static async Task CriarOuResetarUsuario(UserManager<IdentityUser> userManager, string email, string senha, string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user != null)
        {
            await userManager.DeleteAsync(user);
        }

        var novoUser = new IdentityUser 
        { 
            UserName = email, 
            Email = email, 
            EmailConfirmed = true 
        };

        var result = await userManager.CreateAsync(novoUser, senha);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(novoUser, role);
        }
    }
}