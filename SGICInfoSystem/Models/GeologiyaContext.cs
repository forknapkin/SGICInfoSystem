using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SGICInfoSystem.Models
{
    public class GeologiyaContext : DbContext
    {
        public DbSet<Essay> Essays { get; set; }
        public DbSet<User> Users { get; set; }
    }
}