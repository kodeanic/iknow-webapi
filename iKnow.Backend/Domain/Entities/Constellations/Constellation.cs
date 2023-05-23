namespace Domain.Entities.Constellations;

public class Constellation
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Star> Stars { get; set; }
    
    public List<Line> Lines { get; set; }
}
