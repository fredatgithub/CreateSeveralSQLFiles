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
            display($"All SQL files have been copied for each server name in the {Settings.Default.ServerListFileName} file.");
            display(string.Empty);
            display("Press any key to exit:");
            Console.ReadKey();
        }
    }
}
