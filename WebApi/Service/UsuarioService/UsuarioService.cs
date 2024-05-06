using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DataContext;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Service.SenhaService;


namespace WebApi.Service.UsuarioService
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly ApplicationDbContext _context;

        private readonly ISenhaInterface _senhaInterface;
        public UsuarioService(ApplicationDbContext context, ISenhaInterface senhaInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
        }


        public async Task<ServiceResponse<List<UsuarioModel>>> DeleteUsuario(int id)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = new ServiceResponse<List<UsuarioModel>>();

            try
            {

                UsuarioModel usuario = _context.Tbl_Usuarios.FirstOrDefault(x => x.Id == id);

                if (usuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não encontrado!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;

                }

                _context.Tbl_Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = null;
                serviceResponse.Mensagem = "Usuário Deletado com Sucesso!";

            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UsuarioResponseDto>> GetUsuarioById(int id)

        {
            ServiceResponse<UsuarioResponseDto> serviceResponse = new ServiceResponse<UsuarioResponseDto>();

            try
            {
                UsuarioModel usuarioModel = await _context.Tbl_Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                if (usuarioModel == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;
                }
                else
                {
                    UsuarioResponseDto usuarioDto = new UsuarioResponseDto
                    {
                        Email = usuarioModel.Email,
                        Nome = usuarioModel.Nome,
                        Ativo = usuarioModel.Ativo,
                        DataCriacao = usuarioModel.DataCriacao
                        // Adicione outras propriedades conforme necessário
                    };

                    serviceResponse.Mensagem = "Usuário localizado com sucesso!";
                    serviceResponse.Dados = usuarioDto;
                    serviceResponse.Sucesso = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = "Erro ao buscar usuário: " + ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UsuarioEditDto>>> UpdateUsuario(UsuarioEditDto editadoUsuario)
        {
            ServiceResponse<List<UsuarioEditDto>> serviceResponse = new ServiceResponse<List<UsuarioEditDto>>();

            try
            {
                UsuarioModel usuario = await _context.Tbl_Usuarios.FindAsync(editadoUsuario.Id);

                if (usuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                    serviceResponse.Mensagem = "Usuário não encontrado.";

                    return serviceResponse;
                }

                if (!VerificaTrocaEmail(editadoUsuario.Email, usuario.Id))
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Sucesso = false;
                    serviceResponse.Mensagem = "E-mail já cadastrado para outro usuário.";

                    return serviceResponse;
                }

                if (!editadoUsuario.AtualizarSenha)
                {
                    // Se não precisar atualizar a senha, atualiza os dados do usuário existente
                    usuario.Nome = editadoUsuario.Nome;
                    usuario.Email = editadoUsuario.Email;
                }
                else
                {
                    // Se precisar atualizar a senha, atualiza os dados do usuário e a senha
                    usuario.Nome = editadoUsuario.Nome;
                    usuario.Email = editadoUsuario.Email;

                    if (!string.IsNullOrEmpty(editadoUsuario.Senha))
                    {
                        _senhaInterface.CriarSenhaHash(editadoUsuario.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                        usuario.SenhaHash = senhaHash;
                        usuario.SenhaSalt = senhaSalt;
                    }
                }

                _context.Tbl_Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                serviceResponse.Mensagem = "Dado(s) atualizado(s) com sucesso!";
                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Dados = null;
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<UsuarioModel>>> GetUsuarios()
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = new ServiceResponse<List<UsuarioModel>>();

            try
            {
                serviceResponse.Dados = _context.Tbl_Usuarios.ToList();
                if (serviceResponse.Dados.Count == 0)
                {
                    serviceResponse.Mensagem = "Não existe usuário cadastrado no banco de dados!";
                }
                serviceResponse.Mensagem = "Usuário(s) Localizado(s) com Sucesso!";
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UsuarioCriacaoDto>> CreateUsuario(UsuarioCriacaoDto usuarioRegistro)

        {
            ServiceResponse<UsuarioCriacaoDto> serviceResponse = new ServiceResponse<UsuarioCriacaoDto>();

            try
            {
                if (!VerificaEmailJaExiste(usuarioRegistro))
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

                serviceResponse.Mensagem = "Usuário cadastrado com sucesso!";



            }
            catch (Exception ex)
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

            if (usuario != null) return false;

            return true;
        }

        public bool VerificaTrocaEmail(string email, int idUsuario)
        {
            var usuario = _context.Tbl_Usuarios.FirstOrDefault(userBanco => userBanco.Email == email && userBanco.Id != idUsuario);

            return usuario == null;
        }

    }
}