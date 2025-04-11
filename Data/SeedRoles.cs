using Microsoft.AspNetCore.Identity;

namespace B_LEI.Data
{
    public static class SeedRoles
    {
        public static void Seed(RoleManager<IdentityRole> roleManager)
        {
            roleManager.CreateAsync(new IdentityRole("admin")).Wait();
            roleManager.CreateAsync(new IdentityRole("leitor")).Wait();
            roleManager.CreateAsync(new IdentityRole("bibliotecario")).Wait();
        }
    }
}
