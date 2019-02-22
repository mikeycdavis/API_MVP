using System;
using System.Collections.Generic;
using System.Linq;
using ApiMvp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiMvp.Controllers
{
    /// <summary>
    /// API methods for an Agent.
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IOptions<Config> _config;
        private readonly string _agentsFilePath;

        public AgentController(IOptions<Config> config)
        {
            _config = config;
            _agentsFilePath = $"{_config.Value.DataFilePath}\\agents.json";
        }

        /// <summary>
        /// Get all agents.
        /// </summary>
        /// <returns>All agents by name.</returns>
        // GET api/Agent/Get
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            IEnumerable<Agent> agents = Agent.GetAllAgents(_agentsFilePath);
            return new ActionResult<IEnumerable<string>>(agents.Select(a => a.Name));
        }

        /// <summary>
        /// Get an agent by id.
        /// </summary>
        /// <param name="id">The agent id.</param>
        /// <returns>The agent.</returns>
        // GET api/agent/Get/5
        [HttpGet("{id}")]
        public ActionResult<Agent> Get(int id)
        {
            IEnumerable<Agent> agents = Agent.GetAllAgents(_agentsFilePath);
            Agent agent = agents.FirstOrDefault(a => a.GetId() == id);
            return agent;
        }

        /// <summary>
        /// Add an agent
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        // POST api/agent/Add
        [HttpPost]
        public void Add([FromBody] Agent agent)
        {
            List<Agent> agents = Agent.GetAllAgents(_agentsFilePath).ToList();

            if (agents.Any(a => a.Equals(agent)))
            {
                throw new Exception("Cannot add Agent. Agent already exists in the system.");
            }

            // Agent Id will be unique if it is the primary key on the table.
            Random rnd = new Random();
            agent.SetId(rnd.Next(int.MaxValue));
            agents.Add(agent);
            Agent.Save(_agentsFilePath, agents);
        }

        /// <summary>
        /// Update an agent.
        /// </summary>
        /// <param name="id">The id of the agent to update.</param>
        /// <param name="agent">The agent to update.</param>
        // POST api/agent/Update
        [HttpPost]
        public void Update(int id, [FromBody] Agent agent)
        {
            List<Agent> agents = Agent.GetAllAgents(_agentsFilePath).ToList();
            Agent existingAgent = agents.FirstOrDefault(a => a.GetId() == id);

            if (existingAgent == null)
            {
                throw new Exception("Cannot update Agent. Agent does not exist in the system.");
            }

            existingAgent.UpdateValues(agent);
            Agent.Save(_agentsFilePath, agents);
        }
    }
}
