using LottoScaper.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LottoScaper.DAL
{
    public class LottoDbContext : DbContext
    {
        public LottoDbContext(DbContextOptions<LottoDbContext> options) : base(options)
        {

        }

        public DbSet<LottoDraw> Draws { get; set; }
    }
}
