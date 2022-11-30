using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Table("Carro")]
    [Index("MarcaId", Name = "IX_marca_id")]
    public partial class Carro
    {
        public Carro()
        {
            Revisaos = new HashSet<Revisao>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        public string? Nome { get; set; }
        [Column("modelo")]
        public string? Modelo { get; set; }
        [Column("anoModelo", TypeName = "datetime")]
        public DateTime AnoModelo { get; set; }
        [Column("anoFabricacao", TypeName = "datetime")]
        public DateTime AnoFabricacao { get; set; }
        [Column("cavalos")]
        public int Cavalos { get; set; }
        [Column("litrosMotor")]
        public int LitrosMotor { get; set; }
        [Column("quantPortas")]
        public int QuantPortas { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("marca_id")]
        public int? MarcaId { get; set; }
        [Column("imagem")]
        public string? Imagem { get; set; }

        [ForeignKey("MarcaId")]
        [InverseProperty("Carros")]
        public virtual Marca? Marca { get; set; }
        [InverseProperty("Carro")]
        public virtual ICollection<Revisao> Revisaos { get; set; }
    }
}
