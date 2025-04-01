using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class ImageDto
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Alternative { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Path { get; set; } = string.Empty;
}
