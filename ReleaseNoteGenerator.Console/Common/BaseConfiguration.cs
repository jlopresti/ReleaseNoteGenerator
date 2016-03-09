using CommandLine;

namespace ReleaseNoteGenerator.Console.Common
{
    public abstract class BaseConfiguration<T> : IConsoleApplicationConfiguration where T : new()
    {
        protected SettingsWrapper<T> Settings { get; set; }

        protected BaseConfiguration()
        {
            Settings = new SettingsWrapper<T>(new T());
        }
        public bool Verbose { get { return Settings.Verbose; } }
        public bool Silent { get { return Settings.Silent; } }

        public bool LoadConfig(string[] args)
        {
            if (Parser.Default.ParseArguments(args, Settings.Value))
            {
                return ValidateConfig(args);
            }

            return false;
        }

        protected abstract bool ValidateConfig(string[] args);
    }
}