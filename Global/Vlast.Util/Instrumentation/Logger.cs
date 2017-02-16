using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Util.Instrumentation
{
 
    public class Logger
    {
        private static readonly string SOURCE = "ASP.NET 4.0.30319.0";
        private static readonly string LOG_NAME = "Application";

        public static void LogInfo(string msg)
        {
            WriteLog(msg, tipo: EventLogEntryType.Error);
        }

        public static void LogError(string msg, Exception ex)
        {
            WriteLog(msg, ex);
        }

        public static void LogError(string msg)
        {
            WriteLog(msg);
        }

        public static void LogException(Exception ex)
        {
            WriteLog(ex.Message, ex);
        }


        private static void WriteLog(string msg, Exception ex = null, EventLogEntryType tipo = EventLogEntryType.Error)
        {
            try
            {
                if (string.IsNullOrEmpty(msg))
                    msg = "";

              /*  try
                {
                    // Creating the source, which will generate error
                    if (!EventLog.SourceExists(SOURCE))
                    {
                        //Creating log, where error will be logged
                        EventLog.CreateEventSource(SOURCE, LOG_NAME);
                    }
                }
                catch { }*/

                if (ex != null)
                {
                    msg += Environment.NewLine + Environment.NewLine + ex.ToString();
                }

                EventLog.WriteEntry(SOURCE, msg, tipo);
            }
            catch {
                Console.WriteLine(msg);
            }
        }
    }
}
