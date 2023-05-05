using FileTransaction.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FileTransaction.Controllers
{
    [ApiController]
    [Route("")]
    [EnableCors("corsPolicy")]
    public class FileController : ControllerBase
    {
        [HttpPost]
        [Route("/processFile")]
        public async Task<IActionResult> ProcessFile(IFormFile file)
        {
            var response = await FileHelper.ProcessFile(file);

            if (response.IsOk) return Ok(response.Message);
            else return BadRequest(response.Message);
        }

        [HttpGet]
        [Route("/downloadFile")]
        public IActionResult DownloadFile()
        {
            return File(System.IO.File.ReadAllBytes("Result.txt"), System.Net.Mime.MediaTypeNames.Text.Plain, "Result.txt");
        }
    }
}