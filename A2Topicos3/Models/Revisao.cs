using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Table("Revisao")]
    [Index("CarroId", Name = "IX_carro_id")]
    [Index("UsuarioId", Name = "IX_usuario_id")]
    public partial class Revisao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("descricao")]
        public string? Descricao { get; set; }
        [Column("dataRevisao", TypeName = "datetime")]
        public DateTime DataRevisao { get; set; }
        [Column("carro_id")]
        public int? CarroId { get; set; }
        [Column("usuario_id")]
        public int? UsuarioId { get; set; }
        [Column("status")]
        [StringLength(100)]
        public string Status { get; set; } = null!;

        [ForeignKey("CarroId")]
        [InverseProperty("Revisaos")]
        public virtual Carro? Carro { get; set; }
        [ForeignKey("UsuarioId")]
        [InverseProperty("Revisaos")]
        public virtual Usuario? Usuario { get; set; }
    }
}
