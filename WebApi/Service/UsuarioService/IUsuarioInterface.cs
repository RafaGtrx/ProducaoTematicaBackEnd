using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Service.UsuarioService
{
    public interface IUsuarioInterface
    {
        
        Task<ServiceResponse<List<UsuarioModel>>> GetUsuarios();

        Task<ServiceResponse<UsuarioResponseDto>> GetUsuarioById(int id); 

        Task<ServiceResponse<UsuarioCriacaoDto>>CreateUsuario(UsuarioCriacaoDto usuarioRegistro);

        Task<ServiceResponse<List<UsuarioEditDto>>> UpdateUsuario(UsuarioEditDto editadoUsuario);

        Task<ServiceResponse<List<UsuarioModel>>> DeleteUsuario(int id);
        
    }
}