using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiMvp.Data
{
    public class Agent
    {
        private int _id;

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int Tier { get; set; }
        public Phone Phone { get; set; }

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

            Agent agent = (Agent)obj;
            return Name == agent.Name &&
                   Address == agent.Address &&
                   City == agent.City &&
                   State == agent.State &&
                   ZipCode == agent.ZipCode &&
                   Tier == agent.Tier &&
                   Phone.Equals(agent.Phone);
        }

        public static IEnumerable<Agent> GetAllAgents(string filePath)
        {
            List<Agent> agents = new List<Agent>();
            // I would have this come from data in a database using Entity Framework.
            // I kept this JSON to stick within the project time limit.
            JArray agentsJson = GetAgentsJson(filePath);

            if (!agentsJson.Children().Any())
            {
                return agents;
            }

            foreach (JToken token in agentsJson.Children())
            {
                Agent agent = JsonConvert.DeserializeObject<Agent>(JsonConvert.SerializeObject(token));
                agent.SetId(token["_id"].Value<int>());
                agents.Add(agent);
            }

            return agents;
        }

        public static void Save(string filePath, IEnumerable<Agent> agents)
        {
            string agentsJson = JsonConvert.SerializeObject(agents.ToArray(), Formatting.Indented);
            JsonReader.WriteJsonFile(filePath, agentsJson);
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public void UpdateValues(Agent agent)
        {
            Name = agent.Name;
            Address = agent.Address;
            City = agent.City;
            State = agent.State;
            ZipCode = agent.ZipCode;
            Tier = agent.Tier;
            Phone = agent.Phone;
        }

        private static JArray GetAgentsJson(string filePath)
        {
            JArray agentsJson = JsonConvert.DeserializeObject<JArray>(JsonReader.ReadJsonFile(filePath));
            return agentsJson;
        }
    }
}
