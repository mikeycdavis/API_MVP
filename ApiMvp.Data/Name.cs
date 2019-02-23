namespace ApiMvp.Data
{
    public class Name
    {
        public string First { get; set; }
        public string Last { get; set; }

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

            Name name = (Name)obj;
            return First == name.First && Last == name.Last;
        }
    }
}
