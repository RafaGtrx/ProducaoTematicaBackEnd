using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DataContext;
using WebApi.Models;

namespace WebApi.Service.UsuarioService
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly ApplicationDbContext _context;
        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<UsuarioModel>>> CreateUsuario(UsuarioModel novoUsuario)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = new ServiceResponse<List<UsuarioModel>>();

            try
            {

                if(novoUsuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Favor Informar os dados do usuário";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;

                }

                _context.Add(novoUsuario);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Tbl_Usuarios.ToList();
                

            }catch(Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UsuarioModel>>> DeleteUsuario(int id)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = new ServiceResponse<List<UsuarioModel>>();
          
            try
            {

                UsuarioModel usuario = _context.Tbl_Usuarios.FirstOrDefault(x => x.Id == id);

                if(usuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não encontrado!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;

                }
   
                _context.Tbl_Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Tbl_Usuarios.ToList();
                serviceResponse.Mensagem = "Usuário Deletado com Sucesso!";
  
            }catch(Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UsuarioModel>> GetUsuarioById(int id)
        {
            ServiceResponse<UsuarioModel> serviceResponse = new ServiceResponse<UsuarioModel>();

            try
            {
                UsuarioModel usuario = _context.Tbl_Usuarios.FirstOrDefault(x => x.Id == id);

                if(usuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;

                }
                serviceResponse.Mensagem = "Usuário Localizado com Sucesso!";
                serviceResponse.Dados = usuario;
            }catch(Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UsuarioModel>>> InativaUsuario(int id)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = new ServiceResponse<List<UsuarioModel>>();

            try
            {
                UsuarioModel usuario = _context.Tbl_Usuarios.FirstOrDefault(x => x.Id == id);

                if(usuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário Não Localizado!";
                    serviceResponse.Sucesso = false;
                }

                usuario.Ativo = false;
                serviceResponse.Mensagem = "Usuário Inativado com sucesso!";

                _context.Tbl_Usuarios.Update(usuario);
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Tbl_Usuarios.ToList();

            }catch(Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UsuarioModel>>> UpdateUsuario(UsuarioModel editadoUsuario)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = new ServiceResponse<List<UsuarioModel>>();

            try
            {
                UsuarioModel usuario = _context.Tbl_Usuarios.AsNoTracking().FirstOrDefault(x => x.Id == editadoUsuario.Id);
                
                if(usuario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não localizado!";
                    serviceResponse.Sucesso = false;

                }

                _context.Tbl_Usuarios.Update(editadoUsuario);
                  
                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Tbl_Usuarios.ToList();
                serviceResponse.Mensagem = "Dados alterados com sucesso!";


            }catch(Exception ex)
            {
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
                if(serviceResponse.Dados.Count ==0)
                {
                    serviceResponse.Mensagem = "Não existe usuário cadastrado no banco de dados!";
                }
                serviceResponse.Mensagem = "Usuário(s) Localizado(s) com Sucesso!";
            }catch(Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }


    }
}