using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Financial.BLL;
using Financial.Entities;
using SCSS.IntegrationApi.Client;
using SCSS.IntegrationApi.Model;
using SCSS.IntegrationApi.Model.User;
using ApplicationId = SCSS.IntegrationApi.Model.User.ApplicationId;

namespace IAPI.Fis.Demo.Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var ids = args
                .Select(a => int.TryParse(a, out var id) ? id : 0)
                .Where(id => id > 0)
                .ToList();

            if (ids.Count == 0)
            {
                Console.WriteLine("Supply list of seUser primary keys as arguments");
                return;
            }

            var endpoint = ConfigurationManager.AppSettings["endpoint"];
            var database = ConfigurationManager.AppSettings["database"];
            var token = ConfigurationManager.AppSettings["apitoken"];

            Console.WriteLine($"Preparing user sync for {database} using endpoint {endpoint}");

            var credentials = new AuthenticationCredentials(database, "scsstest", token, AuthenicationType.IntegrationApiSecurityToken);

            var task = new Sync(endpoint, credentials, ids).Process();

            task.Wait();
        }
    }

    internal class Sync
    {
        private readonly List<int> _ids;
        private readonly ISCSSUserIntegrationApi _client;

        internal Sync(string endpoint, AuthenticationCredentials credentials, List<int> ids)
        {
            _ids = ids;
            _client = SCSSUserClient.Create(endpoint, credentials, true);
        }

        public async Task Process()
        {

            var version = await _client.GetVersionAsync();
            Console.WriteLine($"Versions:\n\n{version}\n");

            var service = new seUserService();

            foreach (var id in _ids)
            {
                var source = service.GetUser(id);
                if (source != null)
                {
                    var target = ToModel(source);
                    await _client.PostAsync(target);
                }
                else
                {
                    Console.WriteLine($"Could not find seUser object with pk value: {id}");
                }
            }
        }

        private UserModel ToModel(seUser source)
        {
            var result = new UserModel
            {
                SourceApplication = ApplicationId.CSIU_Fis,
                UserId = source.Username,
                Email = new EmailAddress(source.Email),
                Name = new PersonName(source.FirstName, source.LastName),
            };

            if (result.UserId == "rtrently")
            {
                result.PrimaryPhone = new Phone("419-555-9876");
            }

            return result;

        }
    }
}