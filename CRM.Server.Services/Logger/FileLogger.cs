
using System;
using System.IO;
using System.Reflection;

namespace CRM.Server.Services.Logger
{
    public enum LogType
    {
        Exception = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Success = 5,
        ExecutionSequence = 6,
        Data = 7
    }

    public sealed class FileLogger
    {
        private static readonly object padlock = new object();
        /* 
        * ensures that only one instance of the object is created 
        */
        private static Lazy<FileLogger> instance = null;

        /*
      * public property is used to return only one instance of the class
      * leveraging on the private property
      */
        public static FileLogger Instance
        {
            get
            {
                lock (padlock)
                {
                    //Eager initialization for thread safety
                    if (instance == null)
                        instance = new Lazy<FileLogger>(() => new FileLogger());
                    return instance.Value;
                }
            }
        }

        /*
      * Private constructor ensures that object is not
      * instantiated other than with in the class itself
      */
        private FileLogger()
        {
        }

        /*
    * Public method which can be invoked through the singleton instance to log to file
    */
        public void LogToFile(string message, LogType type = LogType.Exception)
        {
            var folderpath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\Logs\\{DateTime.Now.Date.ToString("yyyy.MM.dd")}";

            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }

            // Create a writer and open the file:
            //set your logger path to config
            var logFileWithPath = $"{folderpath}\\log-{type}-{DateTime.Now.Date.ToString("yyyy-MM-dd")}.txt";

            var log = !File.Exists(logFileWithPath) ? new StreamWriter(logFileWithPath) : File.AppendText(logFileWithPath);

            // Write to the file:
            log.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:m:ss")}-> { type } : {message}");

            // Close the stream:
            log.Close();
        }
    }
}

