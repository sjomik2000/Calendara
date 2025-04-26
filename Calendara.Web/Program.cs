var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var defaultFilesOptions = new DefaultFilesOptions();
defaultFilesOptions.DefaultFileNames.Clear();
defaultFilesOptions.DefaultFileNames.Add("html/index.html");
app.UseDefaultFiles(defaultFilesOptions);

app.UseStaticFiles();
app.Urls.Add("https://localhost:5001");
app.Run();
