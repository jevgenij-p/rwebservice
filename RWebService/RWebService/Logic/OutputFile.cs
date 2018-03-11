using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace RWebService.Logic
{
    public class OutputFile
    {
        private readonly Script script;

        public string FileFormat { get; private set; }

        public string FileName { get; private set; }

        public bool IsDefined
        {
            get { return !string.IsNullOrWhiteSpace(FileFormat); }
        }

        public bool IsJson
        {
            get { return FileFormat == "json"; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFile"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        public OutputFile(Script script)
        {
            this.script = script;
            FileFormat = !string.IsNullOrEmpty(script.OutputFormat) ? script.OutputFormat.ToLower() : string.Empty;
            FileName = script.OutputFile ?? "output." + FileFormat;
        }

        /// <summary>
        /// Reads JSON output file
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>Deserialize object</returns>
        public object ReadJson(string path)
        {
            string jsonText = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject(jsonText);

            return result;
        }

        /// <summary>
        /// Reads output file stream
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns>FileStreamResult</returns>
        public FileStreamResult ReadFileStream(string path)
        {
            string mime = MimeTypes.GetMimeType(FileName);
            return new FileStreamResult(new FileStream(path, FileMode.Open), mime);
        }
    }
}
