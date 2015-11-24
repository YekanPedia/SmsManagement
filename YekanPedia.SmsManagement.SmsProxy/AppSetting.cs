namespace YekanPedia.SmsManagement.SmsProxy
{
    using System.Configuration;
    public static class AppSettings
    {
        public static string UserName { get { return ConfigurationManager.AppSettings["UserName"]; } }
        public static string Password { get { return ConfigurationManager.AppSettings["Password"]; } }
    }
}