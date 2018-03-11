using System.Collections.Generic;

namespace RWebService
{
    public interface IInterpretersManager
    {
        string WorkingDirectory { get; }

        List<Interpreter> GetInterpreters();

        Interpreter GetInterpreter(string name);
    }
}
