using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
            Enderecos = new HashSet<Endereco>();
            Revisaos = new HashSet<Revisao>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nome")]
        public string? Nome { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("senha")]
        public string? Senha { get; set; }
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}")]
        [Column("dataNascimento", TypeName = "datetime")]
        public DateTime DataNascimento { get; set; }
        [Column("rg")]
        public string? Rg { get; set; }
        [Column("cpf")]
        public string? Cpf { get; set; }
        [Column("cnpj")]
        public string? Cnpj { get; set; }
        [Column("ativo")]
        public bool Ativo { get; set; }

        [InverseProperty("Usuario")]
        public virtual ICollection<Endereco> Enderecos { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<Revisao> Revisaos { get; set; }
    }
}
