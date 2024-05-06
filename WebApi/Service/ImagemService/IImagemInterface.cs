using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Service.ImagemService
{
    public interface IImagemInterface
    {
        //Task<string> SaveImageAsync(IFormFile imageFile);
        Task<ServiceResponse<List<ImagemAddDto>>> AddNewImage(ImagemAddDto novaImagem);

        Task<ServiceResponse<List<ImagemListDto>>> GetImagesDto();

        Task<ServiceResponse<List<ImagemListDto>>> GetImagesDtoById(int id);

        Task<ServiceResponse<List<ImagemModel>>> DeleteImage(int Id, int IdUsuario);

        Task<ServiceResponse<List<ImagemEditDto>>> UpdateDescriptionImage(ImagemEditDto descricaoEditada);



    }
}