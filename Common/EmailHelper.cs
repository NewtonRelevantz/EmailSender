using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;

namespace EmailSenderProgram.Common
{
    public static class EmailHelper
    {
        public static void SendEmail(List<string> To, List<string> CC, string subject, string body)
        {
            try
            {
                LogHelper.LogInformation("EmailHelper=>SendEmail=> Method Initiated.");
                if (To != null && To.Any())
                {
                    To.ForEach(e =>
                    {
                        if (e.IsValidEmail())
                        {
                            //Create a new MailMessage
                            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
                            //Add customer to reciever list
                            m.To.Add(e);
                            if (CC != null && CC.Any())
                            {
                                CC.ForEach(p =>
                                {
                                    if (e.IsValidEmail())
                                    {
                                        m.CC.Add(p);
                                    }
                                    else
                                    {
                                        LogHelper.LogInformation($"EmailHelper=>SendEmail=> one of the CC email is empty or not valid=> EmailId=>{e}");
                                    }
                                });
                            }
                            //Add subject
                            m.Subject = subject;
                            //Send mail from info@EO.com
                            m.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings.GetValues("senderemail")?.FirstOrDefault()?.ToString());
                            //Add body to mail
                            m.Body = "Hi " + e.Split('@')?.FirstOrDefault() +
                                     "<br>" + body + "<br><br>Best Regards,<br>EO Team";
                            m.IsBodyHtml = true;
                            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings.GetValues("smptserver")?.FirstOrDefault()?.ToString());
                            client.Port = int.Parse(ConfigurationManager.AppSettings.GetValues("smptport")?.FirstOrDefault()?.ToString());
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings.GetValues("senderemail")?.FirstOrDefault()?.ToString(), ConfigurationManager.AppSettings.GetValues("password")?.FirstOrDefault()?.ToString());
                            client.EnableSsl = true;
                            //Send mail
                            client.Send(m);
                            client.Dispose();
                            LogHelper.LogInformation($"EmailHelper=>SendEmail=> Mail send successfully=> EmailId=>{e}");
                        }
                        else
                        {
                            LogHelper.LogInformation($"EmailHelper=>SendEmail=> one of the TO email is empty or not valid=> EmailId=>{e}");
                        }


                    });
                }
                else
                {
                    LogHelper.LogInformation("EmailHelper=>SendEmail=> unable to send email because of no receipients are found");
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError($"{ex.Message}=> Stack Trace=>{ex.StackTrace}");
                throw;
            }
        }

        private static bool IsValidEmail(this string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
    }
}
