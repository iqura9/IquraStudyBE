using Microsoft.AspNetCore.Identity;

namespace IquraStudyBE.Models;

public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string teacherEmail = "admin@gmail.com";
        string password = "_Aa123456";
        if (await roleManager.FindByNameAsync("Teacher") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Teacher"));
        }
        if (await roleManager.FindByNameAsync("Student") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Student"));
        }
        if (await userManager.FindByNameAsync(teacherEmail) == null)
        {
            User teacher = new User { Email = teacherEmail, UserName = teacherEmail, CreatedAt = DateTime.UtcNow };
            IdentityResult result = await userManager.CreateAsync(teacher, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(teacher, "Teacher");
            }
        }
    }
}