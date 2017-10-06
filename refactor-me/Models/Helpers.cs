using System.Data.SqlClient;
using System.Web;

namespace refactor_me.Models
{
    public class Helpers
    {
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";
        private const string TestConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\TestDatabase.mdf;Integrated Security=True";

        public static string TestDataDirectory { get; set; }

        public static SqlConnection NewConnection()
        {
            return !string.IsNullOrEmpty(TestDataDirectory) ? new SqlConnection(TestConnectionString.Replace("{DataDirectory}", TestDataDirectory)) : new SqlConnection(ConnectionString.Replace("{DataDirectory}", HttpContext.Current.Server.MapPath("~/App_Data")));
        }
    }
}