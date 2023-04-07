namespace AnvizWeb.Models
{
    public class BiometricDevices
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public string Location { get; set; }
        public DateTime LastFailDateTime { get; set; }
    }
}
