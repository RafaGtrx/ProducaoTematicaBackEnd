using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DataContext;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Service.SenhaService;

namespace WebApi.Service.AuthService
{
    public class AuthService : IAuthInterface
    {

        private readonly ApplicationDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        public AuthService(ApplicationDbContext context, ISenhaInterface senhaInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
        }

      public async Task<ServiceResponse<LoginResponseDto>> Login(UsuarioLoginDto usuarioLogin)
        {
            ServiceResponse<LoginResponseDto> serviceResponse = new ServiceResponse<LoginResponseDto>();

            try
            {
                var usuario = await _context.Tbl_Usuarios.FirstOrDefaultAsync(userBanco => userBanco.Email == usuarioLogin.Email);

                if(usuario == null)
                {
                    serviceResponse.Mensagem = "Credenciais Inválidas!";
                    serviceResponse.Sucesso = false;
                    
                    return serviceResponse;
                }

                if(!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    serviceResponse.Mensagem = "Credenciais Inválidas!";
                    serviceResponse.Sucesso = false;
                    
                    return serviceResponse;
                }

                var token = _senhaInterface.CriarToken(usuario);

                LoginResponseDto loginResponse = new LoginResponseDto
                {
 
                    Token = token
                };

                serviceResponse.Dados = loginResponse;
                serviceResponse.Mensagem = "Usuário Logado Com Sucesso!";

            } catch(Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

    }
}