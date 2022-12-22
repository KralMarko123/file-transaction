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
            if (!file.ContentType.StartsWith("text")) return BadRequest("Wrong Format");
            if (file.Length > 0)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = 0;

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        var row = await reader.ReadLineAsync();
                        var rowAsNumber = Int32.Parse(row.Trim());

                        result += rowAsNumber;
                    }
                }

                using (var fileWriter = new StreamWriter("Result.txt"))
                {
                    fileWriter.WriteLine($"Sum of integers is: {result}");
                }

                var resultFileBytes = System.IO.File.ReadAllBytes("Result.txt");
                var fileName = "Result.txt";

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                var response = new
                {
                    result,
                    elapsedMs,
                    file = File(resultFileBytes, System.Net.Mime.MediaTypeNames.Text.Plain, fileName),
                };

                return Ok(response);
            }
            else return BadRequest("Empty file");
        }
    }
}