using Microsoft.EntityFrameworkCore;
using Session1.Api.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Dal
{
    public class AnimalsDbContext : DbContext
    {

        public AnimalsDbContext()
        {
        }

        public AnimalsDbContext(DbContextOptions<AnimalsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Animal> Animal { get; set; }
    }
}
