using Client;
using Fleet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHttpClient<ServerClient>(client =>
    {
        client.BaseAddress = new Uri("https://azuremanagedserver.azurewebsites.net");
    })
    .AddHttpMessageHandler(sp => new ManagedIdentityAuthenticationDelegationHandler("api://AzureManagedServer/.default", sp.GetRequiredService<ILogger<ManagedIdentityAuthenticationDelegationHandler>>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseDeveloperExceptionPage();

app.UseAuthorization();

app.MapControllers();

app.Run();
