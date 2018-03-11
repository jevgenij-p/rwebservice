using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RWebService.Helpers
{
    public static class Utilities
    {
        /// <summary>
        /// Copy a directory and its contents
        /// </summary>
        /// <param name="source">The source directory</param>
        /// <param name="target">The target directory</param>
        public static void CopyFiles(string source, string target)
        {
            var diSource = new DirectoryInfo(source);
            var diTarget = new DirectoryInfo(target);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// Creates a temporary directory in the specified directory
        /// </summary>
        /// <param name="dir">The directory</param>
        /// <returns>Temporary directory</returns>
        public static string CreateTempDirectory(string dir)
        {
            string tempDirectory = Path.Combine(dir, Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);

            return tempDirectory;
        }

        /// <summary>
        /// Saves POST request data as JSON-serialized collection
        /// </summary>
        /// <param name="request">Http request</param>
        /// <param name="path">The target path.</param>
        public static void SavePostRequestData(HttpRequest request, string path)
        {
            var list = new Dictionary<string, object>();
            foreach (string key in request.Form.Keys)
            {
                string val = request.Form[key];

                if (int.TryParse(val, out int intVal))
                {
                    list.Add(key, intVal);
                }
                else if (long.TryParse(val, out long longVal))
                {
                    list.Add(key, longVal);
                }
                else if(double.TryParse(val, out double doubleVal))
                {
                    list.Add(key, doubleVal);
                }
                else if (bool.TryParse(val, out bool boolVal))
                {
                    list.Add(key, boolVal);
                }
                else
                {
                    list.Add(key, val);
                }
            }

            var result = JsonConvert.SerializeObject(list, 
                new JsonSerializerSettings { Formatting = Formatting.Indented });

            File.WriteAllText(path, result);
        }
    }
}
