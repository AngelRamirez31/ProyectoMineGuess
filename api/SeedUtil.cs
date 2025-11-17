using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MineGuess.Api.Data;
using MineGuess.Api.Models;

namespace MineGuess.Api;

public static class SeedUtil
{
    public static async Task SeedAsync(AppDb db, IHostEnvironment env)
    {
        if (await db.GameVersions.AnyAsync())
            return;

        var versions = new[] {
            new GameVersion { Name = "1.0",  Channel = "release", OrderKey = 10000 },
            new GameVersion { Name = "1.3",  Channel = "release", OrderKey = 10300 },
            new GameVersion { Name = "1.4",  Channel = "release", OrderKey = 10400 },
            new GameVersion { Name = "1.5",  Channel = "release", OrderKey = 10500 },
            new GameVersion { Name = "1.8",  Channel = "release", OrderKey = 10800 },
            new GameVersion { Name = "1.11", Channel = "release", OrderKey = 11100 },
            new GameVersion { Name = "1.12", Channel = "release", OrderKey = 11200 },
            new GameVersion { Name = "1.13", Channel = "release", OrderKey = 11300 },
            new GameVersion { Name = "1.14", Channel = "release", OrderKey = 11400 },
            new GameVersion { Name = "1.15", Channel = "release", OrderKey = 11500 },
            new GameVersion { Name = "1.16", Channel = "release", OrderKey = 11600 },
            new GameVersion { Name = "1.17", Channel = "release", OrderKey = 11700 },
            new GameVersion { Name = "1.19", Channel = "release", OrderKey = 11900 },
            new GameVersion { Name = "1.20", Channel = "release", OrderKey = 12000 }
        };
        db.GameVersions.AddRange(versions);
        await db.SaveChangesAsync();

        var dims = new[] {
            new Dimension { Key = "overworld", Name = "Overworld" },
            new Dimension { Key = "nether", Name = "Nether" },
            new Dimension { Key = "end", Name = "End" }
        };
        db.Dimensions.AddRange(dims);
        await db.SaveChangesAsync();

        var contentRoot = env.ContentRootPath;
        var seedDir = Path.Combine(contentRoot, "seed");
        var blocksPath = Path.Combine(seedDir, "blocks.json");
        var entitiesPath = Path.Combine(seedDir, "entities.json");
        var biomesPath = Path.Combine(seedDir, "biomes.json");

        if (File.Exists(biomesPath) && !await db.Biomes.AnyAsync())
        {
            var jsonBiomes = await File.ReadAllTextAsync(biomesPath);
            var seeds = JsonSerializer.Deserialize<List<BiomeSeed>>(jsonBiomes, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<BiomeSeed>();

            var biomeEntities = new List<Biome>();

            foreach (var s in seeds)
            {
                var ver = versions.FirstOrDefault(v => v.Name == s.addedIn);

                var b = new Biome
                {
                    Key = s.key,
                    Name = s.name,
                    Climate = s.climate,
                    Precipitation = s.precipitation,
                    Dimension = s.dimension,
                    Height = s.height,
                    ReleaseYear = s.year,
                    AddedInVersionId = ver != null ? ver.Id : (int?)null
                };
                biomeEntities.Add(b);
            }

            db.Biomes.AddRange(biomeEntities);
            await db.SaveChangesAsync();
        }


        if (File.Exists(blocksPath))
        {
            var json = await File.ReadAllTextAsync(blocksPath);
            var blocks = JsonSerializer.Deserialize<List<BlockSeed>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<BlockSeed>();

            foreach (var s in blocks)
            {
                var dim = dims.FirstOrDefault(d => d.Key == s.dimension) ?? dims[0];
                var ver = versions.FirstOrDefault(v => v.Name == s.addedIn) ?? versions[0];

                var b = new Block
                {
                    Key = s.key,
                    Name = s.name,
                    Category = s.category,
                    DimensionId = dim.Id,
                    AddedInVersionId = ver.Id
                };
                db.Blocks.Add(b);
            }
            await db.SaveChangesAsync();
        }

        if (File.Exists(entitiesPath))
        {
            var json = await File.ReadAllTextAsync(entitiesPath);
            var entities = JsonSerializer.Deserialize<List<EntitySeed>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<EntitySeed>();

            foreach (var s in entities)
            {
                var ver = versions.FirstOrDefault(v => v.Name == s.addedIn) ?? versions[0];
                var e = new Entity
                {
                    Key = s.key,
                    Name = s.name,
                    Kind = s.kind,
                    Health = s.health,
                    Attack = s.attack,
                    AddedInVersionId = ver.Id
                };
                db.Entities.Add(e);
            }
            await db.SaveChangesAsync();

            var allEntities = await db.Entities.ToListAsync();
            foreach (var s in entities)
            {
                var e = allEntities.First(x => x.Key == s.key);
                foreach (var dKey in s.dimensions ?? Array.Empty<string>())
                {
                    var dim = dims.FirstOrDefault(d => d.Key == dKey);
                    if (dim != null)
                    {
                        db.EntityDimensions.Add(new EntityDimension
                        {
                            EntityId = e.Id,
                            DimensionId = dim.Id
                        });
                    }
                }
            }
            await db.SaveChangesAsync();
        }
    }
}
