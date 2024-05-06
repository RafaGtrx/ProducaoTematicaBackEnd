using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class ComentarioModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdUsuario_FK { get; set; }

        [ForeignKey("IdUsuario_FK")]

        public UsuarioModel Usuario { get; set; } 
        
        [ForeignKey("IdImagem_FK")]
        public ImagemModel Imagem { get; set; }// Propriedade de navegação para UsuarioModel

        [Required]
        public int IdImagem_FK { get; set; }

        [Required]
        public string Comentario { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now.ToLocalTime();

        public DateTime DataAlteracao { get; set; } = DateTime.Now.ToLocalTime();
    }

}