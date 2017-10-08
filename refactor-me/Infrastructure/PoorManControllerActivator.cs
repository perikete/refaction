using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using refactor_me.Controllers;
using refactor_me.Repositories;

namespace refactor_me.Infrastructure
{
    /// <summary>
    /// Suuuuuper simple activator class to "simulate" a DI container.
    /// Ideally this should be using some DI framework like autofac, ninject, etc but for this simple 
    /// app would be overkill
    /// </summary>
    public class PoorManControllerActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var dbConnection = GetDbConnection();
            var productRepository = GetProductRepository(dbConnection);
            return GetController(request, productRepository);
        } 

        private static IHttpController GetController(HttpRequestMessage request, ProductRepository productRepository)
        {
            if (request.RequestUri.ToString().ToLowerInvariant().Contains("options"))
            {
                return new ProductOptionsController(productRepository);
            }

            return new ProductsController(productRepository);
        }

        private static ProductRepository GetProductRepository(IDbConnection dbConnection)
        {
            return new ProductRepository(dbConnection);
        }

        private static SqlConnection GetDbConnection()
        { 
            var dbConnection = new SqlConnection(RefactorMeConfiguration.ConnectionString);
            return dbConnection;
        }
    }
}