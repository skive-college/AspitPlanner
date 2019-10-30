using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspitPlanner.Helpers
{
    public class ProgramFinder
    {
        public static List<Program> findAll()
        {
            string sti = @"O:\Aspit\Underviser\Tekniske filer\Kjeldtest";

            List<Program> programmer = EnumerateFiles(sti, true, ".exe").ToList();

            return programmer;
        }

        static IEnumerable<Program> EnumerateFiles(string path, bool recursive, params string[] extensions)
        {
            var files = Directory.EnumerateFiles(path, "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                if (extensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                {
                    string[] temp = file.Split('\\');
                    Program p = new Program()
                    {

                        Navn = temp[temp.Length - 1],
                        Sti = file.Substring(0, file.LastIndexOf("\\"))
                    };
                    yield return p;
                }

            };
        }
    }
}

