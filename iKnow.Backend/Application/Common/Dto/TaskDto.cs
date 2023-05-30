using Domain.Enums;

namespace Application.Common.Dto;

public class TaskDto
{
    public string Type { get; set; }
    
    public ExerciseDto? Exercise { get; set; }
    
    public ConstellationDto? Constellation { get; set; }
}
