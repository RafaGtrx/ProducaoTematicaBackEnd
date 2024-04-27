using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Service.AuthService
{
    public interface IAuthInterface
    {
        Task<ServiceResponse<UsuarioCriacaoDto>>Registrar(UsuarioCriacaoDto usuarioRegistro);

        Task<ServiceResponse<string>>Login(UsuarioLoginDto usuarioLogin);
    }
}