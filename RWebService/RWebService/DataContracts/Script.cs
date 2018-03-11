namespace RWebService
{
    public class Script
    {
        public string Route { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Interpreter { get; set; }

        public string Location { get; set; }

        public bool CopyToTemp { get; set; }

        public bool NoOutput { get; set; }

        public string OutputFormat { get; set; }

        public string OutputFile { get; set; }
    }
}
