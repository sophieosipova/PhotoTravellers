using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsService.Models
{
    public class PostsContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public PostsContext(DbContextOptions<PostsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
