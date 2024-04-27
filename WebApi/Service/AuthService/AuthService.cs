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

        public async Task<ServiceResponse<UsuarioCriacaoDto>>Registrar(UsuarioCriacaoDto usuarioRegistro)
        
        {
            ServiceResponse<UsuarioCriacaoDto> serviceResponse = new ServiceResponse<UsuarioCriacaoDto>();

            try
            {
                if(!VerificaEmailJaExiste(usuarioRegistro))
                {
                    //ja existe
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                    serviceResponse.Mensagem = "Email já cadastrado!";

                    return serviceResponse;
                }

                _senhaInterface.CriarSenhaHash(usuarioRegistro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                UsuarioModel usuario = new UsuarioModel
                {
                    Nome = usuarioRegistro.Nome,
                    Email = usuarioRegistro.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt,

                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                serviceResponse.Mensagem = "Email cadastrado com sucesso!";



            }catch(Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>>Login(UsuarioLoginDto usuarioLogin)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

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

                serviceResponse.Dados = token;
                serviceResponse.Mensagem = "Usuário Logado Com Sucesso!";

            }catch(Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
        public bool VerificaEmailJaExiste(UsuarioCriacaoDto usuarioRegistro)
        {
            var usuario = _context.Tbl_Usuarios.FirstOrDefault(userBanco => userBanco.Email == usuarioRegistro.Email);

            if(usuario !=null) return false;

            return true;
        }
    }
}