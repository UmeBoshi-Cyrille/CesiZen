using CesiZen.Domain.Datamodel;
using CesiZen.Infrastructure.DatabaseContext;
using CesiZen.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure;

internal static class DbInitializer
{
    public static void InitiliazeData(CesizenDbContext context)
    {
        context.Database.EnsureCreated();

        AddArticlesAndCategories(context);

        AddUserWithLoginAndExercises(context);
    }

    private static void AddArticlesAndCategories(CesizenDbContext context)
    {
        var articles = ArticleSeeder.FakeGenerator().Generate(200);
        var fakeCategories = CategorySeeder.FakeGenerator().Generate(20);
        var categories = fakeCategories
            .GroupBy(c => c.Name)
            .Select(c => c.First())
            .ToList();

        var contains = context.Articles.Any(a => a.Author == articles[0].Author);

        if (!contains)
        {
            context.Articles.AddRange(articles);
            context.Categories.AddRange(categories);

            context.SaveChanges();

            var articleCategories = ArticleSeeder.FakeArticleCategoryGenerator(articles, categories);
            AddArticleCategories(context, articleCategories);

            context.SaveChanges();
        }
    }


    private static void AddArticleCategories(CesizenDbContext context, List<ArticleCategory> articleCategories)
    {
        foreach (var articleCategory in articleCategories)
        {
            context.Database.ExecuteSqlRaw(
                $"INSERT INTO public.\"ArticleCategory\" (\"ArticlesId\", \"CategoriesId\") VALUES ({articleCategory.ArticlesId}, {articleCategory.CategoriesId})"
            );
        }
    }

    private static void AddUserWithLoginAndExercises(CesizenDbContext context)
    {
        var users = UserSeeder.FakeGenerator().Generate(50);
        var logins = UserSeeder.FakeLoginGenerator().Generate(50);

        for (var i = 0; i < users.Count; i++)
        {
            users[i].Login = logins[i];
        }

        var contains = context.Users.Any(u => u.Username == users[0].Username);

        if (!contains)
        {
            context.Users.AddRange(users);

            context.SaveChanges();
        }
    }
}
