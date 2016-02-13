using System.Threading.Tasks;
using log4net;


namespace ReleaseNoteGenerator.Console
{
    internal class ReleaseNoteGeneratorConsoleApplication : IConsoleApplication
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(ReleaseNoteGeneratorConsoleApplication));
        public Task Run(string[] args)
        {
            _logger.Debug("Hello world");
            _logger.Info("Hello world");
            _logger.Warn("Hello world");
            _logger.Error("Hello world");
            _logger.Fatal("Hello world");

            return Task.FromResult(1);
        }
    }
}