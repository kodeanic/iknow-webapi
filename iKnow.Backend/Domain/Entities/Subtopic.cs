namespace Domain.Entities;

public class Subtopic
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public List<Exercise> Exercises { get; set; }
}
