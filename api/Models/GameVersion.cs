
namespace MineGuess.Api.Models;

public class GameVersion
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Channel { get; set; } = "release"; 
    public int OrderKey { get; set; } 
    public DateOnly? ReleaseDate { get; set; }

    
}
