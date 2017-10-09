using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_me.Tests.Integration.Repositories
{
    [TestClass]
    public class ProductRepositoryTests : IntegrationTestBase
    {
        [TestMethod]
        public void Can_Get_All_Products()
        {
            var repository = GetProductRepository();

            var products = repository.GetAll();

            Assert.AreEqual(products.Items.Count(), 3);
        }

        [TestMethod]
        public void Can_Get_Products_By_Name_with_lowercase()
        {
            var repository = GetProductRepository();

            var products = repository.GetByName("iphone");

            Assert.AreEqual(products.Items.Count(), 1);
        }

        [TestMethod]
        public void Can_Get_Products_By_Name_with_uppercase()
        {
            var repository = GetProductRepository();

            var products = repository.GetByName("IPHONE");

            Assert.AreEqual(products.Items.Count(), 1);
        }

        [TestMethod]
        public void Can_Get_Products_By_Id()
        {
            var repository = GetProductRepository();
            var newProduct = new Product
            {
                Description = "Test",
                DeliveryPrice = 12, 
                Name = "Test Name",
                Price = 34
            };
            repository.Add(newProduct);

            var product = repository.GetById(newProduct.Id);

            Assert.IsNotNull(product);
            Assert.AreEqual(product.Id, newProduct.Id);
        }

        [TestMethod]
        public void Can_Add_A_Product()
        {
            var repository = GetProductRepository();
            var newProduct = new Product
            {
                Description = "Test",
                DeliveryPrice = 12,
                Name = "Test Name",
                Price = 34
            }; 
            repository.Add(newProduct);

            var product = repository.GetById(newProduct.Id);

            Assert.AreEqual(product.Name, newProduct.Name);
            Assert.AreEqual(product.DeliveryPrice, newProduct.DeliveryPrice);
            Assert.AreEqual(product.Description, newProduct.Description);
            Assert.AreEqual(product.Price, newProduct.Price);
            Assert.AreEqual(product.Id, newProduct.Id);
        }

        [TestMethod]
        public void Can_Update_Product()
        {
            var productId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef");
            var repository = GetProductRepository(); 
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
            var repository = GetProductRepository();
            var newProduct = new Product
            {
                Description = "Test",
                DeliveryPrice = 12,
                Name = "Test",
                Price = 32
            };
            repository.Add(newProduct);

            repository.Delete(newProduct.Id);

            var product = repository.GetById(newProduct.Id); 
            Assert.IsNull(product);
        }

        [TestMethod]
        public void Can_Add_Product_Option()
        {
            var productId = Guid.NewGuid();
            var repository = GetProductRepository();
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                ProductId = productId
            };
            
            repository.AddOption(productId, newProductOption);

            var productOptions = repository.GetOption(newProductOption.Id);
            Assert.AreEqual(productOptions.Id, newProductOption.Id);
        }

        [TestMethod]
        public void Can_Get_Product_Option()
        {
            var repository = GetProductRepository();
            var productId = Guid.NewGuid();
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                ProductId = productId
            };
            repository.AddOption(productId, newProductOption);

            var productOption = repository.GetOption(newProductOption.Id);

            Assert.IsNotNull(productOption);
            Assert.AreEqual(productOption.Id, newProductOption.Id);
        }

        [TestMethod]
        public void Can_Delete_Product_Option()
        {
            var repository = GetProductRepository();
            var productId = Guid.NewGuid();
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                ProductId = productId
            }; 
            repository.AddOption(productId, newProductOption);

            repository.DeleteOption(newProductOption.Id);

            var productOption = repository.GetOption(newProductOption.Id);

            Assert.IsNull(productOption);
        }

        [TestMethod]
        public void Can_Update_Product_Option()
        {
            var repository = GetProductRepository();
            var productId = Guid.NewGuid();
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                ProductId = productId
            };
            repository.AddOption(productId, newProductOption);
            var updatedOption = new ProductOption {Description = "Updated Description", Name = "Updated Name"};

            repository.UpdateOption(newProductOption.Id, updatedOption);

            var productOptions = repository.GetOption(newProductOption.Id);
            Assert.AreEqual(updatedOption.Name, productOptions.Name);
            Assert.AreEqual(updatedOption.Description, productOptions.Description);
        }

        [TestMethod]
        public void Can_Delete_Product_With_Options()
        {
            var repository = GetProductRepository();
            var newProduct = new Product
            {
                Name = "Test",
                Description = "Test",
                DeliveryPrice = 12,
                Price = 30
            };
            repository.Add(newProduct);
            var newProductOption = new ProductOption
            {
                Description = "Option 1",
                Name = "Option",
                ProductId = newProduct.Id
            }; 
            repository.AddOption(newProduct.Id, newProductOption);

            repository.Delete(newProduct.Id);

            var product = repository.GetById(newProduct.Id);
            var productOptions = repository.GetOptions(newProduct.Id);

            Assert.IsNull(product);
            Assert.AreEqual(productOptions.Count(), 0);
        }

        private ProductRepository GetProductRepository()
        {
            return new ProductRepository(GetTestDbConnection());    
        }
    }
}
