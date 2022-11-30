using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Keyless]
    public partial class Permissoes
    {
        [Column("permissao")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Permissao { get; set; }
        [Column("id_usuario")]
        public int? IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
