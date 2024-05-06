using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
  public class ImagemAddDto

    {
        public int IdUsuario { get; set; }
        public string DescricaoImagem {get; set;}

        [NotMapped]
        public IFormFile ImageFile { get; set; }    

    }
}
