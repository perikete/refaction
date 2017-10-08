using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly ProductRepository _productRepository;

        public ProductsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [Route]
        [HttpGet]
        public Products GetAll()
        {
            return _productRepository.GetAll();
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            return _productRepository.GetByName(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            _productRepository.Add(product); 
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            _productRepository.Update(id, product); 
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            _productRepository.Delete(id);
        }  
    }
}
