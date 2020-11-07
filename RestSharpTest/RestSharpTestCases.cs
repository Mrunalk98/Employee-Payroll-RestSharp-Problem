using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            Assert.AreEqual(11, dataResponse.Count);
        }

        [TestMethod]
        public void GivenEmployee_DoPost_ShouldReturnAddedEmployee()
        {
            // arrange
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject objectBody = new JObject();
            objectBody.Add("name", "John");
            objectBody.Add("salary", "18000");

            request.AddParameter("application/json", objectBody, ParameterType.RequestBody);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("John", dataResponse.name);
            Assert.AreEqual("18000", dataResponse.salary);
        }
    }
}
