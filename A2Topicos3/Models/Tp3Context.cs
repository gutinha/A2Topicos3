using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace A2Topicos3.Models
{
    public partial class Tp3Context : DbContext
    {
        public Tp3Context()
        {
        }

        public Tp3Context(DbContextOptions<Tp3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Carro> Carros { get; set; } = null!;
        public virtual DbSet<Endereco> Enderecos { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Permisso> Permissoes { get; set; } = null!;
        public virtual DbSet<Revisao> Revisaos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=Topicos3;Persist Security Info=True;User ID=sa;Password=guta1299!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carro>(entity =>
            {
                entity.HasIndex(e => e.MarcaId, "IX_marca_id")
                    .HasFillFactor(100);

                entity.HasOne(d => d.Marca)
                    .WithMany(p => p.Carros)
                    .HasForeignKey(d => d.MarcaId)
                    .HasConstraintName("FK_dbo.Carro_dbo.Marca_marca_id1");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasIndex(e => e.UsuarioId, "IX_Usuario_id")
                    .HasFillFactor(100);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Enderecos)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_dbo.Endereco_dbo.Usuario_Usuario_id");
            });

            modelBuilder.Entity<Permisso>(entity =>
            {
                entity.HasIndex(e => e.IdUsuario, "IX_Permissoes_id_usuario")
                    .HasFillFactor(100);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Permissos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Permissoes_FK");
            });

            modelBuilder.Entity<Revisao>(entity =>
            {
                entity.HasIndex(e => e.CarroId, "IX_carro_id")
                    .HasFillFactor(100);

                entity.HasIndex(e => e.UsuarioId, "IX_usuario_id")
                    .HasFillFactor(100);

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
