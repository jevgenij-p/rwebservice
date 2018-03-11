using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace RWebService.Logic
{
    public static class ArgumentsBuilder
    {
        /// <summary>
        /// Builds process arguments, concatinating interpreter's options, script name and script options.
        /// The result could be like: "--vanilla test.R -out 'some.txt'"
        /// </summary>
        /// <param name="interpreter">The interpreter</param>
        /// <param name="script">The script</param>
        /// <param name="request">Http request</param>
        /// <returns>Process arguments</returns>
        public static string Build(Interpreter interpreter, Script script, HttpRequest request)
        {
            var arguments = new StringBuilder();
            if (!string.IsNullOrEmpty(interpreter.Options))
            {
                arguments.Append(interpreter.Options);
                arguments.Append(" ");
            }

            arguments.Append(script.Name);

            if (request.Method == "GET")
            {
                string options = BuildScriptOptionsFromRequest(request);
                if (!string.IsNullOrEmpty(options))
                {
                    arguments.Append(" ");
                    arguments.Append(options);
                }
            }

            return arguments.ToString();
        }

        private static string BuildScriptOptionsFromRequest(HttpRequest request)
        {
            if (request.Query.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var key in request.Query.Keys)
                {
                    sb.AppendFormat("--{0} ", key);
                    if (request.Query.TryGetValue(key, out StringValues value))
                    {
                        string str = value.ToString();
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (str.Contains(" "))
                                str = "\"" + str + "\"";

                            sb.AppendFormat("{0} ", str);
                        }
                    }
                }

                return sb.ToString().TrimEnd();
            }

            return null;
        }
    }
}
