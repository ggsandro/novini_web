using Dapper;
using MySql.Data.MySqlClient;
using Novini.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Novini.Repository
{
    public class ScrapperRepository
    {
        private IDbConnection connection;
        public ScrapperRepository()
        {
            connection = new MySqlConnection(AppSettings.AppSettings.DatabaseConnection);
            connection.Open();
        }

        public void Save()
        {

        }

        public List<ScrapperTemplateModel> GetAll()
        {
            string query = "SELECT * FROM SCRAPPERTEMPLATE";
            return connection.Query<ScrapperTemplateModel>(query).ToList();
        }
    }
}
