using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Service.UsuarioService
{
    public interface IUsuarioInterface
    {
        Task<ServiceResponse<List<UsuarioModel>>> GetUsuarios();

        Task<ServiceResponse<List<UsuarioModel>>> CreateUsuario(UsuarioModel novoUsuario);

        Task<ServiceResponse<UsuarioModel>> GetUsuarioById(int id);

        Task<ServiceResponse<List<UsuarioModel>>> UpdateUsuario(UsuarioModel editadoUsuario);

        Task<ServiceResponse<List<UsuarioModel>>> DeleteUsuario(int id);

        Task<ServiceResponse<List<UsuarioModel>>> InativaUsuario(int id);
    }
}