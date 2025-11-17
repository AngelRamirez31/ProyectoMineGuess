namespace MineGuess.Api.Models;

public class Dimension
{
    public int Id { get; set; }
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";

    public List<Block> Blocks { get; set; } = new();
    public List<EntityDimension> EntityDimensions { get; set; } = new();
}

public class Biome
{
    public int Id { get; set; }
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";

    public string? Climate { get; set; }
    public string? Precipitation { get; set; }
    public string? Dimension { get; set; }
    public string? Height { get; set; }
    public int? ReleaseYear { get; set; }

    public int? AddedInVersionId { get; set; }
    public GameVersion? AddedInVersion { get; set; }

    public List<BlockBiome> BlockBiomes { get; set; } = new();
}
