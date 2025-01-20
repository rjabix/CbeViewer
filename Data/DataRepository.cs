using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CbeViewer.Data;

public class DataRepository(ApplicationDbContext dbContext, IWebHostEnvironment environment)
{
    private readonly string DataPath = environment.IsDevelopment() ? 
        "/home/rjabix/DockerTests/CbeViewer/Videos" 
        : 
        "/env/Videos";
    public List<string?> GetFolders()
        => Directory.GetDirectories(DataPath)
            .Select(Path.GetFileName)
            .ToList();
    
    public List<string?> GetVideos(string folder)
    => Directory.GetFiles(DataPath + "/" + folder)
        .Select(Path.GetFileName)
        .ToList();
    
    public string GetVideoPath(string folder, string video)
        => $"{DataPath}/{folder}/{video}";
    
    public async Task<uint> GetInitialSeconds(string folder, string video, string UserName)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == UserName);
        if (user is null) return 0;
        
        var dict = user.StartSeconds == null ? new Dictionary<string, uint>() : JsonSerializer.Deserialize<Dictionary<string, uint>>(user.StartSeconds);
        return !dict.ContainsKey(folder + "/" + video) ? (uint)0 : dict[folder + "/" + video];
    }
    
    public async Task SetInitialSeconds(string folder, string video, double time, string UserName)
    {
        if (time == 0) return;
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == UserName);
        var dict = user.StartSeconds == null ? new Dictionary<string, uint>() : JsonSerializer.Deserialize<Dictionary<string, uint>>(user.StartSeconds);
        dict[folder + "/" + video] = (uint)Math.Floor(time!);
        user.StartSeconds = JsonSerializer.Serialize(dict);
        await dbContext.SaveChangesAsync();
        return;
    }
}