using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Ranger.NetCore.RazorHtml
{
    public class FileExistsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (!(value is string)) return false;

            return File.Exists(value.ToString());
        }
    }
}