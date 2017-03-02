using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Ranger.NetCore.Plugins
{
    public class DirectoryExistsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (!(value is string)) return false;

            var dir = Path.GetDirectoryName(value.ToString());
            return Directory.Exists(dir);
        }
    }
}