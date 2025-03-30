using Microsoft.OpenApi.Models; //for swagger
using Local.DB; //for local PSQL itembase accessed in LocalDB.cs
using DotNetEnv; //for .env file access
var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();


// -------------------- SERVICE Modules -----------------------
// Pulls Swagger into the service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
   c.SwaggerDoc("v1", new OpenApiInfo { Title = "Testing API", Description = "Trying out all of the basic API things that the things thing.", Version = "v1" });
});

// Initiates the database connection
DataDB.Init();

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
app.MapGet("/item", DataDB.GetAllItems); // When parameterless

//Use lambda functions when parameters needed
//The URL parameters are automatically pulled out for use
app.MapGet("/item/{id}", (int id) => DataDB.GetItem(id));

// TODO: IMPLEMENT BELOW WITH NEW LOCAL DB CLASS
//Pulls the item object from request body too
app.MapPost("/item", (Item newItem) => DataDB.CreateItem(newItem));
// app.MapPut("/item", (Item newItem) => DataDB.UpdateItem(newItem));

app.MapDelete("/item/{id}", (int id) => DataDB.RemoveItem(id));

// SEND IT!
app.Run();