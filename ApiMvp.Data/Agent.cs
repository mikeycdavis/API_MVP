using System;

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
        public string Tier { get; set; }
        public Phone Phone { get; set; }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }
    }
}
