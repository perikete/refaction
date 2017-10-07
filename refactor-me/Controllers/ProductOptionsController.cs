using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionsController : ApiController
    {
        private readonly ProductRepository _productRepository;

        public ProductOptionsController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            var productOptions = new ProductOptions
            {
                Items = _productRepository.GetOptions(productId)
            };
            return productOptions;
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = _productRepository.GetOption(id);
            if (option == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            _productRepository.AddOption(productId, option);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            _productRepository.UpdateOption(id, option);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
           _productRepository.DeleteOption(id);
        }

    }
}