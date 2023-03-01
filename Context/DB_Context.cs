using System;
using JWT_ASP_WEBAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_ASP_WEBAPI.Context
{
  public class DB_Context : DbContext
  {
    public DB_Context(DbContextOptions<DB_Context> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
  }
}
