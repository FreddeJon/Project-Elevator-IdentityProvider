using IdentityModel;
using IdentityProvider.Data;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>() ?? throw new ArgumentNullException();


            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            CreateRole(roleMgr, ApplicationRoles.Admin).Wait();
            CreateRole(roleMgr, ApplicationRoles.SecondlineTechnician).Wait();
            CreateRole(roleMgr, ApplicationRoles.Technician).Wait();



            var admin = userMgr.FindByNameAsync("admin").Result;
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@email.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(admin, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                userMgr.AddToRoleAsync(admin, ApplicationRoles.Admin).Wait();

                result = userMgr.AddClaimsAsync(admin, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Admin"),
                    new Claim(JwtClaimTypes.GivenName, "Admin"),
                    new Claim(JwtClaimTypes.FamilyName, "Adminsson"),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("admin created");
            }
            else
            {
                Log.Debug("admin already exists");
            }

            var secondlineTech = userMgr.FindByNameAsync("SecondlineTech").Result;
            if (secondlineTech == null)
            {
                secondlineTech = new IdentityUser
                {
                    UserName = "secondlineTech",
                    Email = "secondlineTech@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(secondlineTech, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }


                userMgr.AddToRoleAsync(secondlineTech, ApplicationRoles.SecondlineTechnician).Wait();

                result = userMgr.AddClaimsAsync(secondlineTech, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "SecondlineTech SecondlineTechsson"),
                    new Claim(JwtClaimTypes.GivenName, "SecondlineTech"),
                    new Claim(JwtClaimTypes.FamilyName, "SecondlineTechsson"),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("SecondlineTech created");
            }
            else
            {
                Log.Debug("SecondlineTech already exists");
            }

            var technician = userMgr.FindByNameAsync("technician").Result;
            if (technician == null)
            {
                technician = new IdentityUser
                {
                    UserName = "technician",
                    Email = "technician@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(technician, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }


                userMgr.AddToRoleAsync(technician, ApplicationRoles.Technician).Wait();

                result = userMgr.AddClaimsAsync(secondlineTech, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Technician Techniciansson"),
                    new Claim(JwtClaimTypes.GivenName, "Technician"),
                    new Claim(JwtClaimTypes.FamilyName, "Techniciansson"),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("Technician created");
            }
            else
            {
                Log.Debug("Technician already exists");
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleMgr, string roleName)
        {
            if (await roleMgr.FindByNameAsync(roleName) is null)
            {
                await roleMgr.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
