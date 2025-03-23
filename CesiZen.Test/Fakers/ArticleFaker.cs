using Bogus;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Test.Fakers;

internal class ArticleFaker
{
    public static Faker<ArticleDto> FakeArticleDtoGenerator()
    {
        return new Faker<ArticleDto>()
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.Author, f => f.Name.FullName())
            .RuleFor(a => a.Description, f => f.Lorem.Paragraph(1))
            .RuleFor(a => a.Content, f => f.Lorem.Paragraph(3));
    }

    public static Faker<Article> FakeArticleGenerator()
    {
        return new Faker<Article>()
            .RuleFor(a => a.Id, f => f.Random.Int(1, 200))
            .RuleFor(a => a.Title, f => f.Name.JobTitle())
            .RuleFor(a => a.Author, f => f.Name.FullName())
            .RuleFor(a => a.Description, f => f.Lorem.Paragraph(1))
            .RuleFor(a => a.Content, f => f.Lorem.Paragraph(3));
    }
}
