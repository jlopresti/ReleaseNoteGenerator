namespace ReleaseNoteGenerator.Console.Common
{
    public class SettingsWrapper<T> : IVerbose
    {
        private readonly T _settings;

        public SettingsWrapper(T settings)
        {
            _settings = settings;
        }

        public bool Verbose
        {
            get
            {
                var v = _settings as IVerbose;
                return v != null && v.Verbose;
            }
        }

        public T Value { get { return _settings; } }
    }
}