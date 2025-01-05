namespace CbeViewer.Data;

public class DataRepository(ApplicationDbContext dbContext, IWebHostEnvironment environment)
{
    private readonly string DataPath = environment.IsDevelopment() ? 
        Directory.GetCurrentDirectory() + "/Data/" 
        : 
        "/env/";
    public List<string?> GetFolders()
        => Directory.GetDirectories(DataPath + "Videos")
            .Select(Path.GetFileName)
            .ToList();
    
    public List<string?> GetVideos(string folder)
    => Directory.GetFiles(DataPath + "Videos/" + folder)
        .Select(Path.GetFileName)
        .ToList();
    
    public string GetVideoPath(string folder, string video)
        => Path.Combine(DataPath, "Videos", folder, video);
    
}