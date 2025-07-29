using CoreDX.Extensions.DependencyInjection;
using FastEndpoints;
using MyDeltas;
using MyDeltas.Emit;
using MyDeltas.Json;
using System.Text.Json;
using TestApi.Modify;
using TestApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
var poco = PocoEmit.Mapper.Global;
IMyDeltaFactory deltaFactory = new EmitDeltaFactory();
builder.Host.UseServiceProviderFactory(new TypedImplementationFactoryServiceProviderFactory());
builder.Services
    .AddSingleton(poco)
    .AddSingleton(deltaFactory);
RegistServices(builder.Services);
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.UseFastEndpoints(cfg => {
    cfg.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    cfg.Serializer.Options.Converters.Add(new MyDeltaConverterFactory(deltaFactory));
});
app.Run();

static void RegistServices(IServiceCollection services)
{
    services.UseConverter(PocoEmit.Mapper.Global)
        .AddSingleton<UserRepository>()
        .AddSingleton<UserModifyDTOValidator>()
        .AddFastEndpoints();
    //services.AddSingletonTypedFactory(typeof(IPocoConverter<,>), (sp, converterType) => sp.GetRequiredService<IMapperOptions>().GetGenericConverter(converterType));
    //services.AddSingletonTypedFactory(typeof(IPocoCopier<,>), (sp, copierType) => sp.GetRequiredService<IMapperOptions>().GetGenericCopier(copierType));
}