using System;
using System.Linq;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Controllers;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_me.Tests.Integration.Controllers
{
    [TestClass]
    public class ProductsControllerTests : IntegrationTestBase
    {
        [TestMethod]
        public void Can_Get_All_Products()
        {
            var controller = GetProductsController();

            var products = controller.GetAll();

            Assert.AreEqual(products.Items.Count(), 3);
        }  

        [TestMethod]
        public void Can_Search_By_Name()
        {
            const string name = "iphone";
            var controller = GetProductsController();

            var products = controller.SearchByName(name);

            Assert.AreEqual(products.Items.Count(), 1);
            Assert.IsTrue(products.Items.First().Name.ToLowerInvariant().Contains(name));
        }

        [TestMethod]
        public void Serching_No_Existing_Product_Should_Return_Empty()
        {
            var controller = GetProductsController();

            var products = controller.SearchByName("zzz");

            Assert.AreEqual(products.Items.Count(), 0);
        }

        [TestMethod]
        public void Can_Get_Product_By_Id()
        {
            var controller = GetProductsController();
            var newProduct = new Product
            {
                DeliveryPrice = 12,
                Description = "Description",
                Name = "Name",
                Price = 32
            };
            controller.Create(newProduct);

            var product = controller.GetProduct(newProduct.Id);

            Assert.IsNotNull(product);
            Assert.AreEqual(product.Id, newProduct.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Getting_No_Existing_Product_Should_Throw()
        {
            var controller = GetProductsController();

            controller.GetProduct(Guid.Empty);
        }

        [TestMethod]
        public void Can_Create_Product()
        {
            var controller = GetProductsController();
            var newProduct = new Product {DeliveryPrice = 12, Description = "Test", Name = "Test", Price = 3};

            controller.Create(newProduct);
            var product = controller.GetProduct(newProduct.Id);

            Assert.IsNotNull(product);
        }

        [TestMethod]
        public void Can_Update_Product()
        {
            var productId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef");
            const string newName = "New Name";
            var controller = GetProductsController();
            var productToUpdate = new Product {Name = newName};


            controller.Update(productId, productToUpdate);
            var product = controller.GetProduct(productId);

            Assert.AreEqual(product.Name, newName); 
        }

        [TestMethod]
        public void Can_Delete_Product()
        {
            var controller = GetProductsController();
            var productId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef");
            var pass = false;

            controller.Delete(productId);

            try
            {
                controller.GetProduct(productId);
            }
            catch (HttpResponseException)
            {
                pass = true;
            }
            finally
            {
                Assert.IsTrue(pass);
            } 
        }

        private ProductsController GetProductsController()
        {
            var productRepository = new ProductRepository(GetTestDbConnection());
            return new ProductsController(productRepository);
        }
    }
}
