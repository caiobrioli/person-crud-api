using Asp.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using person_crud_api.Model.Context;
using person_crud_api.Services;
using person_crud_api.Services.Implementations;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;

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

builder.Services.AddScoped<IPersonService, PersonServiceImplementations>();

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
