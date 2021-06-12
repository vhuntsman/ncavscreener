using RestSharp;
using Xunit;
using Moq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace NcavScreener.Test
{
    [ExcludeFromCodeCoverage]
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
        public void GetAuthResponseEmpty()
        {
            // Arrange
            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET))).Returns(new RestResponse() { });

            // Act
            var crumb = _api.GetAuthResponse(restMoq.Object);

            // Assert
            Assert.Equal(string.Empty, crumb);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)), Times.Once());
        }

        [Fact]
        public void GetAuthResponseNull()
        {
            // Arrange
            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET))).Returns((IRestResponse)null);

            // Act
            var crumb = _api.GetAuthResponse(restMoq.Object);

            // Assert
            Assert.Null(crumb);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)), Times.Once());
        }

        [Fact]
        public void GetAuthResponseNullCookies()
        {
            // Arrange
            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)).Content).Returns("crumb");
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)).Cookies).Returns((List<RestResponseCookie>)null);

            // Act
            var crumb = _api.GetAuthResponse(restMoq.Object);

            // Assert
            Assert.Equal("crumb", crumb);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)), Times.Once());
        }

        [Fact]
        public void GetAuthResponseEmptyCookiesList()
        {
            // Arrange
            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)).Content).Returns("crumb");
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)).Cookies).Returns(new List<RestResponseCookie> { null });

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
            JObject o1 = new JObject();
            o1["finance"] = new JObject();
            o1["finance"]["result"] = new JArray(new JObject());
            o1["finance"]["result"][0] = new JObject();
            o1["finance"]["result"][0]["total"] = 600;
            o1["finance"]["result"][0]["quotes"] = new JArray(new JObject(), new JObject());
            o1["finance"]["result"][0]["quotes"][0]["symbol"] = "ABC";
            o1["finance"]["result"][0]["quotes"][1]["symbol"] = "DEF";

            JObject o2 = new JObject();
            o2["finance"] = new JObject();
            o2["finance"]["result"] = new JArray(new JObject());
            o2["finance"]["result"][0] = new JObject();
            o2["finance"]["result"][0]["total"] = 600;
            o2["finance"]["result"][0]["quotes"] = new JArray(new JObject(), new JObject());
            o2["finance"]["result"][0]["quotes"][0]["symbol"] = "GHI";
            o2["finance"]["result"][0]["quotes"][1]["symbol"] = "JKL";

            JObject o3 = new JObject();
            o3["finance"] = new JObject();
            o3["finance"]["result"] = new JArray(new JObject());
            o3["finance"]["result"][0] = new JObject();
            o3["finance"]["result"][0]["total"] = 600;
            o3["finance"]["result"][0]["quotes"] = new JArray(new JObject(), new JObject());
            o3["finance"]["result"][0]["quotes"][0]["symbol"] = "MNO";
            o3["finance"]["result"][0]["quotes"][1]["symbol"] = "PQR";

            var restMoq = new Mock<IRestClient>();
            restMoq.SetupSequence(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.POST)).Content).Returns(o1.ToString()).Returns(o2.ToString()).Returns(o3.ToString());

            // Act
            JArray result = _api.GetStockList(restMoq.Object);

            // Assert
            Assert.Equal(6, result.Count);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.POST)), Times.Exactly(3));
            Assert.Equal("ABC", result[0]["symbol"].ToString());
            Assert.Equal("DEF", result[1]["symbol"].ToString());
            Assert.Equal("GHI", result[2]["symbol"].ToString());
            Assert.Equal("JKL", result[3]["symbol"].ToString());
            Assert.Equal("MNO", result[4]["symbol"].ToString());
            Assert.Equal("PQR", result[5]["symbol"].ToString());
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

        [Fact]
        public void GetStockNull()
        {
            // Arrange
            var restMoq = new Mock<IRestClient>();
            restMoq.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET))).Returns((IRestResponse)null);

            // Act
            var stock = _api.GetStock(restMoq.Object);

            // Assert
            Assert.Null(stock);
            restMoq.Verify(x => x.Execute(It.Is<IRestRequest>(r => r.Method == Method.GET)), Times.Once());
        }
    }  
}
