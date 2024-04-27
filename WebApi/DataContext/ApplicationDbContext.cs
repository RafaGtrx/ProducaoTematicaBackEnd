using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DataContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UsuarioModel> Tbl_Usuarios {get;set;}
        
        public DbSet<FotoModel> Tbl_Fotos {get;set;}

        public DbSet<ComentarioModel> Tbl_Comentarios {get;set;}
    }
}