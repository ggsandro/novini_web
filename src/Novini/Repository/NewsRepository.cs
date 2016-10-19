using Dapper;
using MySql.Data.MySqlClient;
using Novini.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Novini.Repository
{
    public class NewsRepository
    {
        public NewsRepository()
        {
            var mySqlConnectionBuilder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                UserID = "novini",
                Password = "GGSANDRO",
                Database = "novini",
                ConvertZeroDateTime = true,
                UseAffectedRows = true,
            };
            connection = new MySqlConnection(mySqlConnectionBuilder.ConnectionString);
            connection.Open();
        }

        private IDbConnection connection;

        public IEnumerable<NewsModel> TakeApprovedNews(int skip, int take)
        {
            string query = "SELECT TITLE,CONTENT,URL,TIMESTAMP FROM NEWS WHERE ISAPPROVED = 1 ORDER BY ID DESC LIMIT @take OFFSET @skip";
            return connection.Query<NewsModel>(query, new { skip, take });
        }

        public bool AddNewsItem(NewsModel model)
        {
            string query = "INSERT INTO NEWS(TITLE,CONTENT,ISAPPROVED,URL) VALUES(@Title, @Content, @IsApproved, @Url)";
            connection.Execute(query, new { model.Title, model.Content, model.IsApproved, model.Url });
            return true;
        }

        public IEnumerable<NewsModel> TakeNews(int skip, int take)
        {
            string query = "SELECT ID,TITLE,CONTENT,TIMESTAMP,ISAPPROVED,URL FROM NEWS ORDER BY ID DESC LIMIT @take OFFSET @skip";
            return connection.Query<NewsModel>(query, new { skip, take });
        }

        public bool UpdateNews(IEnumerable<NewsModel> newsList)
        {
            foreach(var news in newsList)
            {
                string query = "UPDATE NEWS SET TITLE = @Title, CONTENT = @Content, ISAPPROVED = @IsApproved, URL = @Url WHERE ID = @Id";
                connection.Execute(query, new { news.Title, news.Content, news.IsApproved, news.Url, news.Id });
            }
            return true;
        }

        //public NewsModel GetNewsItem(int id)
        //{
        //    string query = "Select * NEWS where Id = @id";
        //    return connection.Query<NewsModel>(query, new { id }).SingleOrDefault();
        //}
    }
}
