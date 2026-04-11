using AspireDemoTemplate.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireDemoTemplate.ApiService;

public class SampleDbContext : DbContext
{
    public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    {
    }

    public DbSet<SampleItem> SampleItems { get; set; }
}
