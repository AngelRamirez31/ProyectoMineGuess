using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MVP_ProyectoFinal.Models.Elementos;

namespace MVP_ProyectoFinal.Services;

public class ApiClientOriginal
{
    private readonly HttpClient _http;
    private readonly string _base;

    public ApiClientOriginal(string baseUrl)
    {
        _http = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        _base = baseUrl.TrimEnd('/');
    }

        private static int ComputeEntityHealth(string? key, string? kind, int? health)
    {
        if (health.HasValue && health.Value > 0) return health.Value;

        var k = (key ?? string.Empty).ToLowerInvariant();

        switch (k)
        {
            case "pig": return 10;
            case "sheep": return 8;
            case "cow": return 10;
            case "chicken": return 4;
            case "squid": return 10;
            case "mooshroom": return 10;
            case "villager": return 20;
            case "snow_golem": return 4;
            case "wolf": return 16;
            case "enderman": return 40;
            case "zombie_pigman": return 20;
            case "spider": return 16;
            case "zombie": return 20;
            case "skeleton": return 20;
            case "creeper": return 20;
            case "cave_spider": return 12;
            case "slime": return 16;
            case "ghast": return 20;
            case "blaze": return 20;
            case "magma_cube": return 16;
            case "silverfish": return 8;
            case "ender_dragon": return 200;
            case "wither": return 300;
            case "wither_skeleton": return 20;
            case "witch": return 26;
            case "guardian": return 30;
            case "elder_guardian": return 80;
            case "iron_golem": return 100;
            case "shulker": return 30;
            case "husk": return 20;
            case "stray": return 20;
            case "drowned": return 20;
            case "phantom": return 20;
            case "llama": return 20;
            case "panda": return 20;
            case "fox": return 10;
            case "bee": return 10;
            case "axolotl": return 14;
            case "goat": return 20;
            case "frog": return 10;
            case "tadpole": return 6;
            case "warden": return 500;
            case "allay": return 20;
            case "glow_squid": return 10;
            case "piglin": return 16;
            case "piglin_brute": return 50;
            case "hoglin": return 40;
            case "zoglin": return 40;
            case "strider": return 20;
            case "zombified_piglin": return 20;
            case "dolphin": return 10;
            case "turtle": return 30;
            case "parrot": return 6;
            case "trader_llama": return 20;
            case "wandering_trader": return 20;
            case "ravager": return 100;
            case "pillager": return 24;
            case "vindicator": return 24;
            case "evoker": return 24;
        }

        var tipo = (kind ?? string.Empty).ToLowerInvariant();
        if (tipo == "boss") return 300;
        if (tipo == "hostile") return 20;
        if (tipo == "neutral") return 20;
        if (tipo == "passive") return 10;

        return 10;
    }


    private static int ComputeEntityAttack(string? key, string? kind, int? attack)
    {
        if (attack.HasValue && attack.Value > 0) return attack.Value;

        var k = (key ?? string.Empty).ToLowerInvariant();

        switch (k)
        {
            case "bee": return 4;
            case "blaze": return 12;
            case "cave_spider": return 4;
            case "creeper": return 86;
            case "dolphin": return 6;
            case "drowned": return 6;
            case "elder_guardian": return 16;
            case "ender_dragon": return 20;
            case "enderman": return 14;
            case "evoker": return 12;
            case "ghast": return 24;
            case "goat": return 4;
            case "guardian": return 12;
            case "hoglin": return 16;
            case "husk": return 6;
            case "iron_golem": return 15;
            case "magma_cube": return 12;
            case "phantom": return 6;
            case "piglin": return 16;
            case "piglin_brute": return 26;
            case "pillager": return 8;
            case "ravager": return 24;
            case "shulker": return 8;
            case "silverfish": return 2;
            case "skeleton": return 8;
            case "slime": return 8;
            case "spider": return 6;
            case "stray": return 8;
            case "vindicator": return 26;
            case "wandering_trader": return 0;
            case "warden": return 60;
            case "witch": return 12;
            case "wither": return 16;
            case "wither_skeleton": return 16;
            case "wolf": return 8;
            case "zoglin": return 16;
            case "zombie": return 6;
            case "zombie_pigman": return 16;
            case "zombified_piglin": return 16;
        }

        var tipo = (kind ?? string.Empty).ToLowerInvariant();
        if (tipo == "boss") return 20;
        if (tipo == "hostile") return 6;
        if (tipo == "neutral") return 4;

        return 0;
    }

    private static int ComputeEntityYear(string? key, string? addedInVersion)
    {
        var k = (key ?? string.Empty).ToLowerInvariant();

        switch (k)
        {
            case "wither":
            case "wither_skeleton":
            case "witch":
                return 2012;
            case "iron_golem":
                return 2012;
            case "guardian":
            case "elder_guardian":
                return 2014;
            case "shulker":
                return 2016;
            case "husk":
            case "stray":
                return 2016;
            case "drowned":
            case "phantom":
                return 2018;
            case "llama":
                return 2016;
            case "panda":
            case "fox":
                return 2019;
            case "bee":
                return 2019;
        }

        return YearFromVersion(addedInVersion);
    }

    private static int YearFromVersion(string? v)
    {
        if (string.IsNullOrWhiteSpace(v)) return 0;
        v = v.Trim();
        if (v.StartsWith("Alpha 1.0", StringComparison.OrdinalIgnoreCase)) return 2009;
        if (v.StartsWith("Alpha 1.1", StringComparison.OrdinalIgnoreCase) || v.StartsWith("Alpha 1.2", StringComparison.OrdinalIgnoreCase)) return 2010;
        if (v.StartsWith("Beta", StringComparison.OrdinalIgnoreCase)) return 2011;
        if (!v.StartsWith("1.", StringComparison.OrdinalIgnoreCase)) return 0;
        var parts = v.Split('.');
        if (parts.Length < 2) return 0;
        if (!int.TryParse(parts[1], out var minor)) return 0;
        switch (minor)
        {
            case 0: return 2011;
            case 1: return 2012;
            case 2: return 2012;
            case 3: return 2012;
            case 4: return 2012;
            case 5: return 2013;
            case 6: return 2013;
            case 7: return 2013;
            case 8: return 2014;
            case 9: return 2016;
            case 10: return 2016;
            case 11: return 2016;
            case 12: return 2017;
            case 13: return 2018;
            case 14: return 2019;
            case 15: return 2019;
            case 16: return 2020;
            case 17: return 2021;
            case 18: return 2021;
            case 19: return 2022;
            case 20: return 2023;
            case 21: return 2024;
            default: return 0;
        }
    }

    private static bool IsCraftable(BlockDto b)
    {
        var key = (b.Key ?? string.Empty).ToLowerInvariant();
        var category = (b.Category ?? string.Empty).ToLowerInvariant();

        if (category == "ore" || key.Contains("ore"))
            return false;

        if (category == "decorative" || category == "functional" || category == "light" || category == "redstone" || category == "storage")
            return true;

        if (string.IsNullOrEmpty(category))
        {
            if (key == "glass")
                return true;
        }

        return false;
    }

    private static bool IsExterior(BlockDto b)
    {
        var dimension = b.Dimension;
        if (!string.Equals(dimension, "overworld", StringComparison.OrdinalIgnoreCase))
            return false;

        var key = (b.Key ?? string.Empty).ToLowerInvariant();
        var category = (b.Category ?? string.Empty).ToLowerInvariant();

        if (category == "ore" || key.Contains("ore") || key.Contains("deepslate") || key.Contains("ancient_debris"))
            return false;

        return true;
    }


    public async Task<List<Bloque>> GetBlocksAsync()
    {
        var url = $"{_base}/api/v1/blocks?page=1&page_size=500";
        var doc = await _http.GetFromJsonAsync<BlocksList>(url);
        var items = doc?.items ?? new List<BlockDto>();
        return items.Select(b => new Bloque
        {
            Nombre = b.Name ?? b.Key ?? string.Empty,
            Version = b.AddedInVersion ?? string.Empty,
            Funcion = b.Category ?? string.Empty,
            Bioma = (b.Biomes?.FirstOrDefault() ?? b.Dimension ?? string.Empty),
            EsCrafteable = IsCraftable(b),
            EsDeExterior = IsExterior(b),
            YearLanzamiento = YearFromVersion(b.AddedInVersion)
        }).ToList();
    }

    public async Task<List<Entidad>> GetEntitiesAsync()
    {
        var url = $"{_base}/api/v1/entities?page=1&page_size=500";
        var doc = await _http.GetFromJsonAsync<EntitiesList>(url);
        var items = doc?.items ?? new List<EntityDto>();
        return items.Select(e =>
        {
            var tipo = e.Kind ?? string.Empty;
            var vida = ComputeEntityHealth(e.Key, tipo, e.Health);
            var ataque = ComputeEntityAttack(e.Key, tipo, e.Attack);

            return new Entidad
            {
                Id = 0,
                Nombre = e.Name ?? e.Key ?? string.Empty,
                Tipo = tipo,
                Vida = vida,
                Ataque = ataque,
                Dimension = e.Dimensions?.FirstOrDefault() ?? string.Empty,
                YearLanzamiento = ComputeEntityYear(e.Key, e.AddedInVersion)
            };
        }).ToList();
    }

    private record BlocksList(List<BlockDto> items, int total);
    private record BlockDto(string? Key, string? Name, string? Category, bool? IsBreakable, string? Dimension, List<string>? Biomes, string? AddedInVersion);
    private record EntitiesList(List<EntityDto> items, int total);
    private record EntityDto(string? Key, string? Name, string? Kind, int? Health, int? Attack, List<string>? Dimensions, string? AddedInVersion);
}