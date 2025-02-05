using my_virtual_pets_api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace my_virtual_pets.Tests
{
    public class DragonEyeAPITests
    {
        [SetUp]
        public void Setup()
        {
            Mock<IImagesService> _mockImageService = new Mock<IImagesService>();
            IImagesService _imageService = _mockImageService.Object;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}