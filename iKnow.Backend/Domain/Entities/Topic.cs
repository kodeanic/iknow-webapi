namespace Domain.Entities;

public class Topic
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public List<Subtopic> Subtopics { get; set; }
}
