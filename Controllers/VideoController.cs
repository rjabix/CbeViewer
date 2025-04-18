using System.Text.Json;
using CbeViewer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CbeViewer.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideoController(DataRepository dataRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ControllerBase
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
    
    [Authorize]
    [HttpGet("initial-seconds/{folder}/{video}")]
    public async Task<ActionResult<int>> GetInitialSeconds(string folder, string video)
    {
        var user = await userManager.GetUserAsync(User);
        var dict = user.StartSeconds == null ? new Dictionary<string, uint>() : JsonSerializer.Deserialize<Dictionary<string, uint>>(user.StartSeconds);
        if (!dict.ContainsKey(folder + "/" + video))
            return Ok(0);
        return Ok(dict[folder + "/" + video]);
    }
    
    [Authorize]
    [HttpPost("initial-seconds/{folder}/{video}")]
    public async Task<ActionResult> SetInitialSeconds(string folder, string video, [FromQuery]double time)
    {
        if (time == 0) return Ok();
        var user = await userManager.GetUserAsync(User);
        var dict = user!.StartSeconds == null ? new Dictionary<string, uint>() : JsonSerializer.Deserialize<Dictionary<string, uint>>(user.StartSeconds);
        dict![folder + "/" + video] = (uint)Math.Floor(time!);
        user.StartSeconds = JsonSerializer.Serialize(dict);
        await userManager.UpdateAsync(user);
        
        return Ok();
    }
}