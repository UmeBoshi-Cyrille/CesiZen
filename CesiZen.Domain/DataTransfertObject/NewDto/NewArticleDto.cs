using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.DataTransfertObject;

public class NewArticleDto
{
    [MaxLength(54)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "Unauthorised characters")]
    [DefaultValue("title")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(54)]
    [MinLength(2)]
    [RegularExpression(@"^[a-zA-Z]+$",
        ErrorMessage = "Unauthorised characters")]
    [DefaultValue("author")]
    public string Author { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    [MaxLength(54)]
    public string ImagePath { get; set; } = string.Empty;
    public ICollection<ImageDto>? Images { get; set; }
    public List<int>? Categories { get; set; }
}
