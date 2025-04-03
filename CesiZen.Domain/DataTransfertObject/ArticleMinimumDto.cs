﻿namespace CesiZen.Domain.DataTransfertObject;

public class ArticleMinimumDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
}
