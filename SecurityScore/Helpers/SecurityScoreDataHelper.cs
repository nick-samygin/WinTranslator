using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasswordBoss.Model.SecurityScore;

namespace PasswordBoss.Helpers
{
    public class SecurityScoreDataHelper
    {
        private const decimal _duplicatePwdsMaxScore = 40;
        private const decimal _weakPwdsMaxScore = 40;
        private const decimal _oldPwdsMaxScore = 20;
        IResolver _resolver;
        public SecurityScoreDataHelper(IResolver resolver)
        {
            _resolver = resolver;
        }

        internal List<SecurityScoreData> GetSecurityScoreData(String type)
        {
            var SecurityScoreList = GetSecurityScoreItems(type);
            return SecurityScoreList;
        }

        private List<SecurityScoreData> GetSecurityScoreItems(string type)
        {
            PasswordScanner scanner = new PasswordScanner();
            IPBData pbData = _resolver.GetInstanceOf<IPBData>();
            var sites = pbData.GetSecureItemsByItemType(SecurityItemsDefaultProperties.SecurityItemType_PasswordVault);
            sites = sites.Where(p => !p.SharedWithUser && p.Data != null).ToList();

            var weakPwdStrength = new List<PasswordScanner.Strength> { PasswordScanner.Strength.VERYWEAK, PasswordScanner.Strength.WEAK };

            if (type == SecurityScoreItemType.week)
            {
                return sites.Where(p => weakPwdStrength.Contains(scanner.scanPassword(p.Data.password))).Select(s => new SecurityScoreData { id = s.Id, siteName = s.Name, siteUri = s.LoginUrl, password = s.Data.password, userName = !string.IsNullOrWhiteSpace(s.Data.username) ? s.Data.username : s.Data.email, LastModifiedDate = s.LastModifiedDate, ReEnterPassword = s.Data.require_master_password }).ToList();
            }

            if (type == SecurityScoreItemType.duplicate)
            {
                var pwdList = sites.Where(p=>!string.IsNullOrWhiteSpace(p.Data.password)).GroupBy(p => p.Data.password).Select(g => new { Key = g.Key, Count = g.Count() }).Where(o => o.Count > 1).Select(s => s.Key);
                return sites.Where(p => pwdList.Contains(p.Data.password)).Select(s => new SecurityScoreData { id = s.Id, siteName = s.Name, siteUri = s.LoginUrl, password = s.Data.password,userName = !string.IsNullOrWhiteSpace(s.Data.username) ? s.Data.username : s.Data.email, LastModifiedDate = s.LastModifiedDate, ReEnterPassword = s.Data.require_master_password }).ToList();
            }

            if (type == SecurityScoreItemType.old)
            {
                return sites.Where(p => p.LastModifiedDate < DateTime.Today.AddMonths(-6)).Select(s => new SecurityScoreData { id = s.Id, siteName = s.Name, siteUri = s.LoginUrl, password = s.Data.password, userName = !string.IsNullOrWhiteSpace(s.Data.username) ? s.Data.username : s.Data.email, LastModifiedDate = s.LastModifiedDate, ReEnterPassword = s.Data.require_master_password }).ToList();
            }

            if(type == SecurityScoreItemType.all)
            {
                return sites.Select(s => new SecurityScoreData { id = s.Id, siteName = s.Name, siteUri = s.LoginUrl, password = s.Data.password, userName = !string.IsNullOrWhiteSpace(s.Data.username) ? s.Data.username : s.Data.email, LastModifiedDate = s.LastModifiedDate, ReEnterPassword = s.Data.require_master_password }).ToList();
            }

            throw new InvalidOperationException(string.Format("Invalid imte type {0}", type));

        }

        internal decimal GetSecurityScorePercentage(List<SecurityScoreData> duplicatePwdList, List<SecurityScoreData> weakPwdList, List<SecurityScoreData> oldPwdList)
        {
            var totalPwds = GetSecurityScoreItems(SecurityScoreItemType.all).Count();
            decimal percentage = CalculatePercentageForPwds(duplicatePwdList, totalPwds, _duplicatePwdsMaxScore);
            percentage += CalculatePercentageForPwds(weakPwdList, totalPwds, _weakPwdsMaxScore);
            percentage += CalculatePercentageForPwds(oldPwdList, totalPwds, _oldPwdsMaxScore);

            return Math.Round(percentage, 0);
            //return string.Format("{0:N}%", percentage);
        }

        private decimal CalculatePercentageForPwds(List<SecurityScoreData> pwdList, decimal totalPwds, decimal maxScore)
        {
            if (pwdList.Count == 0 || totalPwds == 0)
            {
                return maxScore;
            }

            decimal percent = (pwdList.Count() / totalPwds) * new decimal(100);
            return maxScore - ((maxScore * percent) / 100);
        }
    }
}
