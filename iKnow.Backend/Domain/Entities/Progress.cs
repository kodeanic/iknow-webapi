using Domain.Enums;

namespace Domain.Entities;

public class Progress
{
    public int Id { get; set; }
    
    public User User { get; set; }
    
    public Subtopic Topic { get; set; }
    
    public int Number { get; set; }

    public TopicState State { get; set; } = TopicState.IsLocked;
}
