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
                string query = "SELECT * FROM NEWS";
                return connection.Query<NewsModel>(query);
            }
        }

        public IEnumerable<NewsModel> TakeApprovedNews()
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "SELECT * FROM NEWS WHERE ISAPPROVED = 'TRUE'";
                return connection.Query<NewsModel>(query);
            }
        }

        public void AddNewsItem(NewsModel model)
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "INSERT INTO NEWS(Title,Content,IsApproved) values(@Title, @Content, @IsApproved)";
                connection.Execute(query, new { model.Title, model.Content, model.IsApproved });
            }
        }

        public void UpdateNewsItem(NewsModel model)
        {
            //using (var sqlConnection = new SqlConnection("Server=(localdb)\v11.0;Integrated Security=true;"))
            using (IDbConnection connection = new SqlConnection(@"Server=DESKTOP-R4SAAK4\SQLEXPRESS;Initial Catalog=Novini;Integrated Security=true;"))
            {
                string query = "UPDATE NEWS SET TITLE = @Title, CONTENT = @Content, ISAPPROVED = @IsApproved) WHERE ID = @Id";
                connection.Execute(query, new { model.Title, model.Content, model.IsApproved, model.Id });
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
