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
            objectBody.Add("name", "Johnny");
            objectBody.Add("salary", "12000");

            request.AddParameter("application/json", objectBody, ParameterType.RequestBody);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Johnny", dataResponse.name);
            Assert.AreEqual("12000", dataResponse.salary);
        }

        [TestMethod]
        public void GivenEmployee_OnPut_ShouldReturnUpdatedEmployee()
        {
            // arrange
            RestRequest request = new RestRequest("/employees/7", Method.PUT);
            JObject objectBody = new JObject();
            objectBody.Add("name", "Lavanya");
            objectBody.Add("salary", "13000");

            request.AddParameter("application/json", objectBody, ParameterType.RequestBody);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Lavanya", dataResponse.name);
            Assert.AreEqual("13000", dataResponse.salary);
        }

        [TestMethod]
        public void GivenEmployee_OnDelete_ShouldReturnSuccess()
        {
            // arrange
            RestRequest request = new RestRequest("/employees/13", Method.DELETE);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
    }
}
