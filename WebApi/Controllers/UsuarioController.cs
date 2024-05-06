using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Service.UsuarioService;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioInterface _usuarioInterface;
        public UsuarioController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }
        [Authorize]
        [HttpGet]

        public async Task<ActionResult<ServiceResponse<List<UsuarioModel>>>> GetUsuarios()
        {
            return Ok(await _usuarioInterface.GetUsuarios());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<UsuarioResponseDto>>> GetUsuarioById(int id)
        {
            ServiceResponse<UsuarioResponseDto> serviceResponse = await _usuarioInterface.GetUsuarioById(id);

            if (serviceResponse.Sucesso)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return NotFound(serviceResponse);
            }
        }
        
        
        [HttpPost]
        [HttpPut("update-usuario")]
        public async Task<ActionResult<ServiceResponse<List<UsuarioEditDto>>>> UpdateUsuario(UsuarioEditDto editadoUsuario)
        {
            ServiceResponse<List<UsuarioEditDto>> serviceResponse = await _usuarioInterface.UpdateUsuario(editadoUsuario);

            return Ok(serviceResponse);
        }

        [Authorize]
        [HttpDelete("delete-usuario")]
        public async Task<ActionResult<ServiceResponse<List<UsuarioModel>>>> DeleteUsuario(int id)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = await _usuarioInterface.DeleteUsuario(id);

            return Ok(serviceResponse);
        }

        [HttpPost("cadastrar-usuario")]
        public async Task<ActionResult>CreateUsuario(UsuarioCriacaoDto usuarioRegister)
        
        {
           var response = await _usuarioInterface.CreateUsuario(usuarioRegister);
            return Ok(response);
        }
   
    }
}