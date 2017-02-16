using Vlast.Util.Instrumentation;
using Vlast.Util.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vlast.Util.Aws;
using Amazon.SimpleEmail.Model;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
 

namespace Vlast.Broker.EMAIL
{
    public class EmailDispatcher
    {

        /// <summary>
        /// Envia um email para os destinatarios
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool SendEmail(string from, string subject, List<string> toList, string body, string fromDisplayName = null, byte[] attachment = null, string contentType = null)
        {
            bool sent = false;
            try
            {

                if (string.IsNullOrWhiteSpace(subject) || toList == null || toList.Count > 45 || string.IsNullOrWhiteSpace(body))
                {
                    ServiceHelper.ThrowError("INVALID_MESSAGE_PARAMETERS: " + subject + ", " + toList + ", " + (body.Length > 100 ? body.Substring(0, 100) : body));
                }

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(from);
                if (!string.IsNullOrWhiteSpace(fromDisplayName))
                {
                    mailMessage.From = new MailAddress(from, fromDisplayName);
                }
                else
                {
                    mailMessage.From = new MailAddress(from);
                }

                mailMessage.Subject = subject;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
              
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
				htmlView.ContentType.CharSet = Encoding.UTF8.WebName;
                mailMessage.AlternateViews.Add(htmlView);


                RawMessage rawMessage = new RawMessage();

                using (MemoryStream memoryStream = BuildRawMailHelper.ConvertMailMessageToMemoryStream(mailMessage))
                {
                    rawMessage.Data = memoryStream;
                }

                SendRawEmailRequest request = new SendRawEmailRequest();
                request.RawMessage = rawMessage;

                request.Destinations = toList;
                request.Source = from;

                SendRawEmailResponse response = AWSFactory.SESClient.SendRawEmail(request);
                return true;
            }
            catch (Exception ex)
            {
                sent = false;
                Logger.LogException(ex);
            }
            return sent;
        }
    }

    public class BuildRawMailHelper
    {
        static Type mailWriterType;
        static ConstructorInfo mailWriterContructor;
        static MethodInfo sendMethod;
        static MethodInfo closeMethod;

        static BuildRawMailHelper()
        {
            Assembly assembly = typeof(SmtpClient).Assembly;

            mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Stream) }, null);
            sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);

        }

        public static MemoryStream ConvertMailMessageToMemoryStream(MailMessage message)
        {
            MemoryStream fileStream = new MemoryStream();
            object mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });
            sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { mailWriter, true, true }, null);
            MethodInfo closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);
            closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);
            return fileStream;
        }

    }
}
