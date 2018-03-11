using System.Diagnostics;

namespace RWebService.Logic
{
    public class ProcessExt : Process
    {
        /// <summary>
        /// Gets or sets a value indicating whether to suppress StandardOutput.
        /// </summary>
        public bool NoOutput { get; set; }

        /// <summary>
        /// Gets or sets StandardOutput saver as an array of strings.
        /// </summary>
        public string[] Output { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessExt"/> class.
        /// </summary>
        public ProcessExt()
        {
        }

        /// <summary>
        /// Starts the process and waits for exit.
        /// </summary>
        public void StartAndWait()
        {
            this.Output = null;
            StartInfo.RedirectStandardOutput = !NoOutput;
            StartInfo.RedirectStandardError = !NoOutput;

            var outputBuilder = new OutputBuilder(NoOutput);
            OutputDataReceived += (sender, eventArgs) => outputBuilder.Add(eventArgs.Data);
            ErrorDataReceived += (sender, eventArgs) => outputBuilder.Add(eventArgs.Data);

            Start();

            if (!NoOutput)
            {
                BeginOutputReadLine();
                BeginErrorReadLine();
            }

            WaitForExit();

            if (!NoOutput)
            {
                CancelOutputRead();
                CancelErrorRead();
            }

            this.Output = outputBuilder.Output;
        }

        /// <summary>
        /// Creates an instance of extended Process class
        /// </summary>
        /// <returns>Extended Process class</returns>
        public static ProcessExt Create()
        {
            var process = new ProcessExt();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;

            return process;
        }
    }
}
