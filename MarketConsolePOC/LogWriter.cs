using System;
using System.IO;
using System.Text;


namespace FixOrderConsole
{

    public static class LogWriter
    {

        public static readonly object ObjLock = new object();
        public static DateTime LastTraceDate;
        public static DateTime LastErrorDate;
        private static string errorLogFilePath;
        private static string traceLogFilePath;
        private static string errorLogFileName;
        private static string traceLogFileName;

        public static void WriteErrorLog(Exception ex, string methodName)
        {
           
            var sb = new StringBuilder();
            try
            {
                sb.AppendLine("-------------------------------------------------------------------------------------------------------------");
                sb.AppendLine("DateTime    : " + DateTime.Now.ToString("ddMMMyy HH:mm:ss fff"));
                sb.AppendLine("Method      : " + methodName);
                sb.AppendLine("Message     : " + ex.Message);
                sb.AppendLine("Source	    : " + ex.Source);
                sb.AppendLine("StackTrace  : " + ex.StackTrace);
                sb.AppendLine("TargetSite  : " + ex.TargetSite);
                sb.AppendLine("_____________________________________________________________________________________________________________");


                Console.WriteLine(ex.Message);                
                WriteErrorLogToFile(sb.ToString());

            }
            catch (IOException ioex)
            {
                Console.WriteLine("Error while writing ErrorLog :" + ioex.Message);
            }
            catch (Exception ex1)
            {
                Console.WriteLine("Error while writing ErrorLog :" + ex1.Message);
            }

        }

        

        public static void WriteTraceLog(string sTrace)
        {
            try
            {
                //string sDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm ss");
                //sTrace = $"{sDate}\t{sTrace}";
                Console.WriteLine(sTrace);
                WriteTraceLogToFile(sTrace);
            }
            catch (IOException ioex)
            {
                Console.WriteLine("Error while writing ErrorLog :" + ioex.Message);
            }
            catch (Exception ex1)
            {
                Console.WriteLine("Error while writing ErrorLog :" + ex1.Message);
            }
        }


        private static void WriteErrorLogToFile(string msg)
        {
            try
            {
                lock (ObjLock)
                {
                    if (string.IsNullOrEmpty(errorLogFilePath)) errorLogFilePath = GetFilePath(LoggerLogType.Error);

                    if (LastErrorDate == DateTime.MinValue || LastErrorDate.Day != DateTime.Now.Day)
                    {
                        LastErrorDate = DateTime.Now;
                        errorLogFileName = GetFileName(LoggerLogType.Error);
                    }

                    CreateDirectory(errorLogFilePath);
                    CreateFile(errorLogFilePath, errorLogFileName);
                    using (StreamWriter swLog = File.AppendText(errorLogFilePath + errorLogFileName))
                    {
                        swLog.WriteLine(DateTime.Now + " : " + msg);
                    }
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private static void WriteTraceLogToFile(string msg)
        {
            try
            {
                lock (ObjLock)
                {
                    if (string.IsNullOrEmpty(traceLogFilePath)) traceLogFilePath = GetFilePath(LoggerLogType.Trace);
                    if (LastTraceDate == DateTime.MinValue || LastTraceDate.Day != DateTime.Now.Day)
                    {
                        LastTraceDate = DateTime.Now;
                        traceLogFileName = GetFileName(LoggerLogType.Trace);
                    }
                    CreateDirectory(traceLogFilePath);
                    CreateFile(traceLogFilePath, traceLogFileName);
                    using (StreamWriter swLog = File.AppendText(traceLogFilePath + traceLogFileName))
                    {
                        swLog.WriteLine(DateTime.Now + " : " + msg);
                    }
                }

            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CreateDirectory(string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
        }
        private static void CreateFile(string filePath, string fileName)
        {
            if (!File.Exists(filePath + fileName))
                File.Create(filePath + fileName).Close();
        }
        private static string GetFilePath(LoggerLogType logType)
        {
            string path = string.Empty;
            DirectoryInfo diExecutingAssembly = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (diExecutingAssembly != null && diExecutingAssembly.Exists)
            {
                if (logType == LoggerLogType.Error) path = @"\\Dump\\Error\\"; else if (logType == LoggerLogType.Trace) path = @"\\Dump\\Trace\\";
                string currentApplicationPath = diExecutingAssembly.FullName + path;
                CreateDirectory(currentApplicationPath);
                return currentApplicationPath;
            }
            return string.Empty;
        }
        private static string GetFileName(LoggerLogType logType)
        {
            if (logType == LoggerLogType.Error)
            {
                return "Error_" + LastErrorDate.ToString("ddMMMyy") + ".log";
            }
            else if (logType == LoggerLogType.Trace)
            {
                return "Trace_" + LastTraceDate.ToString("ddMMMyy") + ".log";
            }
            return string.Empty;
        }

        public enum LoggerLogType
        {
            Error,
            Trace
        }

    }


}
