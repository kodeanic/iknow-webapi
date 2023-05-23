namespace Application.Common.Dto;

public class ConstellationDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<StarDto> Stars { get; set; }
    
    public List<LineDto> Lines { get; set; }
}