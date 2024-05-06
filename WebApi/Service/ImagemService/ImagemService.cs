using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.DataContext;
using WebApi.Dtos;
using WebApi.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace WebApi.Service.ImagemService
{
    public class ImagemService : IImagemInterface
    {
        private readonly string _uploadsDirectory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApplicationDbContext _context;

        public ImagemService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
        {
            _uploadsDirectory = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<ServiceResponse<List<ImagemAddDto>>> AddNewImage(ImagemAddDto novaImagem)
        {
            ServiceResponse<List<ImagemAddDto>> serviceResponse = new ServiceResponse<List<ImagemAddDto>>();

            try
            {
                // Verifica se o usuário existe
                var usuario = await _context.Tbl_Usuarios.FindAsync(novaImagem.IdUsuario);
                if (usuario == null)
                {
                    serviceResponse.Mensagem = "Usuário não encontrado";
                    serviceResponse.Sucesso = false;
                    return serviceResponse;
                }

                if (!Directory.Exists(_uploadsDirectory))
                {
                    Directory.CreateDirectory(_uploadsDirectory);
                }

                var nomeArquivo = Path.GetFileNameWithoutExtension(novaImagem.ImageFile.FileName.ToLower());
                var nomeUnicoImagem = nomeArquivo + '-' + Guid.NewGuid().ToString() + Path.GetExtension(novaImagem.ImageFile.FileName);

                if (string.IsNullOrEmpty(Path.GetExtension(novaImagem.ImageFile.FileName)))
                {
                    nomeUnicoImagem += ".jpg";
                }

                var imagePath = Path.Combine(_uploadsDirectory, nomeUnicoImagem);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await novaImagem.ImageFile.CopyToAsync(stream);
                }

                var serverBaseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
                var serverImagePath = Path.Combine(serverBaseUrl, "uploads", nomeUnicoImagem);

                ImagemModel imagemSalva = new ImagemModel
                {
                    ImagePath = serverImagePath,
                    IdUsuario = novaImagem.IdUsuario,
                    DescricaoImagem = novaImagem.DescricaoImagem,
                };

                _context.Tbl_Imagens.Add(imagemSalva);

                await _context.SaveChangesAsync();

                serviceResponse.Mensagem = "Imagem salva com sucesso";
                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = $"Erro ao salvar imagem: {ex.Message}";
                serviceResponse.Sucesso = false;

                // Adicione logs de debugging aqui
                Console.WriteLine($"Erro ao salvar imagem: {ex}");
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<ImagemModel>>> DeleteImage(int Id, int IdUsuario)
        {
            ServiceResponse<List<ImagemModel>> serviceResponse = new ServiceResponse<List<ImagemModel>>();

            try
            {
                ImagemModel imagem = _context.Tbl_Imagens.FirstOrDefault(x => x.Id == Id);

                if (imagem == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Imagem não encontrada!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }

                if (imagem.IdUsuario != IdUsuario)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Usuário não autorizado! Só é permitido deletar imagens que você postou.";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }

                // Remove o arquivo do disco
                if (File.Exists(imagem.ImagePath))
                {
                    File.Delete(imagem.ImagePath);
                }

                _context.Tbl_Imagens.Remove(imagem);
                await _context.SaveChangesAsync();



                List<ImagemModel> imagensAtivas = await _context.Tbl_Imagens.Where(im => im.Ativa == true).ToListAsync();

                serviceResponse.Dados = imagensAtivas;
                serviceResponse.Mensagem = "Imagem deletada com sucesso!";
                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Mensagem = ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<ImagemEditDto>>> UpdateDescriptionImage(ImagemEditDto descricaoEditada)
        {
            ServiceResponse<List<ImagemEditDto>> serviceResponse = new ServiceResponse<List<ImagemEditDto>>();

            try
            {
                // Procurar a imagem pelo Id na base de dados
                ImagemModel imagem = await _context.Tbl_Imagens.FirstOrDefaultAsync(x => x.Id == descricaoEditada.Id);

                // Verificar se a imagem existe
                if (imagem == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Mensagem = "Imagem não encontrada!";
                    serviceResponse.Sucesso = false;

                    return serviceResponse;
                }

                // Atualizar a descrição da imagem
                imagem.DescricaoImagem = descricaoEditada.DescricaoImagem; // Supondo que a propriedade Descricao exista no ImagemModel

                // Atualizar a imagem no contexto
                _context.Tbl_Imagens.Update(imagem);

                // Salvar as mudanças no banco de dados
                await _context.SaveChangesAsync();

                // Após a atualização bem-sucedida, listar todas as imagens novamente (opcional)
                List<ImagemEditDto> imagensAtualizadas = new List<ImagemEditDto>
                {
                    new ImagemEditDto { Id = imagem.Id, DescricaoImagem = imagem.DescricaoImagem }
                };

                // Definir a resposta de sucesso
                serviceResponse.Dados = imagensAtualizadas;
                serviceResponse.Mensagem = "Descrição da imagem atualizada com sucesso!";
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
        public async Task<ServiceResponse<List<ImagemListDto>>> GetImagesDto()
        {
            ServiceResponse<List<ImagemListDto>> serviceResponse = new ServiceResponse<List<ImagemListDto>>();

            try
            {
                // Consulta apenas imagens ativas no banco de dados de forma assíncrona
                List<ImagemListDto> imagens = await _context.Tbl_Imagens
                    .Where(im => im.Ativa)
                    .Include(im => im.Usuario) // Inclui a tabela de usuários
                    .Select(im => new ImagemListDto
                    {
                        Id = im.Id,
                        ImagePath = im.ImagePath,
                        DescricaoImagem = im.DescricaoImagem,
                        DataCriacao = im.DataCriacao,
                        Ativa = im.Ativa,
                        IdUsuario = im.IdUsuario,
                        NomeUsuario = im.Usuario.Nome, // Aqui acessamos o nome do usuário
                        ExisteComentario = _context.Tbl_Comentarios.Any(c => c.IdImagem_FK == im.Id) // Verifica se há algum comentário associado à imagem
                    })
                    .ToListAsync();

                if (imagens.Count == 0)
                {
                    serviceResponse.Mensagem = "Não existem imagens cadastradas no banco de dados!";
                }
                else
                {
                    serviceResponse.Dados = imagens;
                    serviceResponse.Mensagem = "Imagem(s) localizada(s) com sucesso!";
                }

                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                // Em caso de exceção, definir a mensagem de erro na resposta
                serviceResponse.Mensagem = "Ocorreu um erro ao obter as imagens: " + ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<List<ImagemListDto>>> GetImagesDtoById(int id)
        {
            ServiceResponse<List<ImagemListDto>> serviceResponse = new ServiceResponse<List<ImagemListDto>>();

            try
            {
                // Consulta a imagem com o ID especificado de forma assíncrona
                ImagemListDto imagem = await _context.Tbl_Imagens
                    .Where(im => im.Id == id && im.Ativa)
                    .Include(im => im.Usuario) // Inclui a tabela de usuários
                    .Select(im => new ImagemListDto
                    {
                        Id = im.Id,
                        ImagePath = im.ImagePath,
                        DescricaoImagem = im.DescricaoImagem,
                        DataCriacao = im.DataCriacao,
                        Ativa = im.Ativa,
                        IdUsuario = im.IdUsuario,
                        NomeUsuario = im.Usuario.Nome // Aqui acessamos o nome do usuário
                    })
                    .FirstOrDefaultAsync();

                if (imagem == null)
                {
                    serviceResponse.Mensagem = "Imagem não encontrada!";
                }
                else
                {
                    serviceResponse.Dados = new List<ImagemListDto> { imagem };
                    serviceResponse.Mensagem = "Imagem localizada com sucesso!";
                }

                serviceResponse.Sucesso = true;
            }
            catch (Exception ex)
            {
                // Em caso de exceção, definir a mensagem de erro na resposta
                serviceResponse.Mensagem = "Ocorreu um erro ao obter a imagem: " + ex.Message;
                serviceResponse.Sucesso = false;
            }

            return serviceResponse;
        }
    }
}