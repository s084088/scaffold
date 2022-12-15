using System.IO;
using ApiServer.Config;
using ApiServer.Middlewares;
using AspNetCoreRateLimit;
using Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

//------------------------------------------------------------------------------------应用创建器------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//配置限流策略
builder.BaseInit().Services
    .AddOptions()
    .AddMemoryCache()
    .AddScoped<RequestTemporaryData>()
    .Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"))
    .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
    .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()
    .AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>()
    .AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

//配置丝袜哥
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("Test", new OpenApiInfo { Title = "测试接口", Version = "Test" });
    s.SwaggerDoc("Users", new OpenApiInfo { Title = "用户接口", Version = "Users" });
    s.SwaggerDoc("Quantifiers", new OpenApiInfo { Title = "量词训练", Version = "Quantifiers" });
    s.SwaggerDoc("Base", new OpenApiInfo { Title = "基础接口", Version = "Base" });
    s.DocInclusionPredicate((d, a) => a.GroupName.ToUpper() == d.ToUpper());
    string b = Path.GetDirectoryName(typeof(Program).Assembly.Location);
    s.IncludeXmlComments(Path.Combine(b, "API.xml"));
    s.IncludeXmlComments(Path.Combine(b, "DTO.xml"));

    s.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Description = "请输入登录后获取到的 token 以便调试需要token验证的接口",
        Name = "token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference()
            {
                Id = "token",
                Type = ReferenceType.SecurityScheme
            }
        },
        Array.Empty<string>()
    }});
});

//配置拦截器,Json序列化配置,自定义参数验证等
builder.Services.AddControllers(o => o.Filters.AddCustomFilters()).AddNewtonsoftJson(m =>
{
    m.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    m.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    m.SerializerSettings.ContractResolver = CustomContractResolver.Default.AddEmptyCollectionIgnore();
    m.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
    m.SerializerSettings.Converters.Add(new DateOnlyCanNullJsonConverter());
    m.SerializerSettings.Converters.Add(new TimeOnlyJsonConverter());
    m.SerializerSettings.Converters.Add(new TimeOnlyCanNullJsonConverter());
}).ConfigureApiBehaviorOptions(a => a.SuppressModelStateInvalidFilter = true);

//配置端口监听和证书等
builder.WebHost.ConfigureKestrel(o =>
{
    o.ListenAnyIP(builder.Configuration["Environment:HttpPort"].ToInt());
    //o.ListenAnyIP(builder.Configuration["Environment:HttpsPort"].ToInt(), l => l.UseHttps("5368162__aixueshi.top.pfx", "GkGHOvs3"));
});

//-----------------------------------------------------------------------------------创建应用并启动-----------------------------------------------------------------------------------
WebApplication app = builder.Build();

//开发调试标识
if (app.Environment.IsDevelopment()) Res.IsTest = true;

//允许跨域  启用丝袜哥
app.UseCors(c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
.UseSwagger(s => s.RouteTemplate = "swagger/{documentName}/swagger.json")
.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint($"/swagger/Test/swagger.json", "Test");
    s.SwaggerEndpoint($"/swagger/Users/swagger.json", "Users");
    s.SwaggerEndpoint($"/swagger/Quantifiers/swagger.json", "Quantifiers");
    s.SwaggerEndpoint($"/swagger/Base/swagger.json", "Base");
    s.EnableFilter();
    s.RoutePrefix = "FTOCUU5XXIDHB0SUPP0NCW4RJ9XA087EJBQPKUQNBCU0NM2XEWXJG3HZXD7Z8H8H7GVG5YQK09ZR45S0H57CEAJUOSORCLUXF1K";
});

//启用接口统计统计
app.UseCustomMiddleware();

//配置路由  启动程序
app.MapControllers();
app.Run();