using FileTransaction.Models;

namespace FileTransaction.Helpers
{
    public static class FileHelper
    {
        public static async Task<Response> ProcessFile(IFormFile file)
        {
            if (!file.ContentType.StartsWith("text")) return new Response { IsOk = false, Message = "Wrong file format, please upload a .txt file" };
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

                            if (string.IsNullOrEmpty(row)) continue;

                            var rowAsNumber = int.Parse(row!.Trim());

                            result += rowAsNumber;
                        }
                    }

                    using (var fileWriter = new StreamWriter("Result.txt"))
                    {
                        fileWriter.WriteLine($"Sum of integers is: {result}");
                    }

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;

                    return new Response { IsOk = true, Message = $"Sum is: {result}. Processing took about {elapsedMs} milliseconds." };
                }
                catch { return new Response { IsOk = false, Message = "Error while calculating the sum. Please check and reupload the file, then try again" }; };

            }
            else return new Response { IsOk = false, Message = "File seems to be empty" };
        }
    }
}
