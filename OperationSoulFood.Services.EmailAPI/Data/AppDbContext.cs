using Microsoft.EntityFrameworkCore;
using OperationSoulFood.Services.EmailAPI.Models;

namespace OperationSoulFood.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }      

        public DbSet<EmailLogger> EmailsLoggers { get; set; }

       
    }

}
