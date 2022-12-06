using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Index("IdUsuario", Name = "IX_Permissoes_id_usuario")]
    public partial class Permisso
    {
        [Column("permissao")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Permissao { get; set; }
        [Column("id_usuario")]
        public int? IdUsuario { get; set; }
        [Key]
        public int Id { get; set; }

        [ForeignKey("IdUsuario")]
        [InverseProperty("Permissos")]
        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
