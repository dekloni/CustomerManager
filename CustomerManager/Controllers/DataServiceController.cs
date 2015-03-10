using CustomerManager.Model;
using CustomerManager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CustomerManager.Controllers
{
    public class DataServiceController : ApiController, IDisposable
    {
        CustomerRepository _Repository;
        private bool disposed;

        public DataServiceController()
        {
            _Repository = new CustomerRepository();
        }

        [HttpGet]
        [Queryable]
        public HttpResponseMessage Customers()
        {
            var customers = _Repository.GetCustomers();
            var totalRecords = customers.Count();
            HttpContext.Current.Response.Headers.Add("X-InlineCount", totalRecords.ToString());
            return Request.CreateResponse(HttpStatusCode.OK, customers);
        }

        [HttpGet]
        [Queryable]
        public HttpResponseMessage Payments()
        {
            var g = _Repository.GetPayments().ToList();
            var totalRecords = g.Count();
            HttpContext.Current.Response.Headers.Add("X-InlineCount", totalRecords.ToString());
            return Request.CreateResponse(HttpStatusCode.OK, g);
        }

        [HttpGet]
        public HttpResponseMessage States()
        {
            var states = _Repository.GetStates();
            return Request.CreateResponse(HttpStatusCode.OK, states);
        }

        [HttpGet]
        [Queryable]
        public HttpResponseMessage CustomersSummary()
        {
            int totalRecords;
            var custSummary = _Repository.GetCustomersSummary(out totalRecords);
            HttpContext.Current.Response.Headers.Add("X-InlineCount", totalRecords.ToString());
            return Request.CreateResponse(HttpStatusCode.OK, custSummary);
        }

        [HttpGet]
        public HttpResponseMessage CheckUnique(int id, string property, string value)
        {
            var opStatus = _Repository.CheckUnique(id, property, value);
            return Request.CreateResponse(HttpStatusCode.OK, opStatus);
        }

        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserLogin userLogin)
        {
            //Simulated login
            return Request.CreateResponse(HttpStatusCode.OK, new { status = true });
        }

        [HttpPost]
        public HttpResponseMessage Logout()
        {
            //Simulated logout
            return Request.CreateResponse(HttpStatusCode.OK, new { status = true });
        }

        // GET api/<controller>/5
        [HttpGet]
        public HttpResponseMessage CustomerById(int id)
        {
            var customer = _Repository.GetCustomerById(id);
            return Request.CreateResponse(HttpStatusCode.OK, customer);
        }


        [HttpGet]
        public HttpResponseMessage PaymentById(int id)
        {
            var pmnt = _Repository.GetPayments().FirstOrDefault(p => p.Id == id);
            return Request.CreateResponse(HttpStatusCode.OK, pmnt);
        }


        // POST api/<controller>
        public HttpResponseMessage PostPayment([FromBody]Payment customer)
        {
            var opStatus = _Repository.InsertPayment(customer);
            if (opStatus.Status)
            {
                var response = Request.CreateResponse<Payment>(HttpStatusCode.Created, customer);
                string uri = Url.Link("DefaultApi", new { id = customer.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "kuku1");
        }


        // PUT api/<controller>/5
        public HttpResponseMessage PutPayment(int id, [FromBody]Payment customer)
        {
            var opStatus = _Repository.UpdatePayment(customer);
            if (opStatus.Status)
            {
                return Request.CreateResponse<Payment>(HttpStatusCode.Accepted, customer);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, "kuku2");
        }


        [HttpGet]
        public HttpResponseMessage OrderById(int id)
        {
            var order = _Repository.GetOrderById(id);
            return Request.CreateResponse(HttpStatusCode.OK, order);

        }

        // POST api/<controller>
        public HttpResponseMessage PostCustomer([FromBody]Customer customer)
        {
            var opStatus = _Repository.InsertCustomer(customer);
            if (opStatus.Status)
            {
                var response = Request.CreateResponse<Customer>(HttpStatusCode.Created, customer);
                string uri = Url.Link("DefaultApi", new { id = customer.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, opStatus.ExceptionMessage);
        }

        // PUT api/<controller>/5
        public HttpResponseMessage PutCustomer(int id, [FromBody]Customer customer)
        {
            var opStatus = _Repository.UpdateCustomer(customer);
            if (opStatus.Status)
            {
                return Request.CreateResponse<Customer>(HttpStatusCode.Accepted, customer);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotModified, opStatus.ExceptionMessage);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage DeleteCustomer(int id)
        {
            var opStatus = _Repository.DeleteCustomer(id);

            if (opStatus.Status)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, opStatus.ExceptionMessage);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            _Repository.Dispose();

            disposed = true;
        }
    }
}