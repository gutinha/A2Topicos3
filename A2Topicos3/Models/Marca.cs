using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Table("Marca")]
    public partial class Marca
    {
        public Marca()
        {
            Carros = new HashSet<Carro>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        public string? Nome { get; set; }
        [Column("descricao")]
        public string? Descricao { get; set; }
        [Column("imagem")]
        public string? Imagem { get; set; }

        [InverseProperty("Marca")]
        public virtual ICollection<Carro> Carros { get; set; }
    }
}
