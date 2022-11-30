using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Table("Endereco")]
    [Index("UsuarioId", Name = "IX_Usuario_id")]
    public partial class Endereco
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("endereco")]
        public string? Endereco1 { get; set; }
        [Column("numero")]
        public int Numero { get; set; }
        [Column("complemento")]
        public string? Complemento { get; set; }
        [Column("bairro")]
        public string? Bairro { get; set; }
        [Column("cidade")]
        public string? Cidade { get; set; }
        [Column("estado")]
        public string? Estado { get; set; }
        [Column("cep")]
        public string? Cep { get; set; }
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        [InverseProperty("Enderecos")]
        public virtual Usuario? Usuario { get; set; }
    }
}
