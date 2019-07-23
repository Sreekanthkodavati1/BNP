using System;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using JsonParser;


namespace BnPBaseFramework.IO.BatFiles
{
    public class BatFile_Common
    {
        public static Process proc = null;
        
        public static string result = string.Empty;
        public static string OutputFilePath = string.Empty;
        public static string  batfilepath = string.Empty;
        public static string FileProjectPth = ConfigurationManager.AppSettings["JsonSchema"];
        public static int exitcode = 0;

        /// <summary>
        /// Used to create batch file by taking type of file as input.
        /// </summary>
        /// <param name="typeOfFile">type of file</param>
        /// <returns>success or error message</returns>
        public static string CreateBatchFile(TypeOfFile typeOfFile)
        {
            string projectPath = string.Empty;
            Process process = null;
            
            try
            {

                string path = Directory.GetCurrentDirectory();
                var patharray = path.Split('\\');
                int length = patharray.Length;
                for (int i = 0; i <= length - 4; i++)
                {
                 projectPath = projectPath + patharray[i] + "\\";
                }
                projectPath = projectPath + @"BnPBaseFramework.IO";
                
                StringBuilder pushD, config, fileName, script;
                pushD = new StringBuilder();
                pushD.Append(projectPath);
                pushD.Append(ConfigurationManager.AppSettings[typeOfFile.ToString() + "PushD"]);
                pushD.Append("\n");
                config = new StringBuilder();
                config.Append(projectPath);
                config.Append(ConfigurationManager.AppSettings[typeOfFile.ToString() + "Config"]);
                fileName = new StringBuilder();
                fileName.Append(projectPath);
                fileName.Append(ConfigurationManager.AppSettings[typeOfFile.ToString() + "FileName"]);
                fileName.Append("\n");
                script = new StringBuilder();
                script.Append("pushd ");
                script.Append(pushD);
                script.Append("DAPConsoleProcessor.exe  /K -config ");
                script.Append(config);
                script.Append(" -filename ");
                script.Append(fileName);
               // script.Append("pause");
                File.WriteAllText(projectPath + @"\DapStuff\Brierley.DataAcquisition" + "\\" + typeOfFile.ToString() + "File.bat", script.ToString());
                result = "Success";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (process != null)
                    process.Close();
                result = ex.Message;
            }


            return result;
        }


        /// <summary>
        /// Used to create input text file by taking type of file, templateName and fileextension as input.
        /// </summary>
        /// <param name="typeOfFile","templateName","fileextension">type of file</param>
        /// <returns>success or error message</returns>
        public static string GenerateInputDataTextfile(TypeOfFile typeOfFile, string templateName, string fileextension)
        {
            string projectPath = string.Empty;
            string path = Directory.GetCurrentDirectory();
            var patharray = path.Split('\\');
            int length = patharray.Length;
            for (int i = 0; i <= length - 4; i++)
            { projectPath = projectPath + patharray[i] + "\\"; }
            projectPath = projectPath + @"BnPBaseFramework.IO";

            string schemaText = string.Empty;

            string result = "Error";
            StringBuilder jsonSchemaPath = new StringBuilder();
            string inputDataPath = string.Empty;
            try
            {
                if (ConfigurationManager.AppSettings["JsonSchema"] != null)
                {
                    jsonSchemaPath.Append(ConfigurationManager.AppSettings["JsonSchema"]);
                    
                    jsonSchemaPath.Append(@"\");
                    jsonSchemaPath.Append(typeOfFile.ToString() + @"\");
                    jsonSchemaPath.Append(typeOfFile.ToString() + @"_InputTextFiles");
                    jsonSchemaPath.Append(@"\");
                    jsonSchemaPath.Append(templateName);

                    if (File.Exists(jsonSchemaPath.ToString()))
                    {
                        string[] Input = File.ReadAllLines(jsonSchemaPath.ToString());
                        OutputFilePath = projectPath + @"\DapStuff\" + typeOfFile + ".txt";

                        if (File.Exists(OutputFilePath))
                        {
                            File.WriteAllLines(OutputFilePath, Input);
                            result = "Data Inserted into the file Successfully";
                        }
                        else
                        {
                            File.AppendAllLines(OutputFilePath, Input);
                            result = "Data Inserted into the file Successfully";

                        }
                    }

                }

            }
            catch (FileNotFoundException ex)
            {
                result = ex.Message;
            }


            return result;
        }




        /// <summary>
        /// Used to run bat file by taking batFile as input.
        /// </summary>
        /// <param name="batFile","templateName","fileextension">type of file</param>
        /// <returns>success or exception</returns>
        public static void RunBatFile(string batFile)
        {
            try
            {
                string projectPath = string.Empty;
                string path = Directory.GetCurrentDirectory();
                var patharray = path.Split('\\');
                int length = patharray.Length;
                for (int i = 0; i <= length - 4; i++)
                {
                 projectPath = projectPath + patharray[i] + "\\";
                }
                projectPath = projectPath + @"BnPBaseFramework.IO";

                batfilepath = projectPath + "\\" + @"DAPStuff\Brierley.DataAcquisition\";
                string _batDir = string.Format(batfilepath);
                proc = new Process();
                proc.StartInfo.WorkingDirectory = _batDir;
                proc.StartInfo.FileName = batFile;
                proc.StartInfo.CreateNoWindow = false;
                proc.Start();
                proc.WaitForExit(1000 * 60 * 1);
                exitcode = proc.ExitCode;
                proc.Close();
                Console.WriteLine("Bat file executed...");

            }
            catch (Exception ex)
            {
                proc.WaitForExit();
                exitcode = proc.ExitCode;
                proc.Close();
                Console.WriteLine(ex.Message);

            }

        }
    }
}
