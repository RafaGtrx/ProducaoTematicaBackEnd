using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class UsuarioEditDto
    {
        [Required]
        public int Id {get; set;}
        
        [Required]
        public string Nome {get; set;}

        [Required]

        [EmailAddress]
        public string Email { get; set; }

        public string Senha { get; set; }

        [Required]    
        public bool AtualizarSenha { get; set; }

    }
}