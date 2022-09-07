using Microsoft.Extensions.Logging;
using User.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Enums;
using User.Application.Models;

namespace User.Infrastructure.Persistence
{
    public class UserContextSeed
    {
        public static async Task SeedAsync(UserContext userContext, ILogger<UserContextSeed> logger)
        {
            if (!userContext.EmailTemplates.Any())
            {
                userContext.EmailTemplates.AddRange(GetPreconfiguredEmailTemplate());

                await userContext.SaveChangesAsync();
            }
            if (!userContext.UserTypes.Any())
            {
                userContext.UserTypes.AddRange(GetPreconfiguredUserType());

                await userContext.SaveChangesAsync();
            }
            if (!userContext.UserData.Any())
            {

                userContext.UserData.AddRange(GetPreconfiguredUsers());
                await userContext.SaveChangesAsync();

                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(UserContext).Name);
            }
        }

        private static IEnumerable<UserData> GetPreconfiguredUsers() => new List<UserData>
        {
                new UserData() {
                    DisplayName = "Byamasu Patrick",
                    Email = "ptrckbyamasu@gmail.com",
                    EmailConfirmed = false,
                    IsActive = true,
                    RefreshToken = "None",
                    Salt = "76b786254180h8c0981jhuiy971niwojdiq808w9-738067968",
                    Password = "6b786254180h8c0981jhuiy971niwojdiq808w9-73806796",
                    RefreshTokenExpiryTime = DateTime.UtcNow,
                    Profile = new UserProfile()
                    {
                        FirstName = "Byamasu",
                        LastName = "Patrick"
                    },
                    UserTypeId = 3
                }
        };
        private static IEnumerable<UserType> GetPreconfiguredUserType() => new List<UserType>
            {
                new UserType() {
                    UserTypeName = AuthConstants.ADMIN
                },
                new UserType() {
                    UserTypeName = AuthConstants.SHOP_OWNER
                },
                new UserType() {
                    UserTypeName = AuthConstants.PREMIUM_USER
                },
                new UserType() {
                    UserTypeName = AuthConstants.FREE_USER
                }
            };

        private static IEnumerable<EmailTemplate> GetPreconfiguredEmailTemplate() => new List<EmailTemplate>
            {
                new EmailTemplate() {
                    IsActive = true,
                    TemplateType = new TemplateType() {
                        Type = EmailType.Activation.ToString(),
                        CreatedAt = DateTime.UtcNow
                    },
                    TemplateContent = ReadFile(Directory.GetParent(AppContext.BaseDirectory).FullName
                    + Path.DirectorySeparatorChar.ToString()
                    + "Templates" + Path.DirectorySeparatorChar.ToString() +"activation.html")
                },
                new EmailTemplate() {
                    IsActive = true,
                    TemplateType = new TemplateType() {
                        Type = EmailType.ForgotPassword.ToString(),
                        CreatedAt = DateTime.UtcNow
                    },
                    TemplateContent = ReadFile(Directory.GetParent(AppContext.BaseDirectory).FullName
                    + Path.DirectorySeparatorChar.ToString()
                    + "Templates" + Path.DirectorySeparatorChar.ToString() +"forgot-password.html")
                }
            };
        private static string ReadFile(string FilePath)
        {
            try
            {
                //Create an object of FileInfo for specified path            
                FileInfo fi = new FileInfo(FilePath);
                FileStream fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);

                //Create an object of StreamReader by passing FileStream object on which it needs to operates on
                StreamReader sr = new StreamReader(fs);

                //Use the ReadToEnd method to read all the content from file
                string fileContent = sr.ReadToEnd();

                sr.Close();
                fs.Close();

                return fileContent;

            }
            catch (FileNotFoundException error)
            {
                return null;
            }
        }
    }
}
