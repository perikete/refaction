using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using refactor_me.Controllers;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_me.Infrastructure
{
    /// <summary>
    /// Suuuuuper simple activator class to avoid setting up a DI container for this simple app(overkill)
    /// </summary>
    public class PoorManControllerActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var dbConnection = DbHelpers.GetConnection();
            var productRepository = new ProductRepository(dbConnection);
            if (request.RequestUri.ToString().ToLowerInvariant().Contains("options"))
            {
                return new ProductOptionsController(productRepository);
            }

            return new ProductsController(productRepository);
        }
    }
}