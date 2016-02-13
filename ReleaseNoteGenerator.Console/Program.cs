using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseNoteGenerator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            new ApplicationBootstrapper(new ReleaseNoteGeneratorConsoleApplication())
                .ConfigureLogging()
                .ExitOn(ConsoleKey.Enter)
                .Start(args);
        }
    }
}
