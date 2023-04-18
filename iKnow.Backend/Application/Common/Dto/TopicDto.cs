namespace Application.Common.Dto;

public class TopicDto
{
    public string Title { get; set; }
    
    public int Progress { get; set; }
    
    public List<SubtopicDto> Subtopics { get; set; }
}
