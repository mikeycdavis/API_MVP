using System;
using System.Collections.Generic;
using System.Linq;
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
            List<Customer> customers = Customer.GetAllCustomers(_customersFilePath).Where(c => c.Agent_Id == agent_id).ToList();

            if (!customers.Any())
            {
                throw new Exception($"No customers have an agent with id {agent_id}.");
            }

            return new ActionResult<IEnumerable<object>>(customers.Select(c => new { Name = $"{c.Name.Last}, {c.Name.First}", Location = $"{c.Address.Split()[2]},{c.Address.Split()[4]}" }));
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
            List<Customer> customers = GetCustomers();
            Customer customer = customers.FirstOrDefault(a => a.GetId() == id);

            if (customer == null)
            {
                throw new Exception($"Customer with id {id} does not exist.");
            }

            return customer;
        }

        /// <summary>
        /// Add a customer.
        /// </summary>
        /// <param name="customer">The customer to add.</param>
        // POST api/customer/Add
        [HttpPost]
        public void Add([FromBody] Customer customer)
        {
            List<Customer> customers = GetCustomers();

            if (customers.Any(a => a.Equals(customer)))
            {
                throw new Exception("Cannot add Customer. Customer already exists in the system.");
            }

            customers.Add(customer);
            Customer.Save(_customersFilePath, customers);
        }

        /// <summary>
        /// Update a customer.
        /// </summary>
        /// <param name="id">The customer id to update.</param>
        /// <param name="customer">The customer to update.</param>
        // POST api/customer/Update
        [HttpPost]
        public void Update(int id, [FromBody] Customer customer)
        {
            List<Customer> customers = GetCustomers();
            Customer existingCustomer = customers.FirstOrDefault(a => a.GetId() == id);

            if (existingCustomer == null)
            {
                throw new Exception("Cannot update Customer. Customer does not exist in the system.");
            }

            existingCustomer.UpdateValues(customer);
            Customer.Save(_customersFilePath, customers);
        }

        /// <summary>
        /// Delete a customer.
        /// </summary>
        /// <param name="id">The customer id to delete.</param>
        // DELETE api/customer/Delete
        [HttpDelete]
        public void Delete(int id)
        {
            List<Customer> customers = GetCustomers();
            Customer customer = customers.FirstOrDefault(c => c.GetId() == id);

            if (customer == null)
            {
                return;
            }

            customers.Remove(customer);
            Customer.Save(_customersFilePath, customers);
        }

        private List<Customer> GetCustomers()
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
