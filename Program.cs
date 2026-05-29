using PiAssistant.Components;
using PiAssistant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<EmbeddingService>();
builder.Services.AddScoped<SystemPromptService>();
builder.Services.AddSingleton<ToolRegistry>();
builder.Services.AddSingleton<ToolExecutor>();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<OllamaService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(300);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
