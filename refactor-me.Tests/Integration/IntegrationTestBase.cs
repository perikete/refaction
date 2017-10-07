using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace refactor_me.Tests.Integration
{
    public abstract class IntegrationTestBase
    {
        private const string TestConnectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\TestDatabase.mdf;Integrated Security=True";

        protected IDbConnection GetTestDbConnection()
        {
            return new SqlConnection(TestConnectionString.Replace("{DataDirectory}", GetDbPath()));
        }

        public static void SetUp()
        {
            var dbPath = GetDbPath();

            try  // Super hack to avoid "file already being used..." exceptions when running multiple test fixtures
            {
                if (File.Exists(Path.Combine(dbPath, "TestDatabase.mdf")))
                {
                    File.Delete(Path.Combine(dbPath, "TestDatabase.mdf"));
                    File.Delete(Path.Combine(dbPath, "TestDatabase_log.ldf"));
                }

                File.Copy(Path.Combine(dbPath, "Database.mdf"), Path.Combine(dbPath, "TestDatabase.mdf"));
            }
            catch
            {
                // ignored
            }
        }

        private static string GetDbPath()
        {
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
                "Databases");
        }
    }
}