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
    
}