using CbeViewer.Data;
using Microsoft.AspNetCore.Mvc;

namespace CbeViewer.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideoController(DataRepository dataRepository) : ControllerBase
{
    [HttpGet("stream/{folder}/{video}")]
    public ActionResult Stream(string folder, string video)
    {
        var path = dataRepository.GetVideoPath(folder, video);
        
        if(!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        var videoStream = System.IO.File.OpenRead(path);
        
        return File(videoStream, "video/mp4", enableRangeProcessing: true);
    }
}