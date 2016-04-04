using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Ranger.Core.Helpers;
using Ranger.Core.IssueTracker;
using Ranger.Core.Models;
using Ranger.Core.SourceControl;
using Ranger.Core.TemplateProvider;
using Ranger.Web.Models;
using Ranger.Web.Models.Configurations;
using Ranger.Web.Services;

namespace Ranger.Web.Controllers
{
    public class ConfigurationsController : Controller
    {
        private AppService _appService;

        public ConfigurationsController()
        {
            _appService = new AppService();
        }        

        public ActionResult Index(string id)
        {
            var vm = new ViewConfigurationsViewModel();
            vm.Teams = _appService.GetTeams();
            vm.SelectedTeam = string.IsNullOrEmpty(id) ? vm.Teams.FirstOrDefault() : vm.Teams.FirstOrDefault(x => x.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            vm.SelectedTeam = vm.SelectedTeam ?? string.Empty;
            vm.Configs = _appService.GetConfigs(vm.SelectedTeam);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(string id, ConfigViewModel config)
        {
            try
            {
                var conf = config.Configuration.ToObject<Config>();
                Guard.IsValidConfig(() => conf);
                string path = HttpContext.Server.MapPath("~/App_Data/configs");
                string fileName = $"{config.Name}.json";
                string fullpath = Path.Combine(path, id, fileName);
                System.IO.File.WriteAllText(fullpath, config.Configuration);
            }
            catch (JsonException ex)
            {

            }
            catch (ApplicationException ex)
            {

            }
            return RedirectToAction("Index", "Configurations", new { id = id});
        }

        public ActionResult Add(string id)
        {
            var vm = new CreateConfigViewModel();
            var sourceControlProviders = Utils.GetProviders<ISourceControl>();
            vm.SourceControlProviders = sourceControlProviders.Select(x => x.Name).ToList();
            var issueTrackerProviders = Utils.GetProviders<IIssueTracker>();
            vm.IssueTrackerProviders = issueTrackerProviders.Select(x => x.Name).ToList();
            var providerAttributes = Utils.GetProviders<ITemplate>();
            vm.TemplateProviders = providerAttributes.Select(x => x.Name).ToList();
            vm.TestConfig = new Conf
            {
                SourceControl = Activator.CreateInstance(sourceControlProviders.First().ConfigurationType),
                IssueTracker = Activator.CreateInstance(issueTrackerProviders.First().ConfigurationType),
                Template = Activator.CreateInstance(providerAttributes.First().ConfigurationType),
            };
            vm.Configuration = new ConfigViewModel();
            return View(vm);
        }
    }
}