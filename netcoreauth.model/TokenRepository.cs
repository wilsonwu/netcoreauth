using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace netcoreauth.model
{
    public class TokenRepository
    {
        private string connectionString;
        public TokenRepository(string conn)
        {
            connectionString = conn;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public void Add(Token token)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO token (user_id, token_type, jwt_token)"
                                + " VALUES(@User_Id, @Token_Type, @JWT_Token)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, token);
            }
        }

        public void AddForUser(string email, string tokentype, string jwttoken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                string gQuery = "SELECT * FROM [user]"
                                + " WHERE email = @Email";
                var users = dbConnection.Query<User>(gQuery, new { Email = email });
                if (users.Any())
                {
                    int userid = users.SingleOrDefault().Id;
                    string sQuery = "SELECT * FROM token"
                                    + " WHERE user_id = @User_Id AND token_type = @Token_Type";
                    var tokens = dbConnection.Query<Token>(sQuery, new { User_Id = userid, Token_Type = tokentype });
                    if (tokens.Any())
                    {
                        string uQuery = "UPDATE token SET jwt_token = @JWT_Token"
                                        + " WHERE id = @Id";
                        dbConnection.Execute(uQuery, new { Id = tokens.Single().Id, JWT_Token = jwttoken });
                    }
                    else
                    {
                        string iQuery = "INSERT INTO token (user_id, token_type, jwt_token)"
                                        + " VALUES(@User_Id, @Token_Type, @JWT_Token)";
                        dbConnection.Execute(iQuery, new Token() { User_Id = userid, Token_Type = tokentype, JWT_Token = jwttoken });
                    }
                }
            }

        }

        public IEnumerable<Token> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Token>("SELECT * FROM token");
            }
        }

        public Token GetByID(long id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM token"
                               + " WHERE id = @Id";
                dbConnection.Open();
                return dbConnection.Query<Token>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public Token GetByTypeAndToken(string tokentype, string jwttoken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM [token]"
                    + " WHERE token_type = @Token_Type AND jwt_token = @JWT_Token;";
                dbConnection.Open();
                var getToken = dbConnection.Query<Token>(sQuery, new { Token_Type = tokentype, JWT_Token = jwttoken });
                if (getToken.Any())
                {
                    return getToken.SingleOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM token"
                             + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public int DeleteByToken(string jwttoken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM token"
                             + " WHERE jwt_token = @JWT_Token";
                dbConnection.Open();
                return dbConnection.Execute(sQuery, new { JWT_Token = jwttoken });
            }
        }


        public void Update(Token token)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE token SET user_id = @User_Id, token_type = @Token_Type, jwt_token = @JWT_Token"
                    + " WHERE id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, token);
            }
        }
    }
}
