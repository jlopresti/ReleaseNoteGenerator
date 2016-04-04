using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Ninject;
using Ranger.Core.Helpers;
using Ranger.Core.IssueTracker;
using Ranger.Core.Linker;
using Ranger.Core.Models;
using Ranger.Core.SourceControl;
using Ranger.Core.TemplateProvider;
using Ranger.Web.Models;
using Ranger.Web.Models.Home;

namespace Ranger.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vm = new CreateReleaseNoteViewModel();
            string path = HttpContext.Server.MapPath("~/App_Data/configs");
            var files = Directory.EnumerateFiles(path);
            vm.Configs = files.Select(Path.GetFileName).ToList();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(CreateReleaseNoteViewModel vm)
        {
            return RedirectToAction("ReleaseNote", new { id = vm.Release, config = vm.Config });
        }

        public async Task<ActionResult> Release(string id, string config)
        {
            string path = HttpContext.Server.MapPath("~/App_Data/configs");
            var configPath = Directory.EnumerateFiles(path).FirstOrDefault(x => Path.GetFileName(x) == config);
            var cfg = new ReleaseNoteConfiguration(configPath);
            var kernel = new StandardKernel();
            kernel.Bind<ReleaseNoteConfiguration>().ToMethod(x => cfg);
            kernel.Load<ReleaseNoteModule>();
            var issueTracker = kernel.Get<IIssueTracker>();
            var sourceControl = kernel.Get<ISourceControl>();
            var releaseNoteLinker = kernel.Get<IReleaseNoteLinker>();
            var template = kernel.Get<ITemplate>();
            var issues = await issueTracker.GetIssues(id);
            var commits = await sourceControl.GetCommits(id);
            var releaseNoteModel = releaseNoteLinker.Link(commits, issues);
            var output = template.Build(id, releaseNoteModel);

            return View(new ReleaseNoteOutputViewModel() { Html = output});
        }

        public ActionResult ReleaseNote(string id, string config)
        {
            ViewBag.Url = Url.Action("Release", new {id = id, config = config});
            return View();
        }
    }
}