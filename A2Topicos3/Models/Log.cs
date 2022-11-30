using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace A2Topicos3.Models
{
    [Table("Log")]
    public partial class Log
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("logDateTime", TypeName = "datetime")]
        public DateTime LogDateTime { get; set; }
        [Column("texto")]
        public string? Texto { get; set; }
    }
}
