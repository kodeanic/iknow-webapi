namespace Domain.Entities;

public class Progress
{
    public int Id { get; set; }
    
    public User User { get; set; }
    
    public Subtopic Subtopic { get; set; }
    
    public int CompletedExercises { get; set; }

    public bool IsOpen { get; set; } = false;
}
