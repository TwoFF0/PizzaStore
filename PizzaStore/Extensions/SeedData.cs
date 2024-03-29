﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using PizzaStore.Data;
using PizzaStore.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;

namespace PizzaStore.Extensions
{
    /// <summary>
    /// Generates random data.
    /// </summary>
    public static class SeedData
    {
        public async static Task GenerateSeedData(DataContext context, UserManager<User> userManager, RoleManager<AppRole> roleManager, int itemCount)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (itemCount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{itemCount} cannot be less or equal zero");
            }

            string[] photos =
            {
                "https://dodopizza-a.akamaihd.net/static/Img/Products/ace34b88215e4ee790cf090cba9d9e6a_292x292.png",
                "https://dodopizza-a.akamaihd.net/static/Img/Products/4fa4de77d8a34912830cfdbedfaff698_292x292.png",
                "https://dodopizza-a.akamaihd.net/static/Img/Products/b68c363a26644118806d07435d9b2806_292x292.png"
            };

            string[] otherPhotos =
            {
                "https://cdn.dodostatic.net/static/Img/Products/d224a75b54cd48008621053fb23f725c_292x292.jpeg"
            };

            string[] sizes =
            {
                "S",
                "M",
                "L"
            };

            var dict = new Dictionary<int, double>()
            {
                {0, 1},
                {1, 1.35},
                {2, 1.85}
            };

            var categories = new List<string>()
            {
                "Pizza",
                "Beverages",
                "Desserts",
                "Other"
            };

            var random = new Random();

            if (!context.Products.Any())
            {

                for (int i = 0; i < itemCount; i++)
                {
                    var listOfSizes = new List<ProductSize>();

                    var templateProduct = new Faker<Product>("en")
                       .RuleFor(x => x.Category, x => categories[x.Random.Int(0, 2)])
                       .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
                       .RuleFor(x => x.Name, x => x.Commerce.ProductName());

                    var smallWeight = random.Next(450, 550);
                    var smallPrice = random.Next(5, 12);

                    for (int j = 0; j < 3; j++)
                    {
                        var productSize = new Faker<ProductSize>("en")
                            .RuleFor(x => x.ImageUrl, x => photos[j])
                            .RuleFor(x => x.Size, x => sizes[j])
                            .RuleFor(x => x.Price, x => Math.Round(smallPrice * dict[j]) - 0.01)
                            .RuleFor(x => x.Weight, x => Convert.ToInt32(smallWeight * dict[j]).RoundOff()).Generate();

                        listOfSizes.Add(productSize);
                    }

                    var temp = templateProduct.Generate();
                    temp.ProductSizes = listOfSizes;

                    await context.Products.AddAsync(temp);
                    await context.SaveChangesAsync();
                }

                for (int i = 0; i < 10; i++)
                {
                    var listOfSizes = new List<ProductSize>();

                    var product = new Faker<Product>("en")
                        .RuleFor(x => x.Category, x => categories[3])
                        .RuleFor(x => x.Description, x => x.Commerce.ProductDescription())
                        .RuleFor(x => x.Name, x => x.Commerce.ProductName());

                    var Weight = random.Next(100, 200);
                    var Price = random.Next(5, 12);

                    var productSize = new Faker<ProductSize>("en")
                        .RuleFor(x => x.ImageUrl, x => otherPhotos[0])
                        .RuleFor(x => x.Price, x => Math.Round(Price * 1.3) - 0.01)
                        .RuleFor(x => x.Weight, x => Convert.ToInt32(Weight).RoundOff()).Generate();

                    listOfSizes.Add(productSize);

                    product.RuleFor(x => x.ProductSizes, x => listOfSizes);

                    await context.Products.AddAsync(product);
                    await context.SaveChangesAsync();
                }

            }

            if (!userManager.Users.Any())
            {

               var roles = new List<AppRole>(){
                    new AppRole{Name = "Admin"},
                    new AppRole{Name = "Member"},
               };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }

                for (int i = 0; i < itemCount; i++)
                {
                    var user = new Faker<User>("en")
                                    .RuleFor(x => x.Age, f => f.Random.Number(14, 80))
                                    .RuleFor(x => x.UserName, f => f.Person.UserName.ToLower())
                                    .RuleFor(x => x.City, f => f.Address.City())
                                    .RuleFor(x => x.Country, f => f.Address.Country())
                                    .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                                    .RuleFor(x => x.LastName, f => f.Name.LastName())
                                    .RuleFor(x => x.CreatedAt, f => f.Date.Past(5))
                                    .RuleFor(x => x.LastActive, f => f.Date.Past(5))
                                    .RuleFor(x => x.Balance, f => Math.Round(f.Random.Double(5, 150), 2))
                                    .Generate();

                    await userManager.CreateAsync(user, "password");
                    await userManager.AddToRoleAsync(user, "Member");
                }

                var admin = new User
                {
                    UserName = "admin",
                    Balance = double.MaxValue
                };

                await userManager.CreateAsync(admin, "strongPassword");
                await userManager.AddToRoleAsync(admin, "Admin");

            }
        }
    }
}