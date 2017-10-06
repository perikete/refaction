using System.IO;
using refactor_me.Models;

namespace refactor_me.Tests.Integration
{
    public static class TestHelpers
    { 

        public static void SetUp()
        {
            var dbPath = GetDbPath();

            DbHelpers.TestDataDirectory = dbPath; 

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
                
            }
        }

        private static string GetDbPath()
        {
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
                "Databases");
        } 
    }
}
