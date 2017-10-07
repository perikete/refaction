using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_me.Tests.Integration.Repositories
{
    [TestClass]
    public class ProductRepositoryTests
    {
        [TestInitialize]
        public void SetUp()
        {
            TestHelpers.SetUp();
        }

        [TestMethod]
        public void Can_Get_All_Products()
        {
            var repository = GetProductRepository();

            var products = repository.GetAll();

            Assert.AreEqual(products.Items.Count(), 3);
        }

        [TestMethod]
        public void Can_Get_Products_By_Name()
        {
            var repository = GetProductRepository();

            var products = repository.GetByName("iphone");

            Assert.AreEqual(products.Items.Count(), 1);
        }

        [TestMethod]
        public void Can_Get_Products_By_Id()
        {
            var repository = GetProductRepository();
            var productId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef");

            var product = repository.GetById(productId);

            Assert.IsNotNull(product);
        }

        [TestMethod]
        public void Can_Add_A_Product()
        {
            var repository = GetProductRepository();
            var productId = Guid.NewGuid();
            var newProduct = new Product
            {
                Description = "Test",
                DeliveryPrice = 12,
                Id = productId,
                Name = "Test Name",
                Price = 34
            };

            repository.Add(newProduct);

            var product = repository.GetById(productId);

            Assert.AreEqual(product.Name, newProduct.Name);
            Assert.AreEqual(product.DeliveryPrice, newProduct.DeliveryPrice);
            Assert.AreEqual(product.Description, newProduct.Description);
            Assert.AreEqual(product.Price, newProduct.Price);
        }

        [TestMethod]
        public void Can_Update_Product()
        {
            var productId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef");
            var repository = GetProductRepository();
            repository.Add(new Product {Id = productId});
            var productToUpdate = new Product
            {
                Description = "Test",
                DeliveryPrice = 12,
                Name = "Test Name",
                Price = 34
            };

            repository.Update(productId, productToUpdate);
            var product = repository.GetById(productId);

            Assert.AreEqual(product.Name, product.Name);
            Assert.AreEqual(product.DeliveryPrice, product.DeliveryPrice);
            Assert.AreEqual(product.Description, product.Description);
            Assert.AreEqual(product.Price, product.Price); 
        }

        [TestMethod]
        public void Can_Delete_Product_Without_Options()
        {
            var productId = Guid.NewGuid();
            var repository = GetProductRepository();
            repository.Add(new Product { Id = productId });

            repository.Delete(productId);

            var product = repository.GetById(productId);

            Assert.IsNull(product);
        }

        [TestMethod]
        public void Can_Add_Product_Option()
        {
            var repository = GetProductRepository();
            var productId = Guid.NewGuid();
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                Id = Guid.NewGuid(),
                ProductId = productId
            };

            repository.AddOption(productId, newProductOption);

            var productOptions = repository.GetOptions(productId);

            Assert.AreEqual(productOptions.Count(), 1); 
        }

        [TestMethod]
        public void Can_Update_Product_Option()
        {
            var repository = GetProductRepository();
            var productId = Guid.NewGuid();
            var newOptionId = Guid.NewGuid();
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                Id = newOptionId,
                ProductId = productId
            };
            repository.AddOption(productId, newProductOption);
            var updatedOption = new ProductOption {Description = "Updated Description", Name = "Updated Name"};

            repository.UpdateOption(newOptionId, updatedOption);

            var productOptions = repository.GetOptions(productId);

            Assert.AreEqual(productOptions.Count(), 1);
            Assert.AreEqual(updatedOption.Name, productOptions.First().Name);
            Assert.AreEqual(updatedOption.Description, productOptions.First().Description);
        }

        [TestMethod]
        public void Can_Delete_Product_With_Options()
        {
            var productId = Guid.NewGuid();
            var repository = GetProductRepository();
            repository.Add(new Product { Id = productId });
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                Id = Guid.NewGuid(),
                ProductId = productId
            }; 
            repository.AddOption(productId, newProductOption);

            repository.Delete(productId);

            var product = repository.GetById(productId);
            var productOptions = repository.GetOptions(productId);

            Assert.IsNull(product);
            Assert.AreEqual(productOptions.Count(), 0);
        }

        private static ProductRepository GetProductRepository()
        {
            return new ProductRepository(DbHelpers.NewConnection());    
        }
    }
}
