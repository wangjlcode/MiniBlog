using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MiniBlog.Model;
using MongoDB.Driver;

namespace MiniBlog.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly IMongoCollection<Article> articleCollection;

        public ArticleRepository(IMongoClient mongoClient) 
        {
            var mongoDatabase = mongoClient.GetDatabase("MiniBlog");

            articleCollection = mongoDatabase.GetCollection<Article>(Article.CollectionName);
        }

        public async Task<List<Article>> GetArticles() =>
            await articleCollection.Find(_ => true).ToListAsync();

        public async Task<Article> CreateArticle(Article article)
        {
            article.Id = null;
            await articleCollection.InsertOneAsync(article);
            return await articleCollection.Find(a => a.Title == article.Title).FirstAsync();
        }

        public async Task<Article> GetArticleById(string id)
        {
            return await articleCollection.Find(x => x.Id == id).FirstAsync();
        }

        public async Task DeleteAllByUserName(string name)
        {
            await articleCollection.DeleteManyAsync(x => x.UserName == name);
        }
    }
}
