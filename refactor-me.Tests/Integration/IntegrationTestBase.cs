using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Infrastructure;

namespace refactor_me.Tests.Integration
{
    /// <summary>
    /// Base class used for integration test.
    /// The tests run inside a transaction that later is rolled back after each test
    /// this makes the DB safe and isolated between tests.
    /// </summary>
    [TestClass]
    public abstract class IntegrationTestBase
    {
        private TransactionScope _scope;

        protected IDbConnection GetTestDbConnection()
        {
            var connStr = RefactorMeConfiguration.ConnectionString.Replace("{DataDirectory}", GetDbPath());
            return new SqlConnection(connStr);
        }

        private void BeginTransaction()
        {
            _scope = new TransactionScope();
        }

        private void RollbackTransaction()
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