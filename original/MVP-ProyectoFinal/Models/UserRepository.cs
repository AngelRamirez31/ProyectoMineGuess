using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MVP_ProyectoFinal.Models
{
    public static class UserRepository
    {
        static readonly object SyncRoot = new object();
        static List<AppUser> users = new List<AppUser>();
        static bool initialized;

        static string GetFilePath()
        {
            var baseDirectory = AppContext.BaseDirectory;
            var dataDirectory = Path.Combine(baseDirectory, "App_Data");
            Directory.CreateDirectory(dataDirectory);
            return Path.Combine(dataDirectory, "users.json");
        }

        static void EnsureLoaded()
        {
            if (initialized)
            {
                return;
            }

            lock (SyncRoot)
            {
                if (initialized)
                {
                    return;
                }

                var path = GetFilePath();
                if (File.Exists(path))
                {
                    try
                    {
                        var json = File.ReadAllText(path);
                        var existing = JsonSerializer.Deserialize<List<AppUser>>(json);
                        if (existing != null)
                        {
                            users = existing;
                        }
                    }
                    catch
                    {
                        users = new List<AppUser>();
                    }
                }

                if (users.Count == 0)
                {
                    users.Add(new AppUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = "player",
                        Password = "minecraft"
                    });
                    Save();
                }

                initialized = true;
            }
        }

        static void Save()
        {
            var path = GetFilePath();
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(path, json);
        }

        public static AppUser ValidateUser(string userName, string password)
        {
            EnsureLoaded();
            lock (SyncRoot)
            {
                return users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            }
        }

        public static bool UserExists(string userName)
        {
            EnsureLoaded();
            lock (SyncRoot)
            {
                return users.Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static AppUser CreateUser(string userName, string password)
        {
            EnsureLoaded();
            lock (SyncRoot)
            {
                if (users.Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException("User already exists.");
                }

                var user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    Password = password
                };
                users.Add(user);
                Save();
                return user;
            }
        }
    }
}
