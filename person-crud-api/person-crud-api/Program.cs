using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using person_crud_api.Model.Context;
using person_crud_api.Business;
using person_crud_api.Business.Implementations;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using person_crud_api.Bussiness;
using EvolveDb;
using MySqlConnector;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(
    options =>
    {
        options.ReportApiVersions = true;
    })
    .AddMvc(
        options =>
        {
            options.Conventions.Add(new VersionByNamespaceConvention());
        });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("V1", new OpenApiInfo { Title = "Person API", Version = "V1" });
    c.DocInclusionPredicate((version, desc) =>
    {
        var versions = desc.CustomAttributes().OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
        return versions.Any(v => $"V{v}" == version);
    });
});

var connectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");

builder.Services.AddDbContext<MySqlContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


//configurando evolve
if (builder.Environment.IsDevelopment())
{
    var evolveConnection = new MySqlConnection(connectionString);
    var evolve = new Evolve(evolveConnection, Log.Information)
    {
        Locations = new List<string> { "db/migrations", "db/dataset" },
        IsEraseDisabled = true,
    };
    evolve.Migrate();
}


builder.Services.AddScoped<IPersonBusiness, PersonBussinesImplementations>();
builder.Services.AddScoped<IPersonRepository, PersonRepositoryImplementations>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/V1/swagger.json", "Person API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
