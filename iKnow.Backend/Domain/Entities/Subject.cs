using Domain.Enums;

namespace Domain.Entities;

public class Subject
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public SubjectList SubjectType { get; set; }
    
    public List<Topic> Topics { get; set; }
}
