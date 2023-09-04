using Microsoft.EntityFrameworkCore;
using TicketWhatsApp.Domain.Models.Core;

namespace TicketWhatsApp.Persistence;

public class TicketWhatsAppDbContext : DbContext
{
  public TicketWhatsAppDbContext(DbContextOptions<TicketWhatsAppDbContext> options) : base(options)
  {

  }

  public DbSet<Ticket> Tickets { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
  }
}
