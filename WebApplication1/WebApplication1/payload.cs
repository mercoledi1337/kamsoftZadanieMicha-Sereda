namespace WebApplication1
{
    public enum contentTypes
    {
        CSV,
        INTERNAL_JSON
    }
    public class payload
    {
        public contentTypes type;
        public string content { get; set; }
    }
}
