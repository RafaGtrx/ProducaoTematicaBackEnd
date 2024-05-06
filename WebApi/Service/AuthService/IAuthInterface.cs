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

        Task<ServiceResponse<LoginResponseDto>>Login(UsuarioLoginDto usuarioLogin);
        
    }
}