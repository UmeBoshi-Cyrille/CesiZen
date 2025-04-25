namespace CesiZen.Domain.DataTransfertObject;

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Alternative { get; set; } = string.Empty;
}
