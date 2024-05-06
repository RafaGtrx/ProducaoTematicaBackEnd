using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Service.ComentarioService;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComentarioController : ControllerBase
    {

        private readonly IComentarioInterface _comentarioService;

        private readonly JsonSerializerOptions _jsonOptions;

        public ComentarioController(IComentarioInterface comentarioService)
        {
            _comentarioService = comentarioService;

            _jsonOptions = new JsonSerializerOptions
            {
                //ReferenceHandler = ReferenceHandler.IgnoreCycles,
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true // Opcional: formatação para facilitar a leitura
            };

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<ComentarioAddDto>>>> AddNewComment(ComentarioAddDto novoComentario)
        {
            return Ok(await _comentarioService.AddNewComment(novoComentario));
        }

        [Authorize]
        [HttpGet("lista-comentarios")]
        public async Task<IActionResult> GetComments()
        {
            ServiceResponse<List<ComentarioPorImagemDto>> serviceResponse = await _comentarioService.GetComments();

            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<List<ComentarioListDto>>>> GetCommenttoById(int id)
        {

            ServiceResponse<List<ComentarioListDto>> serviceResponse = await _comentarioService.GetCommenttoById(id);

            if (!serviceResponse.Sucesso)
            {
                return BadRequest(serviceResponse.Mensagem);
            }

            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpDelete("deleta-comentario")]
        public async Task<ActionResult<ServiceResponse<List<ComentarioModel>>>> DeleteComment(int Id, int IdUsuario)
        {
            ServiceResponse<List<ComentarioModel>> serviceResponse = await _comentarioService.DeleteComment(Id, IdUsuario);

            if (!serviceResponse.Sucesso)
            {
                return BadRequest(serviceResponse);
            }

            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpPut("altera-comentario")]
        public async Task<ActionResult<ServiceResponse<ComentarioEditDto>>> UpdateComment(ComentarioEditDto comentarioEditado)
        {
            ServiceResponse<List<ComentarioEditDto>> serviceResponse = await _comentarioService.UpdateComment(comentarioEditado);

            if (serviceResponse.Sucesso)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse);
            }
        }
    }
}

