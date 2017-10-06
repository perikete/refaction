using System;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Controllers;
using refactor_me.Models;

namespace refactor_me.Tests.Integration
{
    [TestClass]
    public class ProductOptionsControllerTests
    {
        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            TestHelpers.SetUp();    
        } 

        [TestMethod]
        public void Can_Get_Options_For_Product()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var controller = new ProductOptionsController();

            var options = controller.GetOptions(productId);

            Assert.AreEqual(options.Items.Count, 2);
        }

        [TestMethod]
        public void Can_Get_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            var controller = new ProductOptionsController();

            var productOption = controller.GetOption(productId, optionId);

            Assert.IsNotNull(productOption);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Getting_No_Existing_Product_Option_Should_Throw()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Empty;
            var controller = new ProductOptionsController();

            controller.GetOption(productId, optionId);
        }

        [TestMethod]
        public void Can_Create_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var controller = new ProductOptionsController();
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
            var productOptionToUpdate = new ProductOption { Description = expectedDescription };
            var controller = new ProductOptionsController();

            controller.UpdateOption(optionId, productOptionToUpdate);
            var productOption = controller.GetOption(productId, optionId);

            Assert.AreEqual(productOption.Description, expectedDescription);
        }

        [TestMethod]
        public void Can_Delete_Product_Option()
        {
            var productId = Guid.Parse("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var optionId = Guid.Parse("0643ccf0-ab00-4862-b3c5-40e2731abcc9");
            var controller = new ProductOptionsController();
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
