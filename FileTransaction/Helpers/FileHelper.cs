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
                    var maxInteger = int.MinValue;
                    var maxIntegerRowNumber = int.MinValue;

                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var rowCount = 0;

                        while (reader.Peek() >= 0)
                        {
                            rowCount++;

                            var row = await reader.ReadLineAsync();

                            if (string.IsNullOrEmpty(row)) continue;

                            var rowAsNumber = int.Parse(row!.Trim());

                            if (rowAsNumber > maxInteger)
                            {
                                maxInteger = rowAsNumber;
                                maxIntegerRowNumber = rowCount;
                            }
                        }
                    }

                    using (var fileWriter = new StreamWriter("Result.txt"))
                    {
                        fileWriter.WriteLine($"The max integer is: {maxInteger} and it is located on line: {maxIntegerRowNumber}");
                    }

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;

                    return new Response { IsOk = true, Message = $"Max integer is: {maxInteger} and it was found on line: {maxIntegerRowNumber}. Processing took about {elapsedMs} milliseconds." };
                }
                catch { return new Response { IsOk = false, Message = "Error while finding the maximum value. Please check and reupload the file, then try again" }; };

            }
            else return new Response { IsOk = false, Message = "File seems to be empty" };
        }
    }
}
