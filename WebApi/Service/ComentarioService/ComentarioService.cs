using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DataContext;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Service.ComentarioService
{
    public class ComentarioService : IComentarioInterface

    {

        private readonly ApplicationDbContext _context;
        public ComentarioService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<ComentarioAddDto>>> AddNewComment(ComentarioAddDto novoComentario)
        {
            ServiceResponse<List<ComentarioAddDto>> serviceResponse = new ServiceResponse<List<ComentarioAddDto>>();

            try
            {
                var usuario = await _context.Tbl_Usuarios.FindAsync(novoComentario.IdUsuario);
                if (usuario == null)
                {
                    serviceResponse.Mensagem = "Usuário não encontrado";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }

                var imagem = await _context.Tbl_Imagens.FindAsync(novoComentario.IdImagem);
                if (imagem == null)
                {
                    serviceResponse.Mensagem = "Imagem não encontrada";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }

                if (string.IsNullOrEmpty(novoComentario.Comentario))
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Favor informar o comentário";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }

                // Aqui você precisará mapear o DTO para a entidade Model antes de adicionar ao contexto
                var novoComentarioModel = new ComentarioModel
                {
                    IdImagem_FK = novoComentario.IdImagem,
                    IdUsuario_FK = novoComentario.IdUsuario,
                    Comentario = novoComentario.Comentario
                };

                _context.Add(novoComentarioModel);
                await _context.SaveChangesAsync();

                // Aqui você pode retornar uma lista atualizada de DTOs, se necessário
                serviceResponse.Dados = _context.Tbl_Comentarios.ToList().Select(c => new ComentarioAddDto
                {
                    IdImagem = c.IdImagem_FK,
                    IdUsuario = c.IdUsuario_FK,
                    Comentario = c.Comentario
                }).ToList();

                serviceResponse.Mensagem = "Comentário adicionado com sucesso";
                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<ComentarioModel>>> DeleteComment(int Id, int IdUsuario)
        {
            ServiceResponse<List<ComentarioModel>> serviceResponse = new ServiceResponse<List<ComentarioModel>>();

            try
            {
                ComentarioModel comentario = await _context.Tbl_Comentarios.FirstOrDefaultAsync(x => x.Id == Id);

                if (comentario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Comentário não encontrado!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }

                if (comentario.IdUsuario_FK != IdUsuario)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não autorizado! Só é permitido deletar o próprio comentário.";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }

                _context.Tbl_Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();

                List<ComentarioModel> comentarios = await _context.Tbl_Comentarios.ToListAsync();

                serviceResponse.Dados = comentarios;
                serviceResponse.Mensagem = "Comentário deletado com sucesso!";
                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ComentarioPorImagemDto>>> GetComments()
        {
            ServiceResponse<List<ComentarioPorImagemDto>> serviceResponse = new ServiceResponse<List<ComentarioPorImagemDto>>();

            try
            {
                List<ComentarioPorImagemDto> comentariosPorImagem = await _context.Tbl_Comentarios
                    .Include(im => im.Usuario)
                    .GroupBy(im => im.IdImagem_FK)
                    .Select(group => new ComentarioPorImagemDto
                    {
                        IdImagem = group.Key,
                        Comentarios = group.Select(im => new ComentarioListDto
                        {
                            Id = im.Id,
                            IdUsuario = im.IdUsuario_FK,
                            IdImagem = im.IdImagem_FK,
                            Comentario = im.Comentario,
                            DataCriacao = im.DataCriacao,
                            DataAlteracao = im.DataAlteracao,
                            NomeUsuarioComentario = im.Usuario.Nome
                        }).ToList()
                    })
                    .ToListAsync();

                if (comentariosPorImagem.Count == 0)
                {
                    serviceResponse.Mensagem = "Não existem comentários cadastrados no banco de dados!";
                }
                else
                {
                    serviceResponse.Dados = comentariosPorImagem;
                    serviceResponse.Mensagem = "Comentários por imagem localizados com sucesso!";
                }

                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = "Ocorreu um erro ao obter os comentários por imagem: " + ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<ComentarioListDto>>> GetCommenttoById(int id)
        {
            {
                ServiceResponse<List<ComentarioListDto>> serviceResponse = new ServiceResponse<List<ComentarioListDto>>();

                try
                {
                    List<ComentarioListDto> comentarios = await _context.Tbl_Comentarios
                        .Where(x => x.Id == id)
                        .Select(x => new ComentarioListDto
                        {
                            Id = x.Id,
                            IdUsuario = x.IdUsuario_FK,
                            IdImagem = x.IdImagem_FK,
                            Comentario = x.Comentario,
                            DataCriacao = x.DataCriacao,
                            DataAlteracao = x.DataAlteracao,
                            NomeUsuarioComentario = x.Usuario.Nome
                        })
                        .ToListAsync();

                    if (comentarios == null || comentarios.Count == 0)
                    {
                        serviceResponse.Dados = null;
                        serviceResponse.Mensagem = "Nenhum comentário encontrado!";
                        serviceResponse.Sucesso = false;
                    }
                    else
                    {
                        serviceResponse.Mensagem = "Comentários localizados com sucesso!";
                        serviceResponse.Dados = comentarios;
                        serviceResponse.Sucesso = true;
                    }
                }
                catch (Exception ex)
                {
                    serviceResponse.Mensagem = ex.Message;
                    serviceResponse.Sucesso = false;
                }

                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<List<ComentarioEditDto>>> UpdateComment(ComentarioEditDto comentarioEditado)
        {
             ServiceResponse<List<ComentarioEditDto>> serviceResponse = new ServiceResponse<List<ComentarioEditDto>>();

            try
            {
                // Procurar a imagem pelo Id na base de dados
                ComentarioModel comentario = await _context.Tbl_Comentarios.FirstOrDefaultAsync(x => x.Id == comentarioEditado.Id);

                // Verificar se a imagem existe
                if (comentario == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Comentário não encontrado!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }

                // Atualizar a descrição da imagem
                comentario.Comentario= comentarioEditado.Comentario; // Supondo que a propriedade Descricao exista no ImagemModel

                // Atualizar a imagem no contexto
                _context.Tbl_Comentarios.Update(comentario);

                // Salvar as mudanças no banco de dados
                await _context.SaveChangesAsync();

                // Definir a resposta de sucesso

                serviceResponse.Mensagem = "Comentário editado com sucesso!";
                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                // Em caso de exceção, definir a mensagem de erro na resposta
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            // Retornar a resposta
            return serviceResponse;
        }

    }
}
