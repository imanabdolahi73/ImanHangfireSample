using Castle.Core.Logging;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.Extensions.Logging;
using Moq;
using SampleHangfire.Controllers;
using SampleHangfire.Infrastrucures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexTest()
        {
            //Arrange
            var jobClinet = new Mock<IBackgroundJobClient>();
            var controller = new HomeController(null, jobClinet.Object);
            
            //Act
            controller.Index();

            //Assert
            jobClinet.Verify(p => p.Create(It.Is<Job>(job => job.Method.Name == nameof(EmailService.SendWellcome)), It.IsAny<EnqueuedState>()));
            jobClinet.Verify(p => p.Create(It.Is<Job>(job => job.Type.Name == nameof(EmailService)), It.IsAny<EnqueuedState>()));
        }
    }
}
