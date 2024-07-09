using Microsoft.EntityFrameworkCore;
using WebApiDiary.Models;
namespace WebApiDiary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<DiaryEntry> DiaryEntries { get; set; }
    }
}
