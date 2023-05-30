namespace Domain.Entities;

public class Exercise
{
    public int Id { get; set; }
    
    public string Question { get; set; }
    
    public string? Options { get; set; }
    
    public string Answer { get; set; }
}
