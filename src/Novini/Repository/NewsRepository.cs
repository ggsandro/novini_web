using Dapper;
using Novini.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Novini.Repository
{
    public class NewsRepository
    {
        public IEnumerable<NewsModel> TakeNews()
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "SELECT ID,TITLE,CONTENT,TIMESTAMP,ISAPPROVED,URL FROM NEWS";
                return connection.Query<NewsModel>(query);
            }
        }

        public IEnumerable<NewsModel> TakeApprovedNews()
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "SELECT TITLE,CONTENT,URL FROM NEWS WHERE ISAPPROVED = 'TRUE' ORDER BY ID DESC";
                return connection.Query<NewsModel>(query);
            }
        }

        public void AddNewsItem(NewsModel model)
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "INSERT INTO NEWS(TITLE,CONTENT,ISAPPROVED,URL) VALUES(@Title, @Content, @IsApproved, @Url)";
                connection.Execute(query, new { model.Title, model.Content, model.IsApproved, model.Url });
            }
        }

        public void UpdateNewsItem(NewsModel model)
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "UPDATE NEWS SET TITLE = @Title, CONTENT = @Content, ISAPPROVED = @IsApproved, URL = @Url WHERE ID = @Id";
                connection.Execute(query, new { model.Title, model.Content, model.IsApproved, model.Url, model.Id });
            }
        }

        public NewsModel GetNewsItem(int id)
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "Select * NEWS where Id = @id";
                return connection.Query<NewsModel>(query, new { id}).SingleOrDefault();
            }
        }
    }
}
