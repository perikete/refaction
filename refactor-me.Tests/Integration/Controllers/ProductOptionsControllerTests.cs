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
    public class ProductOptionsControllerTests : IntegrationTestBase
    {  
        [TestMethod]
        public void Can_Get_Options_For_Product()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var controller = GetController();

            var options = controller.GetOptions(productId);

            Assert.AreEqual(options.Items.Count(), 2);
        }

        [TestMethod]
        public void Can_Get_Product_Option()
        {
            var productId = Guid.NewGuid();
            var controller = GetController();
            var newProductOption = new ProductOption
            {
                Description = "Description",
                Name = "Name",
                ProductId = productId
            };
            controller.CreateOption(productId, newProductOption);

            var productOption = controller.GetOption(productId, newProductOption.Id);

            Assert.IsNotNull(productOption);
            Assert.AreEqual(productOption.Id, newProductOption.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Getting_No_Existing_Product_Option_Should_Throw()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Empty;
            var controller = GetController();

            controller.GetOption(productId, optionId);
        }

        [TestMethod]
        public void Can_Create_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var controller = GetController();
            var newProductOption = new ProductOption
            {
                Description = "test",
                Name = "test",
                ProductId = productId
            };

            controller.CreateOption(productId, newProductOption);

            var productOption = controller.GetOption(productId, newProductOption.Id);

            Assert.IsNotNull(productOption);
            Assert.AreEqual(productOption.Id, newProductOption.Id);
        }

        [TestMethod]
        public void Can_Update_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            const string expectedDescription = "test";
            const string expectedName = "Test Name";
            var productOptionToUpdate = new ProductOption { Description = expectedDescription, Name = expectedName };
            var controller = GetController();

            controller.UpdateOption(optionId, productOptionToUpdate);
            var productOption = controller.GetOption(productId, optionId);

            Assert.AreEqual(productOption.Description, expectedDescription);
            Assert.AreEqual(productOption.Name, expectedName);
        }

        [TestMethod]
        public void Can_Delete_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            var controller = GetController();
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

        private ProductOptionsController GetController()
        {
            var productRepository = new ProductRepository(GetTestDbConnection());
            return new ProductOptionsController(productRepository);
        }
    }
}
