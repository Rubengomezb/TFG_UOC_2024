using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.DB.Context
{
    /// <summary>
    /// Class to seed the database with initial data.
    /// </summary>
    public class SeedDb
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userId = Guid.NewGuid();
            const string adminEmail = "admin@website.com";
            const string adminPassword = "passW0rd!";
            const string adminUsername = "admin";
            const string adminFirst = "Admin";
            const string adminLast = "Contact";

            var context = serviceProvider.GetRequiredService<ApplicationContext>();
            context.Database.EnsureCreated();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            // initially create user(s)
            if (!context.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = adminEmail,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = adminUsername,
                    CreatedOn = DateTime.UtcNow,
                    PhoneNumber = "666666666",
                    Contact = new Contact() { Email = adminEmail, FirstName = adminFirst, LastName = adminLast, CreatedOn = DateTime.UtcNow, PhoneNumber = "666666666", Title = "Prueba", CreatedBy = userId, WebsiteUrl = "."}
                };
                var result = userManager.CreateAsync(user, adminPassword).Result;
            }

            // get the admin user we just made
            var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
            if (adminUser == null)
                return;

            // make sure we have some roles
            if (!context.Roles.Any())
            {
                ApplicationRole role = new ApplicationRole() { Name = "Administrator" };

                var result = roleManager.CreateAsync(role).Result;

                // assign admin role to admin
                var ur = userManager.AddToRoleAsync(adminUser, "Administrator").Result;
            }

            if (!context.Ingredient.Any())
            {
                var categories = new List<Category>
                {
                    new Category() { Name = "Vegetables", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/705/original/Fotolia_65015591_XS.jpg?242382", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Fruits", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/702/original/Fotolia_40093177_XS.jpg?805521", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Meat", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/180/original/Fotolia_63808842_XS.jpg?434151", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Bread", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/198/original/Fotolia_60326169_XS.jpg?147420", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Sweets", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/252/original/Fotolia_77703943_XS.jpg?277703", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Pasta", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/206/original/Fotolia_51284648_XS.jpg?979864", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Milky", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/278/original/Fotolia_62567939_XS.jpg?965972", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Eggs", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/722/original/Fotolia_76475468_XS.jpg?019563", CreatedBy = userId, CreatedOn = DateTime.UtcNow },
                    new Category() { Name = "Fish", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/156/original/Fotolia_65747871_XS.jpg?808299", CreatedBy = userId, CreatedOn = DateTime.UtcNow }
                };

                var ingredients = new List<Ingredient>
                {
                    new Ingredient() { Name = "Chicken", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/181/original/Fotolia_64510042_XS.jpg?043973", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[3] },
                    new Ingredient() { Name = "Beef", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/180/original/Fotolia_63808842_XS.jpg?434151", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[3] },
                    new Ingredient() { Name = "Pork", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/190/original/Fotolia_48282102_XS.jpg?085005", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[3] },
                    new Ingredient() { Name = "Lettuce", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/695/original/Fotolia_53483671_XS.jpg?896762", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Mushroom", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/699/original/Fotolia_59358152_XS.jpg?486426", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Pepper", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/705/original/Fotolia_65015591_XS.jpg?242382", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Courgette", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/681/original/Fotolia_60420094_XS.jpg?037005", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Onion", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/700/original/Fotolia_58333191_XS.jpg?071231", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Potato", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/708/original/Fotolia_36790251_XS.jpg?890535", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Carrot", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/673/original/Fotolia_62467540_XS.jpg?292378", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Garlic", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/690/original/Fotolia_59757393_XS.jpg?024210", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[0] },
                    new Ingredient() { Name = "Strawberry", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/713/original/Fotolia_79112171_XS.jpg?985058", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[1] },
                    new Ingredient() { Name = "Tangerine", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/715/original/Fotolia_75910504_XS.jpg?435627", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[1] },
                    new Ingredient() { Name = "Banana", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/671/original/Fotolia_71934090_XS.jpg?679155", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[1] },
                    new Ingredient() { Name = "Peach", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/702/original/Fotolia_40093177_XS.jpg?805521", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[1] },
                    new Ingredient() { Name = "Eggs", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/722/original/Fotolia_76475468_XS.jpg?019563", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[7] },
                    new Ingredient() { Name = "Cheese", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/145/original/Fotolia_20485702_XS.jpg?087703", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[6] },
                    new Ingredient() { Name = "Milk", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/278/original/Fotolia_62567939_XS.jpg?965972", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[6] },
                    new Ingredient() { Name = "Pasta", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/206/original/Fotolia_51284648_XS.jpg?979864", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[5] },
                    new Ingredient() { Name = "Rice", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/207/original/Fotolia_28347338_XS.jpg?312350", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[5] },
                    new Ingredient() { Name = "Noodles", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/204/original/Fotolia_58692586_XS.jpg?445971", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[5] },
                    new Ingredient() { Name = "Bread", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/198/original/Fotolia_60326169_XS.jpg?147420", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[3] },
                    new Ingredient() { Name = "Gilthead", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/155/original/Fotolia_69666433_XS.jpg?101919", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[8] },
                    new Ingredient() { Name = "Cod", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/156/original/Fotolia_65747871_XS.jpg?808299", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[8] },
                    new Ingredient() { Name = "King prawns", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/160/original/Fotolia_66655874_XS.jpg?394307", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[8] },
                    new Ingredient() { Name = "Mussels", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/164/original/Fotolia_60054339_XS.jpg?690549", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[8] },
                    new Ingredient() { Name = "Octopus", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/165/original/Fotolia_70663193_XS.jpg?123617", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[8] },
                    new Ingredient() { Name = "Salmon", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/169/original/Fotolia_55748744_XS.jpg?667675", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[8] },
                    new Ingredient() { Name = "Tuna", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/178/original/Fotolia_73217093_XS.jpg?586517", CreatedBy = userId, CreatedOn = DateTime.UtcNow, CategoryNavigation = categories[8] }
                };

                context.Ingredient.AddRange(ingredients);
                context.SaveChanges();
            }
        }
    }
}
