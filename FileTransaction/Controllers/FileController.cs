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
            if (!file.ContentType.StartsWith("text")) return BadRequest("Wrong file format, please upload a .txt file");
            if (file.Length > 0)
            {
                try
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    var result = 0;

                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (reader.Peek() >= 0)
                        {
                            var row = await reader.ReadLineAsync();
                            var rowAsNumber = int.Parse(row.Trim());

                            result += rowAsNumber;
                        }
                    }

                    using (var fileWriter = new StreamWriter("Result.txt"))
                    {
                        fileWriter.WriteLine($"Sum of integers is: {result}");
                    }

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;

                    return Ok($"Sum is: {result}. Processing took about {elapsedMs} milliseconds.");
                }
                catch { return BadRequest("Error while calculating the sum. Please check and reupload the file, then try again"); }

            }
            else return BadRequest("File seems to be empty");
        }

        [HttpGet]
        [Route("/downloadFile")]
        public IActionResult DownloadFile()
        {
            return File(System.IO.File.ReadAllBytes("Result.txt"), System.Net.Mime.MediaTypeNames.Text.Plain, "Result.txt");
        }
    }
}