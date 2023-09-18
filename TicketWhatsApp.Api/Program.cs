using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TicketWhatsApp.Domain.Interfaces;
using TicketWhatsApp.Domain.Models;
using TicketWhatsApp.Domain.Service;
using TicketWhatsApp.Persistence;
using TicketWhatsApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MessageDbSettings>(builder.Configuration.GetSection(nameof(MessageDbSettings)));
builder.Services.AddSingleton<IDbSettings>(s => s.GetRequiredService<IOptions<MessageDbSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MessageDbSettings:ConnectionString")));

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IGetInfoService, MockGetInfoService>();
builder.Services.AddScoped<IHandleWebhookService, HandleWebHookService>();
builder.Services.AddScoped<IMessageAnswerService, MessageAnswerService>();
builder.Services.AddScoped<ISendMessageService, PositusSendMessageService>();
builder.Services.AddScoped<HttpClient>();

var connStrings = "DataSource=myDb.db";
var conn = new SqliteConnection(connStrings);
conn.Open();

builder.Services.AddDbContext<TicketWhatsAppDbContext>(opt => opt.UseSqlite(conn));

EnsureDatabaseCreated(conn);

static void EnsureDatabaseCreated(SqliteConnection conn)
{
  var builder = new DbContextOptionsBuilder<TicketWhatsAppDbContext>();
  builder.UseSqlite(conn);
  using var context = new TicketWhatsAppDbContext(builder.Options);
  context.Database.EnsureCreated();
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
