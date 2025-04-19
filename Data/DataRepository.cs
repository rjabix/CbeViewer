using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CbeViewer.Data;

public class DataRepository(ApplicationDbContext dbContext, IWebHostEnvironment environment)
{
    private readonly string _dataPath = environment.IsDevelopment() ? 
        "/home/rjabix/DockerTests/CbeViewer/Videos" 
        : 
        "/env/Videos";
    public List<string?> GetFolders()
        => Directory.GetDirectories(_dataPath)
            .Select(Path.GetFileName)
            .ToList();
    
    public List<string?> GetVideos(string folder)
    => Directory.GetFiles(_dataPath + "/" + folder, "*.mp4")
        .Select(Path.GetFileName)
        .ToList();
    
    public List<string?> GetSubtitles(string folder, string video)
    => Directory.GetFiles($"{_dataPath}/{folder}", $"{video.Split('.')[0]}*.vtt")
            .Select(Path.GetFileName).ToList();
    
    
    public string GetVideoPath(string folder, string video)
        => $"{_dataPath}/{folder}/{video}";
    
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