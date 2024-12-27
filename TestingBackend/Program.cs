using Microsoft.OpenApi.Models; //for swagger
using HardCoded.DB; //for hardcoded database defined in HardCodedDB.cs

var builder = WebApplication.CreateBuilder(args);
    
// -------------------- SERVICE Modules -----------------------
// Pulls Swagger into the service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Testing API", Description = "Trying out all of the basic API things that the things thing.", Version = "v1" });
});
    
// ----------------------- APP --------------------------------
var app = builder.Build(); //
    
// Swagger shouldn't be in prod, so check
if (app.Environment.IsDevelopment())
{
    // defines the swagger endpoints, with separate one's for each version
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Testing API V1");
   });
}

// ----------------------- ROUTES ------------------------------
    
app.MapGet("/", () => "Hello World!");
app.MapGet("/data", DataDB.GetAllData); // When parameterless

//Use lambda functions when parameters needed
//The URL parameters are automatically pulled out for use
app.MapGet("/data/{id}", (int id) => DataDB.GetData(id));

//Pulls the data object from request body too
app.MapPost("/data", (Data newData) => DataDB.CreateData(newData));
app.MapPut("/data", (Data newData) => DataDB.UpdateData(newData));

app.MapDelete("/data/{id}", (int id) => DataDB.RemoveData(id));

// SEND IT!
app.Run();