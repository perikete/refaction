using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace refactor_me.Tests.Integration
{
    /// <summary>
    /// Base class used for integration test.
    /// </summary>
    [TestClass]
    public abstract class IntegrationTestBase
    {
        private TransactionScope _scope;

        private const string TestConnectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={DataDirectory}\Database.mdf;Integrated Security=True";

        protected IDbConnection GetTestDbConnection()
        {
            return new SqlConnection(TestConnectionString.Replace("{DataDirectory}", GetDbPath()));
        }

        public void BeginTransaction()
        {
            _scope = new TransactionScope();

        }

        public void RollbackTransaction()
        {
            _scope?.Dispose();
        }

        [TestInitialize]
        public void Initialize()
        {
            BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            RollbackTransaction();
        }

        private static string GetDbPath()
        {
            return Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
                "Databases");
        }
    }
}