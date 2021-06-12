using RestSharp;
using Xunit;
using Moq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace NcavScreener.Test
{
    public class ApiTest
    {
        private Api _api;

        public ApiTest()
        {
            _api = new Api();
        }

        [Fact]
        public void GetAuthResponse()
        {
            // Arrange
            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)).Content).Returns("crumb");
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)).Cookies).Returns(new List<RestResponseCookie> { new RestResponseCookie { Name = "foo", Value = "bar" } });

            // Act
            var crumb = _api.GetAuthResponse(restMoq.Object);

            // Assert
            Assert.Equal("crumb", crumb);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)), Times.Once());
        }

        [Fact]
        public void GetStockList()
        {
            // Arrange
            JObject o = new JObject();
            o["finance"] = new JObject();
            o["finance"]["result"] = new JArray(new JObject());
            o["finance"]["result"][0] = new JObject();
            o["finance"]["result"][0]["total"] = 600;
            o["finance"]["result"][0]["quotes"] = new JArray(new JObject(), new JObject());
            o["finance"]["result"][0]["quotes"][0]["symbol"] = "ABC";
            o["finance"]["result"][0]["quotes"][1]["symbol"] = "DEF";

            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.POST)).Content).Returns(o.ToString());

            // Act
            JArray result = _api.GetStockList(restMoq.Object);

            // Assert
            Assert.Equal(6, result.Count);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.POST)), Times.Exactly(3));
        }

        [Fact]
        public void GetStock()
        {
            // Arrange
            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)).Content).Returns("stock");

            // Act
            var stock = _api.GetStock(restMoq.Object);

            // Assert
            Assert.Equal("stock", stock);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)), Times.Once());
        }
    }  
}
