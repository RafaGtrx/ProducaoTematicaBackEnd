using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FotoModel
    {
        [Key]
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public string TituloFoto { get; set; }

        public string CaminhoFoto { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now.ToLocalTime();

        public DateTime? DataModificacao { get; set; } = DateTime.Now.ToLocalTime();

        public bool Excluida {get;set;} = false;

    
    }
}