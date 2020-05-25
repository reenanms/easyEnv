using System;
using System.Collections.Generic;
using System.Text;

namespace EasyEnv
{
    public class ExecutorErrorException : Exception
    {
        public ExecutorErrorException(string program) : base($"{program} return an error") { }
    }
}
