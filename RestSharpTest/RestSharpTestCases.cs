using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace RestSharpTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }

    [TestClass]
    public class RestSharpTestCases
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse GetEmployeeList()
        {
            // arrange
            RestRequest request = new RestRequest("/employees", Method.GET);

            // act
            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void OnCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(10, dataResponse.Count);
        }
    }
}
