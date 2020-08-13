using CreateSeveralFile.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CreateSeveralFile
{
    internal class Program
    {
        private static void Main()
        {
            Action<string> display = Console.WriteLine;
            var listeServeurs = new List<string>();
            string line = string.Empty;
            if (Settings.Default.ServerListFileName.Trim() == string.Empty)
            {
                Settings.Default.ServerListFileName = "serverList.txt";
                Settings.Default.Save();
            }

            if (!File.Exists(Settings.Default.ServerListFileName))
            {
                using (StreamWriter streamWriter = new StreamWriter(Settings.Default.ServerListFileName))
                {
                    streamWriter.WriteLine("Server1");
                    streamWriter.WriteLine("Server2");
                    streamWriter.WriteLine("Server3");
                }
            }

            using (StreamReader sr = new StreamReader(Settings.Default.ServerListFileName))
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    listeServeurs.Add(line);
                }
            }

            string pattern = "*.sql";
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string[] listOfScripts = Directory.GetFiles(currentDirectory, pattern, SearchOption.TopDirectoryOnly);
            int numberOfSQLScripts = listOfScripts.Length;
            string[] listScriptName = new string[listOfScripts.Length];
            int numberOfDirectories = currentDirectory.Split('\\').Length;
            for (int i = 0; i < listOfScripts.Length; i++)
            {
                listScriptName[i] = listOfScripts[i].Split('\\')[numberOfDirectories];
            }

            foreach (string server in listeServeurs)
            {
                string serverName = server;
                string firstLine = $"USE {server}";
                for (int i = 0; i < listScriptName.Length; i++)
                {
                    string scriptName = listScriptName[i];
                    string scriptContent = string.Empty;
                    using (StreamReader sr = new StreamReader(scriptName))
                    {
                        scriptContent = sr.ReadToEnd();
                    }

                    string newScriptName = $"{serverName}-{scriptName}";
                    if (File.Exists(newScriptName))
                    {
                        File.Delete(newScriptName);
                    }

                    using (StreamWriter sw = new StreamWriter(newScriptName, false))
                    {
                        sw.WriteLine(firstLine);
                        sw.Write(scriptContent);
                    }
                }
            }

            display(string.Empty);
            display($"{numberOfSQLScripts} SQL file{Pluralize(numberOfSQLScripts)} {Pluralize(numberOfSQLScripts, "have")} been copied for each server name in the {Settings.Default.ServerListFileName} file.");
            display(string.Empty);
            display("Press any key to exit:");
            Console.ReadKey();
        }

        public static string Pluralize(int number, string irregularNoun = "")
        {
            switch (irregularNoun)
            {
                case "":
                    return number > 1 ? "s" : string.Empty;
                case "al":
                    return number > 1 ? "aux" : "al";
                case "au":
                    return number > 1 ? "aux" : "au";
                case "eau":
                    return number > 1 ? "eaux" : "eau";
                case "eu":
                    return number > 1 ? "eux" : "eu";
                case "landau":
                    return number > 1 ? "landaus" : "landau";
                case "sarrau":
                    return number > 1 ? "sarraus" : "sarrau";
                case "bleu":
                    return number > 1 ? "bleus" : "bleu";
                case "émeu":
                    return number > 1 ? "émeus" : "émeu";
                case "lieu":
                    return number > 1 ? "lieux" : "lieu";
                case "pneu":
                    return number > 1 ? "pneus" : "pneu";
                case "aval":
                    return number > 1 ? "avals" : "aval";
                case "bal":
                    return number > 1 ? "bals" : "bal";
                case "chacal":
                    return number > 1 ? "chacals" : "chacal";
                case "carnaval":
                    return number > 1 ? "carnavals" : "carnaval";
                case "festival":
                    return number > 1 ? "festivals" : "festival";
                case "récital":
                    return number > 1 ? "récitals" : "récital";
                case "régal":
                    return number > 1 ? "régals" : "régal";
                case "cal":
                    return number > 1 ? "cals" : "cal";
                case "serval":
                    return number > 1 ? "servals" : "serval";
                case "choral":
                    return number > 1 ? "chorals" : "choral";
                case "narval":
                    return number > 1 ? "narvals" : "narval";
                case "bail":
                    return number > 1 ? "baux" : "bail";
                case "corail":
                    return number > 1 ? "coraux" : "corail";
                case "émail":
                    return number > 1 ? "émaux" : "émail";
                case "soupirail":
                    return number > 1 ? "soupiraux" : "soupirail";
                case "travail":
                    return number > 1 ? "travaux" : "travail";
                case "vantail":
                    return number > 1 ? "vantaux" : "vantail";
                case "vitrail":
                    return number > 1 ? "vitraux" : "vitrail";
                case "bijou":
                    return number > 1 ? "bijoux" : "bijou";
                case "caillou":
                    return number > 1 ? "cailloux" : "caillou";
                case "chou":
                    return number > 1 ? "choux" : "chou";
                case "genou":
                    return number > 1 ? "genoux" : "genou";
                case "hibou":
                    return number > 1 ? "hiboux" : "hibou";
                case "joujou":
                    return number > 1 ? "joujoux" : "joujou";
                case "pou":
                    return number > 1 ? "poux" : "pou";
                case "est":
                    return number > 1 ? "sont" : "est";

                // English
                case " is":
                    return number > 1 ? "s are" : " is"; // with a space before
                case "is":
                    return number > 1 ? "are" : "is"; // without a space before
                case "are":
                    return number > 1 ? "are" : "is"; // without a space before
                case "has":
                    return number > 1 ? "have" : "has";
                case "have":
                    return number > 1 ? "have" : "has";
                case "The":
                    return "The"; // CAPITAL, useful because of French plural
                case "the":
                    return "the"; // lower case, useful because of French plural
                default:
                    return number > 1 ? "s" : string.Empty;
            }
        }
    }
}
