using System.Collections.Generic;

namespace RWebService
{
    public class InterpreterSettings
    {
        public List<Interpreter> Interpreters { get; set; }

        public string WorkingDirectory { get; set; }
    }
}
