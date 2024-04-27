using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ComentarioModel
    {
         [Key]
        public int Id { get; set; }

        public int IdUsuario {get; set;}

        public int IdFoto {get;set;}

        [Required]
        public string Texto { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now.ToLocalTime();

        public DateTime? DataModificacao { get; set; } = DateTime.Now.ToLocalTime();

        [Required]
        public bool Excluido { get; set; } = false;

    }
}