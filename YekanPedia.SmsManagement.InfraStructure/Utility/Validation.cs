using System.Text.RegularExpressions;
using System.Web;

namespace YekanPedia.SmsManagement.InfraStructure.Utility
{
    public class Validation
    {
        /// <summary>
        /// Get IP Address
        /// </summary>
        /// <returns>Return IP address of client</returns>
        public static string GetIP()
        {
            var context = HttpContext.Current;
            try
            {
                if (context != null)
                {
                    string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        string[] addresses = ipAddress.Split(',');
                        if (addresses.Length != 0)
                        {
                            return addresses[0];
                        }
                    }
                    return context.Request.ServerVariables["REMOTE_ADDR"];
                }
                return context.Request.UserHostAddress;
            }
            catch
            {
                return "127.0.0.1";// string.Empty;
            }
        }

        /// <summary>
        /// Check For Valid Mobile Number
        /// </summary>
        /// <param name="Mobile">String  Mobile Number</param>
        /// <returns></returns>
        public static bool IsValidMobile(string Mobile)
        {
            try
            {
                Regex rgx = new Regex(@"^(09|9|989|0989|\+989)[0-9]{9}$");
                return rgx.IsMatch(Mobile);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Set Mobile Number To Its Correct Pattern, For Example : 989301919109
        /// </summary>
        /// <param name="Mobile">String  Mobile Number</param>
        /// <returns>String Value That Represent The Correct Pattern Of Mobile Number</returns>
        public static string SetMobilePattern(string Mobile)
        {
            string Pattern = Mobile;

            if (string.IsNullOrEmpty(Mobile)) return null;
            if (Mobile.Length == 10)
            {
                Pattern = "98" + Mobile;
            }
            else if (Mobile.Length == 11)
            {
                if (Mobile.StartsWith("0"))
                {
                    Pattern = "98" + Mobile.Substring(1);
                }
            }
            else if (Mobile.Length == 13)
            {
                Pattern = Mobile.Substring(1);
            }

            return Pattern;
        }


        /// <summary>
        /// Check For Valid App To Send SMS
        /// </summary>
        /// <param name="Username">Application Username</param>
        /// <param name="Password">Application Password</param>
        /// <param name="IP">Application IP Address</param>
        /// <returns>
        ///  1 Valid Application 
        /// -1 Invalid IP
        /// -2 Invalid Password or username
        /// -3 Deactive Application
        /// </returns>
        public static int IsValidApp(string Username, string Password, string IP)
        {
            //Select Valid App
            int appID = 0;
            if (Username == "" && Password == "")
                appID = 1;
            else
                appID = -2;

            return appID;
        }

        /// <summary>
        /// Get The Sms Service Caller Application ID
        /// </summary>
        /// <param name="Username">Application Username</param>
        /// <param name="Password">Application Password</param>
        /// <returns>Return Byte Value Of Sms Service Caller Application ID</returns>
        public static int GetAppID(string Username, string Password)
        {
            try
            {
                //Select App
                var appID = 1;

                return appID;
            }
            catch
            {
                return 0;
            }
        }

    }
}
