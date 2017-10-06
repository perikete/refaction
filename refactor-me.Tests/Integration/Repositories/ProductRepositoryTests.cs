using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Assert.AreEqual(products.Count(), 3);
        }

        [TestMethod]
        public void Can_Get_Products_By_Name()
        {
            var repository = GetProductRepository();

            var products = repository.GetByName("iphone");

            Assert.AreEqual(products.Count(), 1);
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
                Id = productId,
                Name = "Test Name",
                Price = 34
            };

            repository.Update(productToUpdate);
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

        private static ProductRepository GetProductRepository()
        {
            return new ProductRepository(DbHelpers.NewConnection());    
        }
    }
}
