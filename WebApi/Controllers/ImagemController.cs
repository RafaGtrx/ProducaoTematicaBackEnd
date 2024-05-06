
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Service.ImagemService;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagemController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonOptions;

        private readonly IImagemInterface _imageService;

        public ImagemController(IImagemInterface imagemService)
        {
            _imageService = imagemService;

            _jsonOptions = new JsonSerializerOptions
            {
                //ReferenceHandler = ReferenceHandler.IgnoreCycles,
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // Opcional: formatação para facilitar a leitura
            };
        }
        [Authorize]
        [HttpPost("upload")]
        public async Task<ActionResult<ServiceResponse<List<ImagemAddDto>>>> AddNewImage([FromForm] ImagemAddDto request)
        {
            try
            {
                if (request == null || request.ImageFile == null || request.ImageFile.Length == 0)
                {
                    return BadRequest("No image uploaded");
                }

                // Chama o serviço para salvar a imagem
                var serviceResponse = await _imageService.AddNewImage(request);

                // Verifica se a operação foi bem-sucedida
                if (serviceResponse.Sucesso)
                {
                    // Retorna a resposta do serviço com os dados da imagem salva
                    return Ok(serviceResponse);
                }
                else
                {
                    // Se houve um erro ao salvar a imagem, retorna uma resposta de erro
                    return StatusCode(500, serviceResponse);
                }
            }
            catch (Exception ex)
            {
                // Em caso de exceção não tratada, retorna uma resposta de erro interno do servidor
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Authorize]
        [HttpDelete("deletaImagem")]
        public async Task<ActionResult<ServiceResponse<List<ImagemModel>>>> DeleteImage(int Id, int IdUsuario)
        {
            ServiceResponse<List<ImagemModel>> serviceResponse = await _imageService.DeleteImage(Id, IdUsuario);

            if (!serviceResponse.Sucesso)
            {
                return BadRequest(serviceResponse);
            }

            return Ok(serviceResponse);
        }
        [Authorize]
        [HttpPut("editaDescricao")]
        public async Task<IActionResult> UpdateDescriptionImage([FromBody] ImagemEditDto descricaoEditada)
        {
            ServiceResponse<List<ImagemEditDto>> serviceResponse = await _imageService.UpdateDescriptionImage(descricaoEditada);

            if (serviceResponse.Sucesso)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse);
            }
        }

        [Authorize]
        [HttpGet("listaImagem")]
        public async Task<IActionResult> GetImagesDto()
        {
            ServiceResponse<List<ImagemListDto>> serviceResponse = await _imageService.GetImagesDto();

            if (serviceResponse.Sucesso)
            {
                var jsonString = JsonSerializer.Serialize(serviceResponse, _jsonOptions);
                return Ok(jsonString);
            }
            else
            {
                var jsonString = JsonSerializer.Serialize(serviceResponse, _jsonOptions);
                return StatusCode(500, jsonString);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            ServiceResponse<List<ImagemListDto>> serviceResponse = await _imageService.GetImagesDtoById(id);
            if (!serviceResponse.Sucesso)
            {
                return NotFound(serviceResponse);
            }

            return Ok(serviceResponse);
        }

    }

}


