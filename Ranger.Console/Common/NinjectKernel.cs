using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Ranger.Console.Common
{
    class NinjectKernel
    {
        private static object _lock = new object();
        private static IKernel _instance;

        public static IKernel Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new StandardKernel();
                        }
                    }
                }
                return _instance;
            }
        }

    }
}
