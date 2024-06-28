using CancellationTokenExampl.Contracts;
using CancellationTokenExampl.Middleware;
using CancellationTokenExampl.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IExampleRepository, ExampleRepository>();

var app = builder.Build();
app.UseMiddleware<TaskCancellationHandingMiddleware>();
app.MapControllers();

app.Run();
