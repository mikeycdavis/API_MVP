using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiMvp.Data
{
    public class Customer
    {
        private int _id;
        public int Agent_Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public string Balance { get; set; }
        public int Age { get; set; }
        public string EyeColor { get; set; }
        public Name Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Registered { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> Tags { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Customer customer = (Customer)obj;
            List<string> firstNotSecond = Tags.Except(customer.Tags).ToList();
            List<string> secondNotFirst = customer.Tags.Except(Tags).ToList();

            return Agent_Id == customer.Agent_Id &&
                   Guid == customer.Guid &&
                   IsActive == customer.IsActive &&
                   Balance == customer.Balance &&
                   Age == customer.Age &&
                   EyeColor == customer.EyeColor &&
                   Name.Equals(customer.Name) &&
                   Company == customer.Company &&
                   Email == customer.Email &&
                   Phone == customer.Phone &&
                   Address == customer.Address &&
                   Registered == customer.Registered &&
                   Math.Abs(Latitude - customer.Latitude) < 0.0000001 &&
                   Math.Abs(Longitude - customer.Longitude) < 0.0000001 &&
                   !firstNotSecond.Any() && !secondNotFirst.Any();
        }

        public static IEnumerable<Customer> GetAllCustomers(string filePath)
        {
            List<Customer> customers = new List<Customer>();
            // I would have this come from data in a database using Entity Framework.
            // I kept this JSON to stick within the project time limit.
            JArray customersJson = GetCustomersJson(filePath);

            if (!customersJson.Children().Any())
            {
                return customers;
            }

            foreach (JToken token in customersJson.Children())
            {
                Customer customer = JsonConvert.DeserializeObject<Customer>(JsonConvert.SerializeObject(token));
                customer.SetId(token["_id"].Value<int>());
                customers.Add(customer);
            }

            return customers;
        }

        public static void Save(string filePath, IEnumerable<Customer> customers)
        {
            string customersJson = JsonConvert.SerializeObject(customers.ToArray(), Formatting.Indented);
            JsonReader.WriteJsonFile(filePath, customersJson);
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public void UpdateValues(Customer customer)
        {
            Agent_Id = customer.Agent_Id;
            Guid = customer.Guid;
            IsActive = customer.IsActive;
            Balance = customer.Balance;
            Age = customer.Age;
            EyeColor = customer.EyeColor;
            Name = customer.Name;
            Company = customer.Company;
            Email = customer.Email;
            Phone = customer.Phone;
            Address = customer.Address;
            Registered = customer.Registered;
            Latitude = customer.Latitude;
            Longitude = customer.Longitude;
        }

        private static JArray GetCustomersJson(string customersFilePath)
        {
            JArray customersJson = JsonConvert.DeserializeObject<JArray>(JsonReader.ReadJsonFile(customersFilePath));
            return customersJson;
        }
    }
}
