using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CQUCloudWeb.Models;

namespace CQUCloudWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CQUCloudWeb.Models.Algorithm> Algorithm { get; set; }
        public DbSet<CQUCloudWeb.Models.App> App { get; set; }
    }
}
