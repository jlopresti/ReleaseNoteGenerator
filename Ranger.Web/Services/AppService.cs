using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ranger.Web.Models.Configurations;

namespace Ranger.Web.Services
{
    public class AppService
    {
        private readonly Lazy<string> APP_DATA_PATH = new Lazy<string>(() => HttpContext.Current.Server.MapPath("~/App_Data/"));
        private const string CONFIGS_PATH = "configs";
        private const string TEMPLATES_PATH = "templates";

        public IEnumerable<string> GetTeams()
        {
            return Directory.EnumerateDirectories(Path.Combine(APP_DATA_PATH.Value, CONFIGS_PATH)).Select(x =>
            {
                var directoryInfo = new DirectoryInfo(x);
                return directoryInfo.Name;
            }).ToList();
        }

        public void AddTeam(string name)
        {
            var directory = Path.Combine(APP_DATA_PATH.Value, CONFIGS_PATH, name);
            if (!string.IsNullOrEmpty(name) && !TeamExists(name))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public bool TeamExists(string name)
        {
            var directory = Path.Combine(APP_DATA_PATH.Value, CONFIGS_PATH, name);
            return Directory.Exists(directory);
        }

        public IEnumerable<ConfigViewModel> GetConfigs(string team)
        {
            if(string.IsNullOrEmpty(team) || !TeamExists(team))
                return new List<ConfigViewModel>();

            var configPath = Path.Combine(APP_DATA_PATH.Value, CONFIGS_PATH, team);
            return Directory.EnumerateFiles(configPath).Select(x => new ConfigViewModel()
            {
                Name = Path.GetFileName(x)
            }).ToList();
        }
    }
}
