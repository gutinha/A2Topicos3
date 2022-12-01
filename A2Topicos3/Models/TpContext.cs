using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace A2Topicos3.Models
{
    public partial class TpContext : DbContext
    {
        public TpContext()
        {
        }

        public TpContext(DbContextOptions<TpContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Carro> Carros { get; set; } = null!;
        public virtual DbSet<Endereco> Enderecos { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Permissoes> Permissoes { get; set; } = null!;
        public virtual DbSet<Revisao> Revisaos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=Topicos3;user id=sa;password=guta1299!;encrypt=false");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carro>(entity =>
            {
                entity.HasOne(d => d.Marca)
                    .WithMany(p => p.Carros)
                    .HasForeignKey(d => d.MarcaId)
                    .HasConstraintName("FK_dbo.Carro_dbo.Marca_marca_id1");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Enderecos)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_dbo.Endereco_dbo.Usuario_Usuario_id");
            });

            modelBuilder.Entity<Permissoes>(entity =>
            {
                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Permissoes_FK");
            });

            modelBuilder.Entity<Revisao>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValueSql("('Agendada')");

                entity.HasOne(d => d.Carro)
                    .WithMany(p => p.Revisaos)
                    .HasForeignKey(d => d.CarroId)
                    .HasConstraintName("FK_dbo.Revisao_dbo.Carro_carro_id");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Revisaos)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_dbo.Revisao_dbo.Usuario_usuario_id1");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
