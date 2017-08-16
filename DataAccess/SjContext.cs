using System.Data.Entity;
using DataModel.Enities;

namespace DataAccess
{
    public class SjContext : DbContext
    {
        public SjContext() : base("SjContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}