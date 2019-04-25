using System;
using System.Collections.Generic;
using System.Linq;
using Forte.RedirectMiddleware.Model;
using Forte.RedirectMiddleware.Model.RedirectRule;
using Forte.RedirectMiddleware.Model.RedirectType;
using Forte.RedirectMiddleware.Model.UrlPath;

namespace RedirectTests.Data
{
    public static class RandomDataGenerator
    {
        private const int MaxNumberOfDirectories = 3;
        private const int MaxLengthOfDirectory = 6;
        
        private static readonly Random RandomGenerator = new Random();
        
        public static RedirectRule CreateRandomRedirectRule()
        {
            return new RedirectRule
            {
                Id = Guid.NewGuid(),
                OldPath = UrlPath.Parse(GetRandomPath()),
                NewUrl = GetRandomPath(),
                IsActive = true,
                Notes = "some notes",
                CreatedOn = DateTime.Now,
                RedirectType = RedirectType.Temporary
            };
        }
        
        public static RedirectRuleDto CreateRandomRedirectRuleDto()
        {
            return new RedirectRuleDto()
            {
                Id = Guid.NewGuid(),
                OldPath = GetRandomPath(),
                NewUrl = GetRandomPath(),
                IsActive = true,
                Notes = "some notes",
                CreatedOn = DateTime.Now,
                RedirectType = RedirectType.Temporary
            };
        }
        
        private static string GetRandomPath()
        {
            var directoriesNumber = RandomGenerator.Next(1, MaxNumberOfDirectories);

            var directories = new List<string>();

            for (var i = 0; i < directoriesNumber; i++)
            {
                var directory = GetRandomDirectoryString(RandomGenerator);
                directories.Add(directory);
            }
            var randomPath = string.Join("/", directories);
            return randomPath;
        }
        
        private static string GetRandomDirectoryString(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, random.Next(1, MaxLengthOfDirectory))
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}