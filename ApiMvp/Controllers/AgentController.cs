using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using ApiMvp.Data;
using Microsoft.AspNetCore.Http;
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
        public ActionResult<IEnumerable<Agent>> Get()
        {
            try
            {
                IEnumerable<Agent> agents = GetAgents();
                return new ActionResult<IEnumerable<Agent>>(agents);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
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
            try
            {
                IEnumerable<Agent> agents = GetAgents();
                Agent agent = agents.FirstOrDefault(a => a.GetId() == id);

                if (agent == null)
                {
                    throw new Exception($"Agent with id {id} does not exist.");
                }

                return agent;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Add an agent
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        // POST api/agent/Add
        [HttpPost]
        public ActionResult Add([FromBody] Agent agent)
        {
            try
            {
                List<Agent> agents = GetAgents().ToList();

                if (agents.Any(a => a.Equals(agent)))
                {
                    throw new Exception("Cannot add Agent. Agent already exists.");
                }

                // Agent Id will be unique if it is the primary key on the table.
                Random rnd = new Random();
                agent.SetId(rnd.Next(int.MaxValue));
                agents.Add(agent);
                Agent.Save(_agentsFilePath, agents);
                return StatusCode((int)HttpStatusCode.OK, "Agent added successfully.");
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        /// <summary>
        /// Update an agent.
        /// </summary>
        /// <param name="id">The id of the agent to update.</param>
        /// <param name="agent">The agent to update.</param>
        // POST api/agent/Update
        [HttpPost]
        public ActionResult Update(int id, [FromBody] Agent agent)
        {
            try
            {
                List<Agent> agents = GetAgents().ToList();
                Agent existingAgent = agents.FirstOrDefault(a => a.GetId() == id);

                if (existingAgent == null)
                {
                    throw new Exception("Cannot update Agent. Agent does not exist.");
                }

                existingAgent.UpdateValues(agent);
                Agent.Save(_agentsFilePath, agents);
                return StatusCode((int)HttpStatusCode.OK, "Agent updated successfully.");
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
        }

        private IEnumerable<Agent> GetAgents()
        {
            List<Agent> agents = Agent.GetAllAgents(_agentsFilePath).ToList();

            if (!agents.Any())
            {
                throw new Exception($"No agents exist.");
            }

            return agents;
        }
    }
}
