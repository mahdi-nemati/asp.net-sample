using Endpoint.Extentions;

var builder = WebApplication.CreateBuilder(args);

// regester services
var app = builder.ConfigureService();

// regester pipeline
app.ConfigurePipeline();



app.Run();
