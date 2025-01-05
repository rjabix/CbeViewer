using System.ComponentModel.DataAnnotations.Schema;

namespace CbeViewer.Data;

public class VideoModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string VideoUrl { get; set; }
    
}