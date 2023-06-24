using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using ODataBookStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookDb")));
//odata
ODataConventionModelBuilder modelBuilder = new();
modelBuilder.EntitySet<Book>("Book");
modelBuilder.EntitySet<Press>("Presses");
builder.Services.AddControllers().AddOData(option => option.AddRouteComponents("odata", modelBuilder.GetEdmModel())
        .Select()
        .Filter()
        .OrderBy()
        .SetMaxTop(20)
        .Count()
        .Expand());

builder.Services.AddMvc(options => options.EnableEndpointRouting=false);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseODataBatching();
app.UseRouting();
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller}/{action}/{id?}",
        defaults: new { controller = "Book", action = "Index" });
});
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
