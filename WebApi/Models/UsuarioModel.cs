using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public byte[] SenhaHash { get; set; }

        public byte[] SenhaSalt { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now.ToLocalTime();

        [Required]
        public bool Ativo { get; set; } = true;

    }
}