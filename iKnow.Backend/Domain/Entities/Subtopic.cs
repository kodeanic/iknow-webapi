using Domain.Entities.Constellations;
using Domain.Enums;

namespace Domain.Entities;

public class Subtopic
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public TaskType Type { get; set; }
    
    public List<Exercise>? Exercises { get; set; }
    
    public List<Constellation>? Constellations { get; set; }
    
    public string? Theory { get; set; }
}
