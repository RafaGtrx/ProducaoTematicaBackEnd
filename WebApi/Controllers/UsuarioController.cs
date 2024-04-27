using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Ok( await _usuarioInterface.GetUsuarios());
        }

        [HttpGet ("{id}")]

        public async Task<ActionResult<ServiceResponse<UsuarioModel>>> GetUsuarioById(int id)
        {
            ServiceResponse<UsuarioModel> serviceResponse = await _usuarioInterface.GetUsuarioById(id);
            return Ok(serviceResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<UsuarioModel>>>> CreateUsuario(UsuarioModel novoUsuario) 
        {
            return Ok( await _usuarioInterface.CreateUsuario(novoUsuario));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<UsuarioModel>>>> UpdateUsuario(UsuarioModel editadoUsuario)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = await _usuarioInterface.UpdateUsuario(editadoUsuario);

            return Ok(serviceResponse);
        }

        [HttpPut("inativaUsuario/{id}")]
        public async Task<ActionResult<ServiceResponse<List<UsuarioModel>>>> InativaFuncionario(int id)
        
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = await _usuarioInterface.InativaUsuario(id);

            return Ok(serviceResponse);
        }
        
        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<List<UsuarioModel>>>> DeleteUsuario(int id)
        {
            ServiceResponse<List<UsuarioModel>> serviceResponse = await _usuarioInterface.DeleteUsuario(id);

            return Ok(serviceResponse);
        }
    }
}