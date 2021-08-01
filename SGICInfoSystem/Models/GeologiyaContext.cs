using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SGICInfoSystem.Models
{
    public class GeologiyaContext : DbContext
    {
        public GeologiyaContext()
        { }
        public GeologiyaContext(string connection):base(connection)
        {
            
        }
        public DbSet<Essay> Essays { get; set; }
        public DbSet<UzEssay> UzEssays { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Duplicate> Duplicates { get; set; }
        public DbSet<UzDuplicate> UzDuplicates { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}