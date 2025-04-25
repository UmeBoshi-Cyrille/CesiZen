using System.ComponentModel.DataAnnotations;

namespace CesiZen.Domain.Datamodel;
public class Category
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(54)]
    public string ImagePath { get; set; } = string.Empty;

    public string Alternative { get; set; } = string.Empty;
    public ICollection<Article>? Articles { get; set; }
}
