namespace System
{
    public class Alert<T>
    {
        public Alert(string name, T alertInfo)
        {
            if (name.IsNullOrEmpty() || alertInfo == null)
                throw new ArgumentException("Alerts must have a name and some alert information.");

            Name = name;
            AlertInfo = alertInfo;
        }

        public string Name { get; private set; }
        public T AlertInfo { get; private set; }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Alert<T> that))
                return false;

            return this.Name == that.Name && this.AlertInfo!.Equals(that.AlertInfo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, AlertInfo);
        }
    }

}