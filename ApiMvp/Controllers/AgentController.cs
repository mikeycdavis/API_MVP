using System.Collections.Generic;
using System.Linq;
using ApiMvp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonReader = ApiMvp.Data.JsonReader;

namespace ApiMvp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IOptions<Config> _config;

        public AgentController(IOptions<Config> config)
        {
            _config = config;
        }

        // GET api/agent
        [HttpGet]
        public ActionResult<IEnumerable<Agent>> Get()
        {
            List<Agent> agents = new List<Agent>();
            JArray agentsJson = GetAgentsJson();

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

        // GET api/agent/5
        [HttpGet("{id}")]
        public ActionResult<Agent> Get(int id)
        {
            ActionResult<IEnumerable<Agent>> agents = Get();
            Agent agent = agents.Value.FirstOrDefault(a => a.GetId() == id);
            return agent;
        }

        // POST api/agent
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/agent/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/agent/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private JArray GetAgentsJson()
        {
            JArray agentsJson = JsonConvert.DeserializeObject<JArray>(JsonReader.ReadJsonFile($"{_config.Value.DataFilePath}\\agents.json"));
            return agentsJson;
        }
    }
}
