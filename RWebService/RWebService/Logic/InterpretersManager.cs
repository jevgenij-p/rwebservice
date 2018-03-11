using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace RWebService.Logic
{
    public class InterpretersManager : IInterpretersManager
    {
        private readonly ILogger logger;

        private InterpreterSettings interpreterSettings { get; set; }

        public string WorkingDirectory
        {
            get
            {
                return interpreterSettings.WorkingDirectory;
            }
        }

        public InterpretersManager(ILogger<InterpretersManager> logger, IOptions<InterpreterSettings> interpreterSettings)
        {
            this.logger = logger;
            this.interpreterSettings = interpreterSettings.Value;
        }

        public List<Interpreter> GetInterpreters()
        {
            return interpreterSettings.Interpreters;
        }

        public Interpreter GetInterpreter(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                logger.LogError("Interpreter name is empty");
                return null;
            }

            var interpreter = interpreterSettings?.Interpreters.Where(x => string.Compare(x.Name, name, ignoreCase: true) == 0).FirstOrDefault();
            if (interpreter == null)
            {
                logger.LogError($"Interpreter \"{name}\" not found");
                return null;
            }

            logger.LogInformation($"Interpreter: {name}. Options: {interpreter.Options}");

            return interpreter;
        }
    }
}
