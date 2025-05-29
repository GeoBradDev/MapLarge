var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();
// app.UseHttpsRedirection(); // (optional)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.Run();