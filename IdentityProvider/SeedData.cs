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


            //CreateUser("Fredrik", "Jonson", userMgr, ApplicationRoles.Admin).Wait();
            //CreateUser("Gong", "Moonphruk", userMgr, ApplicationRoles.Admin).Wait();
            CreateUser("Robert", "Jodelsohn", userMgr, ApplicationRoles.Admin).Wait();


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

                result = userMgr.AddClaimsAsync(technician, new Claim[]{
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
            var zzache = userMgr.FindByNameAsync("zzache").Result;
            if (zzache == null)
            {
                zzache = new IdentityUser
                {
                    UserName = "zzache",
                    Email = "zzache@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(zzache, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }


                userMgr.AddToRoleAsync(zzache, ApplicationRoles.Technician).Wait();

                result = userMgr.AddClaimsAsync(zzache, new Claim[]{
                    new Claim(JwtClaimTypes.Name, "Zacharias Lönnqvist"),
                    new Claim(JwtClaimTypes.GivenName, "Zacharias"),
                    new Claim(JwtClaimTypes.FamilyName, "Lönnqvist"),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("Zacharias created");
            }
            else
            {
                Log.Debug("Technician already exists");
            }
        }

        private static Task CreateUser(string firstname, string lastname, UserManager<IdentityUser> userManager,string role)
        {
            var user = userManager.FindByNameAsync(firstname).Result;
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = firstname + "123",
                    Email = firstname + "@email.com",
                    EmailConfirmed = true
                };
                var result = userManager.CreateAsync(user, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }


                userManager.AddToRoleAsync(user, role).Wait();

                result = userManager.AddClaimsAsync(user, new Claim[]{
                    new Claim(JwtClaimTypes.Name, firstname + " " + lastname),
                    new Claim(JwtClaimTypes.GivenName, firstname),
                    new Claim(JwtClaimTypes.FamilyName, lastname),
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("Zacharias created");
            }
            else
            {
                Log.Debug("Technician already exists");
            }

            return Task.CompletedTask;
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
