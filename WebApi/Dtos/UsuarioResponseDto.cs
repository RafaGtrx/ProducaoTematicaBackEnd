
using Microsoft.Net.Http.Headers;

namespace WebApi.Dtos
{
    public class UsuarioResponseDto
    {
        
        public string Email { get; set; }

        public string Nome { get; set; }

        public DateTime DataCriacao { get; set; }

        public bool Ativo { get; set; } = true;
 
    }
}