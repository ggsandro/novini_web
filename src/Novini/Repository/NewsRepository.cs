﻿using Dapper;
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
        private IDbConnection connection = new SqlConnection(AppSettings.AppSettings.DatabaseConnection);

        //public IEnumerable<NewsModel> TakeNews()
        //{
        //    string query = "SELECT ID,TITLE,CONTENT,TIMESTAMP,ISAPPROVED,URL FROM NEWS ORDER BY ID DESC";
        //    return connection.Query<NewsModel>(query);
        //}

        public IEnumerable<NewsModel> TakeApprovedNews(int skip, int take)
        {
            string query = "SELECT TITLE,CONTENT,URL, TIMESTAMP FROM NEWS WHERE ISAPPROVED = 'TRUE' ORDER BY ID DESC OFFSET(@skip) ROWS FETCH NEXT(@take) ROWS ONLY";
            return connection.Query<NewsModel>(query, new { skip, take });
        }

        public void AddNewsItem(NewsModel model)
        {
            string query = "INSERT INTO NEWS(TITLE,CONTENT,ISAPPROVED,URL) VALUES(@Title, @Content, @IsApproved, @Url)";
            connection.Execute(query, new { model.Title, model.Content, model.IsApproved, model.Url });
        }

        //public void UpdateNewsItem(NewsModel model)
        //{
        //    string query = "UPDATE NEWS SET TITLE = @Title, CONTENT = @Content, ISAPPROVED = @IsApproved, URL = @Url WHERE ID = @Id";
        //    connection.Execute(query, new { model.Title, model.Content, model.IsApproved, model.Url, model.Id });
        //}

        //public NewsModel GetNewsItem(int id)
        //{
        //    string query = "Select * NEWS where Id = @id";
        //    return connection.Query<NewsModel>(query, new { id }).SingleOrDefault();
        //}
    }
}
