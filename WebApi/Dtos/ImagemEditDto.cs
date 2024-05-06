using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
  public class ImagemEditDto

    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string DescricaoImagem {get; set;}

    }
}
