using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Ninject;
using Ranger.Core.IssueTracker;
using Ranger.Core.Linker;
using Ranger.Core.SourceControl;
using Ranger.Core.TemplateProvider;
using Ranger.Web.Models.Home;
using Ranger.Web.Services;

namespace Ranger.Web.Controllers
{
    public class PastReleaseController : Controller
    {
        private AppService _appservice;

        public PastReleaseController()
        {
            _appservice = new AppService();
        }

        // GET: Release
        public ActionResult Index(string id)
        {
            var teams = _appservice.GetTeams().ToList();
            var cookie = Request.Cookies["team"];
            var teamId = string.IsNullOrEmpty(id) && cookie != null ? cookie.Value : id;
            teamId = string.IsNullOrEmpty(teamId) ? teams.FirstOrDefault() : teamId;
            var vm = new CreateReleaseNoteViewModel();
            var components = _appservice.GetComponents(teamId);
            vm.Components = components.Select(x => new Compo() { Label = x }).ToList();
            vm.Teams = teams;
            vm.Team = teamId;
            Response.Cookies.Set(new HttpCookie("team", vm.Team));
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(string id, CreateReleaseNoteViewModel vm)
        {
            return RedirectToAction("ReleaseNote", new { id = id, release = vm.Release, components = Request.Form["components"] });
        }

        public async Task<ActionResult> Release(string id, string release, string components)
        {
            string path = HttpContext.Server.MapPath("~/App_Data/configs/" + id);
            var configPath = Directory.EnumerateFiles(path).FirstOrDefault(x => Path.GetFileName(x) == AppService.CONFIG_NAME_PATH);
            var cfg = new ReleaseNoteConfiguration(configPath);
            var cmp = cfg.Config.SourceControl["projectConfigs"] as JArray;
            var l = cmp.Where(x => components.Contains(x["project"].Value<string>())).ToList();
            cfg.Config.SourceControl["projectConfigs"] = new JArray(l);
            var kernel = new StandardKernel();
            kernel.Bind<ReleaseNoteConfiguration>().ToMethod(x => cfg);
            kernel.Load<ReleaseNoteModule>();
            var issueTracker = kernel.Get<IIssueTracker>();
            var sourceControl = kernel.Get<ISourceControl>();
            var releaseNoteLinker = kernel.Get<IReleaseNoteLinker>();
            var template = kernel.Get<ITemplate>();
            var issues = await issueTracker.GetIssues(release);
            var commits = await sourceControl.GetCommitsFromPastRelease(release);
            var releaseNoteModel = releaseNoteLinker.Link(commits, issues);
            var output = template.Build(release, releaseNoteModel);

            return View(new ReleaseNoteOutputViewModel() { Html = output });
        }

        public ActionResult ReleaseNote(string id, string release, string components)
        {
            ViewBag.Url = Url.Action("Release", new { id = id, release = release, components = components });
            return View();
        }
    }

}