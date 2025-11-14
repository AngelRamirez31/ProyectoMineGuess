using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MineGuess.Api.Data;

namespace MineGuess.Api.Ingestion;

public class FullImporter
{
    public async Task RunAsync(AppDb db, string fromVersion = "1.0", string toVersion = "latest", CancellationToken ct = default)
    {
        await db.Database.EnsureCreatedAsync(ct);
        var anyVersion = await db.GameVersions.AnyAsync(ct);
        if (!anyVersion)
        {
        }
    }
}
