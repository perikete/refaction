using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Controllers;
using refactor_me.Models;

namespace refactor_me.Tests.Integration
{
    [TestClass]
    public class ProductsControllerTests
    {
        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            var dbPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName,
                "Databases");

            Helpers.TestDataDirectory = dbPath;

            if (File.Exists(Path.Combine(dbPath, "TestDatabase.mdf")))
            {
                File.Delete(Path.Combine(dbPath, "TestDatabase.mdf"));
                File.Delete(Path.Combine(dbPath, "TestDatabase_log.ldf"));
            }

            File.Copy(Path.Combine(dbPath, "Database.mdf"), Path.Combine(dbPath, "TestDatabase.mdf"));
        }

        [TestMethod]
        public void Can_Get_All_Products()
        {
            var controller = new ProductsController();

            var products = controller.GetAll();

            Assert.AreEqual(products.Items.Count, 3);
        }

        [TestMethod]
        public void Can_Search_By_Name()
        {
            const string name = "iphone";
            var controller = new ProductsController();

            var products = controller.SearchByName(name);

            Assert.AreEqual(products.Items.Count, 1);
            Assert.IsTrue(products.Items.First().Name.ToLowerInvariant().Contains(name));
        }

        [TestMethod]
        public void Serching_No_Existing_Product_Should_Return_Empty()
        {
            var controller = new ProductsController();

            var products = controller.SearchByName("zzz");

            Assert.AreEqual(products.Items.Count, 0);
        }

        [TestMethod]
        public void Can_Get_Product_By_Id()
        {
            var controller = new ProductsController();

            var product = controller.GetProduct(Guid.Parse("01234567-89ab-cdef-0123-456789abcdef"));

            Assert.IsNotNull(product);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Getting_No_Existing_Product_Should_Throw()
        {
            var controller = new ProductsController();

            controller.GetProduct(Guid.Empty);
        }

        [TestMethod]
        public void Can_Create_Product()
        {
            var productId = Guid.NewGuid();
            var controller = new ProductsController();
            var newProduct = new Product {DeliveryPrice = 12, Description = "Test", Id = productId, Name = "Test", Price = 3};

            controller.Create(newProduct);
            var product = controller.GetProduct(productId);

            Assert.IsNotNull(product);
        }

        [TestMethod]
        public void Can_Update_Product()
        {
            var productId = Guid.Parse("01234567-89ab-cdef-0123-456789abcdef");
            const string newName = "New Name";
            var controller = new ProductsController();
            var productToUpdate = new Product {Name = newName};


            controller.Update(productId, productToUpdate);
            var product = controller.GetProduct(productId);

            Assert.AreEqual(product.Name, newName); 
        }

        [TestMethod]
        public void Can_Delete_Product()
        {
            var controller = new ProductsController();
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

        [TestMethod]
        public void Can_Get_Options_For_Product()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var controller = new ProductsController();

            var options = controller.GetOptions(productId);

            Assert.AreEqual(options.Items.Count, 2);
        }

        [TestMethod]
        public void Can_Get_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            var controller = new ProductsController();

            var productOption = controller.GetOption(productId, optionId);

            Assert.IsNotNull(productOption);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Getting_No_Existing_Product_Option_Should_Throw()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Empty;
            var controller = new ProductsController();

           controller.GetOption(productId, optionId);
        }

        [TestMethod]
        public void Can_Create_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3"); 
            var controller = new ProductsController();
            var productOptionId = Guid.NewGuid();
            var newProductOption = new ProductOption
            {
                Description = "test",
                Id = productOptionId,
                Name = "test",
                ProductId = productId
            };

            controller.CreateOption(productId, newProductOption);

            var productOption = controller.GetOption(productId, productOptionId);

            Assert.IsNotNull(productOption);
        }

        [TestMethod]
        public void Can_Update_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            const string expectedDescription = "test";
            var productOptionToUpdate = new ProductOption { Description = expectedDescription};
            var controller = new ProductsController();

            controller.UpdateOption(optionId, productOptionToUpdate);
            var productOption = controller.GetOption(productId, optionId);

            Assert.AreEqual(productOption.Description, expectedDescription);
        }

        [TestMethod]
        public void Can_Delete_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            var controller = new ProductsController();
            var pass = false;

            controller.DeleteOption(optionId);
            try
            {
                controller.GetOption(productId, optionId);

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
       
    }
}
