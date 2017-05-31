using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace netcoreauth.model
{
	public class UserRepository
	{
		private string connectionString;
		public UserRepository(string conn)
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

		public int Add(User user)
		{
			using (IDbConnection dbConnection = Connection)
			{
                dbConnection.Open();
                int userCount = dbConnection.Query<User>("SELECT * FROM [user]" + " WHERE email = @Email", new { Email = user.Email }).Count();
                if (userCount > 0)
                {
                    return 0;
                }
                else
                {
                    string sQuery = "INSERT INTO [user] (email, password)"
                                    + " VALUES(@Email, @Password);"
                                    + " SELECT CAST(SCOPE_IDENTITY() as int);";
                    var createdId = dbConnection.Query<int>(sQuery, user).Single();
                    return createdId;
                }

            }
		}

		public IEnumerable<User> GetAll()
		{
			using (IDbConnection dbConnection = Connection)
			{
				dbConnection.Open();
				return dbConnection.Query<User>("SELECT * FROM [user]");
			}
		}

		public User GetByID(int id)
		{
			using (IDbConnection dbConnection = Connection)
			{
				string sQuery = "SELECT * FROM [user]"
							   + " WHERE id = @Id";
				dbConnection.Open();
				return dbConnection.Query<User>(sQuery, new { Id = id }).FirstOrDefault();
			}
		}

        public User GetByEmailAndPassword(string email, string password)
        {
			using (IDbConnection dbConnection = Connection)
			{
				string sQuery = "SELECT * FROM [user]"
							   + " WHERE email = @Email and password = @Password";
				dbConnection.Open();
				var currentUser = dbConnection.Query<User>(sQuery, new { Email = email, Password = password });
				if (currentUser.Count() > 0)
				{
                    return currentUser.SingleOrDefault();
				}
				else
				{
                    return null;
				}
			}

		}

        public User GetByEmail(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM [user]"
                               + " WHERE email = @Email";
                dbConnection.Open();
                var user = dbConnection.Query<User>(sQuery, new { Email = email });
                if (user.Count() > 0)
                {
                    return user.SingleOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public bool SetActive(string email, string jwttoken)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE [user] SET is_activated = 1"
                    + " WHERE email = @Email";
                dbConnection.Open();
                var setActive = dbConnection.Execute(sQuery, new { Email = email });
                if (setActive > 0)
                {
                    string dQuery = "DELETE FROM token"
							        + " WHERE jwt_token = @JWT_Token";
					dbConnection.Execute(dQuery, new { JWT_Token = jwttoken });
					return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdatePasswordByEmail(string email, string newpassword, string jwttoken)
        {
			using (IDbConnection dbConnection = Connection)
			{
				string sQuery = "UPDATE [user] SET [password] = @Password"
					+ " WHERE email = @Email";
				dbConnection.Open();
                var setPassword = dbConnection.Execute(sQuery, new { Password = newpassword, Email = email });
				if (setPassword > 0)
				{
					string dQuery = "DELETE FROM token"
									+ " WHERE jwt_token = @JWT_Token";
					dbConnection.Execute(dQuery, new { JWT_Token = jwttoken });
					return true;
				}
				else
				{
					return false;
				}
			}

		}

        public void Delete(int id)
		{
			using (IDbConnection dbConnection = Connection)
			{
				string sQuery = "DELETE FROM [user]"
							 + " WHERE id = @Id";
				dbConnection.Open();
				dbConnection.Execute(sQuery, new { Id = id });
			}
		}

		public void Update(User user)
		{
			using (IDbConnection dbConnection = Connection)
			{
				string sQuery = "UPDATE [user] SET email = @Email, password = @Password"
					+ " WHERE id = @Id";
				dbConnection.Open();
				dbConnection.Query(sQuery, user);
			}
		}
	}
}
