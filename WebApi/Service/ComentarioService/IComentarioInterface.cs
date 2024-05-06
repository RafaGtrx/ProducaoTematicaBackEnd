using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Service.ComentarioService
{
    public interface IComentarioInterface
    {
        Task<ServiceResponse<List<ComentarioAddDto>>> AddNewComment(ComentarioAddDto novoComentario);

        Task<ServiceResponse<List<ComentarioPorImagemDto>>> GetComments();

        Task<ServiceResponse<List<ComentarioListDto>>> GetCommenttoById(int id);

        Task<ServiceResponse<List<ComentarioModel>>> DeleteComment(int Id, int IdUsuario);

        Task<ServiceResponse<List<ComentarioEditDto>>> UpdateComment(ComentarioEditDto comentarioEditado);
    }
}