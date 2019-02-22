namespace ApiMvp.Data
{
    public class Phone
    {
        public string Primary { get; set; }
        public string Mobile { get; set; }

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

            Phone phone = (Phone)obj;
            return Primary == phone.Primary &&
                   Mobile == phone.Mobile;
        }
    }
}
