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

        public DbSet<UsuarioModel> Tbl_Usuarios { get; set; }
        public DbSet<ImagemModel> Tbl_Imagens { get; set; }
        public DbSet<ComentarioModel> Tbl_Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ComentarioModel>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.IdUsuario_FK)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ComentarioModel>()
                .HasOne(c => c.Imagem)
                .WithMany()
                .HasForeignKey(c => c.IdImagem_FK)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}