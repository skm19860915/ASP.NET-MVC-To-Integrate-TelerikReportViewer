using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BOL.Repository
{
    public class LogRepository
    {
        public static void LogException(Exception ex, string AdditionalInfo = "", string SourceType = "", int ErrorNumber = 0, int LineNumber = 0,
            int UserID = 0, int Severity = 0)
        {
            try
            {
                StringBuilder msg = new StringBuilder("");
                if (ex != null)
                {
                    msg.Append(ex.Message);

                    if (ex.InnerException != null)
                    {
                        msg.Append("-----INNER EX-----").Append(ex.InnerException.Message);

                        if (ex.InnerException.InnerException != null)
                        {
                            msg.Append("-----INNER INNER EX-----").Append(ex.InnerException.InnerException.Message);
                        }
                    }
                }

                StringBuilder trace = new StringBuilder( ex.StackTrace);
                if (!string.IsNullOrWhiteSpace(AdditionalInfo))
                {
                    trace.Append("-----ADDITIONAL INFO-----").Append(AdditionalInfo);
                }


                var log = new errorlog()
                {
                    datecreated = DateTime.Now,
                    errorsource = ex.Source,
                    sourcetype = SourceType,
                    errornumber = ErrorNumber,
                    linenumber = LineNumber,
                    userid = UserID,
                    severity = Severity,
                    errormessage = msg.ToString(),
                    errordescription = trace.ToString()
                };

                InsertErrorLog(log);
            } catch (Exception _ex) { }
        }

        public static void InsertErrorLog(errorlog ErrorLog)
        {
            try
            {
                using (ePontiv2Entities db = new ePontiv2Entities())
                {
                    db.errorlog.Add(ErrorLog);
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { }
        }
    }

    //Extension for Exception Logging
    public static class LogExceptionExtension
    {
        public static void Log(this Exception Ex, string AdditionalInfo = "", string SourceType = "", int ErrorNumber = 0, int LineNumber = 0,
            int UserID = 0, int Severity = 0)
        {
            LogRepository.LogException(Ex, AdditionalInfo, SourceType, ErrorNumber, LineNumber, UserID, Severity);
        }
    }


}
