namespace YekanPedia.SmsManagement.Domain
{
    public class SmsList
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] DestinationNumbers { get; set; }
        public string[] Messages { get; set; }
    }
}