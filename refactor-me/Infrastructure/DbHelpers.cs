using System.Data.SqlClient;
using System.Web;

namespace refactor_me.Models
{
    public class DbHelpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        public static string TestDataDirectory { get; set; }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data")));
        }
    }
}