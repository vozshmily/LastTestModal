using Microsoft.EntityFrameworkCore;

namespace LastTestModal.Models
{
    public class TransactionDbContext: DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {
        }

        public DbSet<TransactionModel> Transactions { get; set; }
    }
}
