namespace POData
{
    public abstract class DataAccess
    {
        public string ConnectionString { get; set; }
        public DataAccess(string yhteys)
        {
            ConnectionString = yhteys;
        }

    }
}
