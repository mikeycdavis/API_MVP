using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ApiMvp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiMvp.Controllers
{
    /// <summary>
    /// API methods for a Customer.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IOptions<Config> _config;
        private readonly string _customersFilePath;

        public CustomerController(IOptions<Config> config)
        {
            _config = config;
            _customersFilePath = $"{_config.Value.DataFilePath}\\customers.json";
        }

        /// <summary>
        /// Get all agent customers.
        /// </summary>
        /// <param name="agent_id">The agent id.</param>
        /// <returns>The customers for the agent.</returns>
        // GET api/Customer/GetAgentCustomers/5
        [HttpGet("agent_id")]
        public ActionResult<IEnumerable<object>> GetAgentCustomers(int agent_id)
        {
            try
            {
                List<Customer> customers = GetCustomers().Where(c => c.Agent_Id == agent_id).ToList();

                if (!customers.Any())
                {
                    throw new Exception($"No customers have an agent with id {agent_id}.");
                }

                return new ActionResult<IEnumerable<object>>(customers.Select(c => new
                {
                    Name = $"{c.Name.Last}, {c.Name.First}",
                    Location = $"{c.Address.Split(',')[1].Trim()}, {c.Address.Split(',')[2].Trim()}"
                }));
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Get a customer.
        /// </summary>
        /// <param name="id">The customer id.</param>
        /// <returns>The customer.</returns>
        // GET api/customer/Get/5
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            try
            {
                IEnumerable<Customer> customers = GetCustomers();
                Customer customer = customers.FirstOrDefault(a => a.GetId() == id);

                if (customer == null)
                {
                    throw new Exception($"Customer with id {id} does not exist.");
                }

                return customer;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Add a customer.
        /// </summary>
        /// <param name="customer">The customer to add.</param>
        // POST api/customer/Add
        [HttpPost]
        public ActionResult Add([FromBody] Customer customer)
        {
            try
            {
                List<Customer> customers = GetCustomers().ToList();

                if (customers.Any(a => a.Equals(customer)))
                {
                    throw new Exception("Cannot add Customer. Customer already exists.");
                }

                customers.Add(customer);
                Customer.Save(_customersFilePath, customers);
                return StatusCode((int)HttpStatusCode.OK, "Customer added successfully.");
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Update a customer.
        /// </summary>
        /// <param name="id">The customer id to update.</param>
        /// <param name="customer">The customer to update.</param>
        // POST api/customer/Update
        [HttpPost]
        public ActionResult Update([FromBody] Customer customer)
        {
            try
            {
                List<Customer> customers = GetCustomers().ToList();
                Customer existingCustomer = customers.FirstOrDefault(a => a.GetId() == customer.GetId());

                if (existingCustomer == null)
                {
                    throw new Exception("Cannot update Customer. Customer does not exist.");
                }

                existingCustomer.UpdateValues(customer);
                Customer.Save(_customersFilePath, customers);
                return StatusCode((int)HttpStatusCode.OK, "Customer updated successfully.");
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Delete a customer.
        /// </summary>
        /// <param name="id">The customer id to delete.</param>
        // DELETE api/customer/Delete
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                List<Customer> customers = GetCustomers().ToList();
                Customer customer = customers.FirstOrDefault(c => c.GetId() == id);

                if (customer == null)
                {
                    throw new Exception("Cannot delete Customer. Customer does not exist.");
                }

                customers.Remove(customer);
                Customer.Save(_customersFilePath, customers);
                return StatusCode((int)HttpStatusCode.OK, "Customer deleted successfully.");
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        private IEnumerable<Customer> GetCustomers()
        {
            List<Customer> customers = Customer.GetAllCustomers(_customersFilePath).ToList();

            if (!customers.Any())
            {
                throw new Exception($"No customers exist.");
            }

            return customers;
        }
    }
}
