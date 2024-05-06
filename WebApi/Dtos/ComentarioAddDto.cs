using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Dtos
{
  public class ComentarioAddDto

    {

        public int IdImagem {get; set;}
        public int IdUsuario { get; set; }
        
        public string Comentario  {get; set;}

    }
}
