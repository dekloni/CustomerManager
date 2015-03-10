using CustomerManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerManager.Repository
{
    public class CustomerRepository : IDisposable
    {
        CustomerManagerContext _Context;
        private bool disposed;

        public CustomerRepository()
        {
            _Context = new CustomerManagerContext();
            //System.Threading.Thread.Sleep(5000); 
        }

        public IQueryable<Customer> GetCustomers()
        {
            var query = _Context.Customers
                        .Include("Orders")
                        .Include("State")
                    .Include("Orders.Payments")
                        .OrderBy(c => c.LastName);
            return query.AsQueryable();
        }

        public List<State> GetStates()
        {
            return _Context.States.OrderBy(s => s.Name).ToList();
        }

        public IQueryable<CustomerSummary> GetCustomersSummary(out int totalRecords)
        {
            var query = _Context.Customers
               .Include("States")
               .OrderBy(c => c.LastName);

            totalRecords = query.Count();

            return query.Select(c => new CustomerSummary
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                City = c.City,
                State = c.State,
                OrderCount = c.Orders.Count(),
                Gender = c.Gender
            }).AsQueryable();
        }

        public IQueryable<Payment> GetPayments()
        {
            var query = _Context.Payments
                       .Include("Order")
                //.Include("State")
                       .OrderBy(c => c.Date);
            return query.AsQueryable();
        }

        public OperationStatus CheckUnique(int id, string property, string value)
        {
            switch (property.ToLower())
            {
                case "email":
                    var unique = !_Context.Customers.Any(c => c.Id != id && c.Email == value);
                    return new OperationStatus { Status = unique };
                default:
                    return new OperationStatus();
            }
        }

        public Customer GetCustomerById(int id)
        {
            return _Context.Customers
                    .Include("Orders")
                    .Include("State")
                    .Include("Orders.Payments")
                    .SingleOrDefault(c => c.Id == id);
        }

        internal Order GetOrderById(int id)
        {
            return _Context.Orders
                   .Include("Customer")
                   .Include("Payments")
                   .SingleOrDefault(c => c.Id == id);
        }

        public OperationStatus InsertCustomer(Customer customer)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                _Context.Customers.Add(customer);
                _Context.SaveChanges();
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = exp.Message;
            }
            return opStatus;
        }

        public OperationStatus UpdateCustomer(Customer customer)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                customer.State.Id = customer.StateId;
                _Context.Customers.Attach(customer);
                _Context.Entry<Customer>(customer).State = System.Data.Entity.EntityState.Modified;
                _Context.SaveChanges();
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = exp.Message;
            }
            return opStatus;
        }


        public OperationStatus DeleteCustomer(int id)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                var cust = _Context.Customers.SingleOrDefault(c => c.Id == id);
                if (cust != null)
                {
                    _Context.Customers.Remove(cust);
                    _Context.SaveChanges();
                }
                else
                {
                    opStatus.Status = false;
                    opStatus.ExceptionMessage = "Customer not found";
                }
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = exp.Message;
            }
            return opStatus;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            this._Context.Dispose();

            disposed = true;
        }

        public OperationStatus InsertPayment(Payment customer)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                _Context.Payments.Add(customer);
                _Context.SaveChanges();
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = exp.Message;
                if (exp.InnerException != null)
                {
                    opStatus.ExceptionMessage += exp.InnerException.Message;
                    if (exp.InnerException.InnerException != null)
                    {
                        opStatus.ExceptionMessage += exp.InnerException.InnerException.Message;
                    }
                }
            }
            return opStatus;
        }

        public OperationStatus UpdatePayment(Payment payment)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                //payment.State.Id = payment.StateId;
                payment.Order.Id = payment.OrderId;
                _Context.Payments.Attach(payment);
                _Context.Entry<Payment>(payment).State = System.Data.Entity.EntityState.Modified;
                _Context.SaveChanges();
            }
            catch (Exception exp)
            {
                opStatus.Status = false;
                opStatus.ExceptionMessage = exp.Message;
            }
            return opStatus;
        }
    }
}