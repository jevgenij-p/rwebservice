using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace RWebService.Logic
{
    public class ScriptsManager : IScriptsManager
    {
        private ScriptSettings scriptSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptsManager"/> class.
        /// </summary>
        /// <param name="scriptSettings">The script settings.</param>
        public ScriptsManager(IOptions<ScriptSettings> scriptSettings)
        {
            this.scriptSettings = scriptSettings.Value;
        }

        /// <summary>
        /// Gets a list of scripts
        /// </summary>
        /// <returns>List of scripts</returns>
        public List<Script> GetScripts()
        {
            return scriptSettings.Scripts;
        }
    }
}
