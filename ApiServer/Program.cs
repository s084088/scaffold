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

//------------------------------------------------------------------------------------Ӧ�ô�����------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//������������
builder.BaseInit().Services
    .AddOptions()
    .AddMemoryCache()
    .AddScoped<RequestTemporaryData>()
    .Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"))
    .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
    .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()
    .AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>()
    .AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

//����˿���
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("Test", new OpenApiInfo { Title = "���Խӿ�", Version = "Test" });
    s.SwaggerDoc("Users", new OpenApiInfo { Title = "�û��ӿ�", Version = "Users" });
    s.SwaggerDoc("Quantifiers", new OpenApiInfo { Title = "����ѵ��", Version = "Quantifiers" });
    s.SwaggerDoc("Base", new OpenApiInfo { Title = "�����ӿ�", Version = "Base" });
    s.DocInclusionPredicate((d, a) => a.GroupName.ToUpper() == d.ToUpper());
    string b = Path.GetDirectoryName(typeof(Program).Assembly.Location);
    s.IncludeXmlComments(Path.Combine(b, "API.xml"));
    s.IncludeXmlComments(Path.Combine(b, "DTO.xml"));

    s.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Description = "�������¼���ȡ���� token �Ա������Ҫtoken��֤�Ľӿ�",
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

//����������,Json���л�����,�Զ��������֤��
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

//���ö˿ڼ�����֤���
builder.WebHost.ConfigureKestrel(o =>
{
    o.ListenAnyIP(builder.Configuration["Environment:HttpPort"].ToInt());
    //o.ListenAnyIP(builder.Configuration["Environment:HttpsPort"].ToInt(), l => l.UseHttps("5368162__aixueshi.top.pfx", "GkGHOvs3"));
});

//-----------------------------------------------------------------------------------����Ӧ�ò�����-----------------------------------------------------------------------------------
WebApplication app = builder.Build();

//�������Ա�ʶ
if (app.Environment.IsDevelopment()) Res.IsTest = true;

//�������  ����˿���
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

//���ýӿ�ͳ��ͳ��
app.UseCustomMiddleware();

//����·��  ��������
app.MapControllers();
app.Run();