namespace Domain.Entities.Constellations;

public class Line
{
    public int Id { get; set; }
    
    public int Number { get; set; }
    
    public int StarLeftId { get; set; }
    
    public int StarRightId { get; set; }
}
