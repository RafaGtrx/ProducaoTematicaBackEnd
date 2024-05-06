using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Dtos
{
    public class ImagemListDto
    {
        
        public int Id { get; set; }
 
        public string ImagePath { get; set; }

        public string DescricaoImagem { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now.ToLocalTime();

        public bool Ativa { get; set; } = true;

        // Chave estrangeira para a tabela de usuários
        public int IdUsuario { get; set; }

        // Propriedade de navegação para o usuário
        
        [ForeignKey("IdUsuario")]
        public UsuarioModel Usuario { get; set; }

         public string NomeUsuario { get; set; } // Propriedade para armazenar o nome do usuário

        public Boolean ExisteComentario {get; set;}

        [NotMapped]
        public IFormFile ImageFile { get; set; }    
    }
}