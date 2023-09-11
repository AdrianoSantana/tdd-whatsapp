using TicketWhatsApp.Domain.Interfaces;

namespace TicketWhatsApp.Domain.Models;

public class MessageDbSettings : IDbSettings
{
  public MessageDbSettings()
  {

  }

  public MessageDbSettings(string dbCollectionName, string databaseName, string connectionString)
  {
    this.DbCollectionName = dbCollectionName;
    this.DatabaseName = databaseName;
    this.ConnectionString = connectionString;
  }
  public string DbCollectionName { get; set; } = string.Empty;
  public string ConnectionString { get; set; } = string.Empty;
  public string DatabaseName { get; set; } = string.Empty;
}