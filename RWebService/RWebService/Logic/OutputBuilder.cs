using System.Collections.Generic;

namespace RWebService.Logic
{
    public class OutputBuilder
    {
        private bool noOutput = false;
        private List<string> builder = new List<string>();

        /// <summary>
        /// Gets an array of strings.
        /// </summary>
        public string[] Output
        {
            get
            {
                return builder.ToArray();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputBuilder"/> class.
        /// </summary>
        public OutputBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputBuilder"/> class.
        /// </summary>
        /// <param name="noOutput">Do not generate output if set to True.</param>
        public OutputBuilder(bool noOutput)
        {
            this.noOutput = noOutput;
        }

        /// <summary>
        /// Adds the specified string to the list.
        /// </summary>
        /// <param name="str">The string.</param>
        public void Add(string str)
        {
            if (!noOutput && str != null)
                builder.Add(str);
        }
    }
}
