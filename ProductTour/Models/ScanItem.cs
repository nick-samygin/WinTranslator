using PasswordBoss;
using System.Linq;
using ProductTour.BusinessLayer;

namespace ProductTour.Models
{
    public class ScanItem
    {
        public string Site { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public ScanRiskFlag Risk { get; set; }

        private static IRiskCalculator riskCalculator = new RiskCalculator();

        public ScanItem(string site, string username, string password)
        {
            Site = site;
            Username = username;
            Password = password;
            Risk = riskCalculator.GetRisk(Password);
        }

        // for tests
        public ScanItem(string site, string username, string password, ScanRiskFlag risk)
            : this(site, username, password)
        {
            Risk.Add(risk);
        }

        public static ScanItem FromLogin(LoginInfo loginInfo)
        {
            return new ScanItem(loginInfo.Url, loginInfo.UserName, loginInfo.Password);
        }
    }
}
