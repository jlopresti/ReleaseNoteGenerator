using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using Ranger.Web.Models.Configurations;
using Ranger.Web.Models.Home;

namespace Ranger.Web.Services
{
    public class AppService
    {
        private readonly Lazy<string> APP_DATA_PATH = new Lazy<string>(() => HttpContext.Current.Server.MapPath("~/App_Data/"));
        private const string CONFIGS_PATH = "configs";
        private const string CONFIG_NAME_PATH = "global.config.json";
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

        public string GetConfig(string team)
        {
            if(string.IsNullOrEmpty(team) || !TeamExists(team))
                return null;

            var configPath = Path.Combine(APP_DATA_PATH.Value, CONFIGS_PATH, team, CONFIG_NAME_PATH);
            return File.Exists(configPath) ? File.ReadAllText(configPath) : null;
        }

        public void CreateConfig(string team, string config)
        {
            if (string.IsNullOrEmpty(team) || !TeamExists(team))
                return;
            var configPath = Path.Combine(APP_DATA_PATH.Value, CONFIGS_PATH, team, CONFIG_NAME_PATH);
            System.IO.File.WriteAllText(configPath, config);

        }

        public IEnumerable<string> GetComponents(string team)
        {
            var configPath = Path.Combine(APP_DATA_PATH.Value, CONFIGS_PATH, team, CONFIG_NAME_PATH);
            if(!File.Exists(configPath))
                return new List<string>();

            var cfg = new ReleaseNoteConfiguration(configPath);
            var components = cfg.Config.SourceControl["projectConfigs"] as JArray;
            return components.Select(x => x["project"].Value<string>()).ToList();
        }
    }
}
