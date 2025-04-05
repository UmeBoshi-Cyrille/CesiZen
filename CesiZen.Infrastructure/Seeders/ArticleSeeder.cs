using Bogus;
using CesiZen.Domain.Datamodel;

namespace CesiZen.Infrastructure.Seeders;

internal static class ArticleSeeder
{
    internal static Faker<Article> FakeGenerator()
    {
        var result = new Faker<Article>()
            .UseSeed(42)
            .RuleFor(i => i.Id, f => f.IndexFaker + 1)
            .RuleFor(i => i.CreatedAt, f => f.Date.Past(3).ToUniversalTime())
            .RuleFor(i => i.UpdatedAt, f => f.Date.Past(2).ToUniversalTime())
            .RuleFor(i => i.Title, f => f.Lorem.Sentence(f.Random.Int(1, 5)))
            .RuleFor(i => i.Description, f => f.Lorem.Paragraph(f.Random.Int(1, 2)))
            .RuleFor(i => i.Content, f => f.Lorem.Paragraph(f.Random.Int(5, 10)))
            .RuleFor(i => i.Author, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
            .RuleFor(i => i.ImagePath, f => ImageSeeder.GeneratePath(f))
            .RuleFor(i => i.Images, (f, a) =>
            {
                return ImageSeeder.FakeGenerator().Generate(f.Random.Int(1, 5))
                                .Select(img =>
                                {
                                    img.ArticleId = a.Id;
                                    return img;
                                }).ToList();
            });

        return result;
    }

    internal static List<ArticleCategory> FakeArticleCategoryGenerator(List<Article> articles, List<Category> categories)
    {
        var random = new Random();
        var articleCategories = articles
            .SelectMany(article =>
                categories.OrderBy(c => random.Next())
                .Take(random.Next(1, 5))
                .Select(category => new ArticleCategory
                {
                    ArticlesId = article.Id,
                    CategoriesId = category.Id,
                })
            ).ToList();

        return articleCategories;
    }
}
