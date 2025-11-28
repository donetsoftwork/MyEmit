using MyDeltas;
using MyDeltas.Json;
using PocoEmit;
using PocoEmit.Mvc;
using TestApi2.Domain.Models;

var builder = WebApplication.CreateSlimBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();

ConfigureMapper(app.Services);
app.Run();


static void ConfigureMapper(IServiceProvider serviceProvider)
{
    var accessor = serviceProvider.GetService<IHttpContextAccessor>();
    var mapper = Mapper.Default;
    mapper.UseDefault(() => UserId.NewId());
    mapper.UseContext(serviceProvider, accessor);
}

static void ConfigureServices(IServiceCollection services)
{
    // Add services to the container.
    IMyDeltaFactory deltaFactory = new EmitDeltaFactory(Mapper.Default, StringComparer.OrdinalIgnoreCase);
    services.AddSingleton(deltaFactory)
        .AddHttpContextAccessor()
        .AddControllers()        
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new MyDeltaConverterFactory(deltaFactory));
        });
}