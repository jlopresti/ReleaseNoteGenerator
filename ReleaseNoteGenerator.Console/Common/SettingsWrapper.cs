namespace ReleaseNoteGenerator.Console.Common
{
    public class SettingsWrapper<T> : IVerboseParameter, ISilentParameter
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
                var v = _settings as IVerboseParameter;
                return v != null && v.Verbose;
            }
        }

        public bool Silent
        {
            get
            {
                var v = _settings as ISilentParameter;
                return v != null && v.Silent;
            }
        }

        public T Value { get { return _settings; } }

    }
}