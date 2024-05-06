using WebApi.Dtos;

public class ComentarioPorImagemDto
{
    public int IdImagem { get; set; }
    public List<ComentarioListDto> Comentarios { get; set; }
}