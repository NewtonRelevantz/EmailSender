using System;
using System.IO;

namespace EmailSenderProgram.Common
{
    public static class LogHelper
    {
        private static string LogPath = $@"log\{DateTime.Now.ToString("yyyyMMdd")}.txt";
        public static void LogInformation(string Message)
        {
            checkLog();
            using (StreamWriter writer = File.AppendText(LogPath))
            {
                writer.WriteLine($"{DateTime.Now}: {Message}");
            }
        }


        public static void LogError(string Message)
        {
            checkLog();
            using (StreamWriter writer = File.AppendText(LogPath))
            {
                writer.WriteLine($"Failed at {DateTime.Now}: {Message}");
            }
        }

        private static void checkLog()
        {
            if (!File.Exists(LogPath))
            {
                var dir = System.IO.Path.GetDirectoryName(
           System.Reflection.Assembly.GetEntryAssembly().Location);
                string folderPath = System.IO.Path.Combine(dir, "log");

                // Check if the folder already exists
                if (!Directory.Exists(folderPath))
                {
                    // Create the folder
                    Directory.CreateDirectory(folderPath);
                }

                using (StreamWriter writer = File.CreateText(LogPath))
                {
                }

            }
        }
    }
}
