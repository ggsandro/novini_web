using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MySql.Data.MySqlClient;
using Novini.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Novini.Repository
{
    public class UserRepository
    {

        public UserRepository()
        {
            connection = new MySqlConnection(AppSettings.AppSettings.DatabaseConnection);
            connection.Open();
        }

        private IDbConnection connection;// = new MySqlConnection(AppSettings.AppSettings.DatabaseConnection);

        public bool CheckLoginPassword(string login, string password)
        {
            string query = "SELECT LOGIN, PASSWORDHASH, SALT FROM USERS WHERE LOGIN = @login";
            var user = connection.Query<UserModel>(query, new { login }).FirstOrDefault();
            if (user == null)
                return false;
            return user.PasswordHash == HashPass(password, user.Salt);
        }

        private string HashPass(string password, byte[] salt = null)
        {
            if (salt == null)
            {
                // generate a 128-bit salt using a secure PRNG
                salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
