using CoreDX.Extensions.DependencyInjection;
using FastEndpoints;
using MyDeltas;
using MyDeltas.Emit;
using MyDeltas.Json;
using PocoEmit;
using PocoEmit.ServiceProvider;
using System.Text.Json;
using TestApi.Models;
using TestApi.Modify;
using TestApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
var poco = PocoEmit.Mapper.Default;
//poco.UseSystemConvert();
IMyDeltaFactory deltaFactory = new EmitDeltaFactory();
builder.Host.UseServiceProviderFactory(new TypedImplementationFactoryServiceProviderFactory());
builder.Services
    .AddSingleton(poco)
    .AddSingleton(deltaFactory).AddScopedForward<User, User>();
RegistServices(builder.Services);
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseFastEndpoints(cfg => {
    cfg.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    cfg.Serializer.Options.Converters.Add(new MyDeltaConverterFactory(deltaFactory));
});
ScopeBuilder wrapper = new(app.Services);
poco.UseDefault(wrapper);
app.Run();

static void RegistServices(IServiceCollection services)
{
    //IServiceProvider provider = null;
    //provider.GetRequiredKeyedService<UserRepository>("");
    //IKeyedServiceProvider keyedProvider = null;
    //keyedProvider.GetRequiredKeyedService<UserRepository>("");
    services.UseConverter(PocoEmit.Mapper.Default)
        .AddSingleton<UserRepository>()
        .AddSingleton<UserModifyDTOValidator>()
        .AddFastEndpoints();
    //services.AddSingletonTypedFactory(typeof(IPocoConverter<,>), (sp, converterType) => sp.GetRequiredService<IMapper>().GetGenericConverter(converterType));
    //services.AddSingletonTypedFactory(typeof(IPocoCopier<,>), (sp, copierType) => sp.GetRequiredService<IMapper>().GetGenericCopier(copierType));
}