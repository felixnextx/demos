using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Stream download test!\r\n/downloadcsv");

app.MapGet("/downloadcsv", async (HttpContext context) =>
{
    string fileName = $"{DateTime.Now.ToString("yyyyMMdd")}.csv";
    context.Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
    context.Response.ContentType = "application/octet-stream";
    await context.Response.StartAsync();
    await GenerateAndWriteAsync(context.Response.Body);
});

app.Run();



async Task GenerateAndWriteAsync(Stream responseStream)
{
    for (var i = 0; i < 10000000; i++)
    {
        var line = $"{i.ToString().PadLeft(8, '0')},TestName{i}\r\n";
        await responseStream.WriteAsync(Encoding.UTF8.GetBytes(line));
        await responseStream.FlushAsync();
    }
}
