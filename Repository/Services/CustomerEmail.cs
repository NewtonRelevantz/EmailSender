using EmailSenderProgram.Common;
using EmailSenderProgram.Repository.Interface;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System;

namespace EmailSenderProgram.Repository.Services
{
    public class CustomerEmail : ICustomerEmail
    {
        /// <summary>
        /// Send Welcome mail
        /// </summary>
        /// <returns></returns>
        public void CustomerCreateAlert()
        {
            try
            {
                string subject = ConfigurationManager.AppSettings.GetValues("CustomerCreateAlertSubject")?.FirstOrDefault()?.ToString();
                string body = ConfigurationManager.AppSettings.GetValues("CustomerCreateAlertBody")?.FirstOrDefault()?.ToString();
                //List all customers
                List<Customer> customersinfo = DataLayer.ListCustomers();
                if (customersinfo != null && customersinfo.Any())
                {
                    var validcustomers = customersinfo.Where(p => p.CreatedDateTime.Day > DateTime.Now.AddDays(-1).Day);
                    if (validcustomers != null && validcustomers.Any())
                    {
                        EmailHelper.SendEmail(validcustomers.Select(p => p.Email).ToList(), null, subject, body);
                    }
                    else
                    {
                        LogHelper.LogInformation("CustomerCreateAlert=>No customers are foun for email trigger.");
                    }
                }
                else
                {
                    LogHelper.LogInformation("CustomerCreateAlert=>No customers are found.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError($"{ex.Message}=> Stack Trace=>{ex.StackTrace}");
            }
        }

        /// <summary>
        /// Send Customer OrderMail
        /// </summary>
        /// <returns></returns>
        public void ComeBackMail()
        {
            try
            {
                //this information has to pick from configuration/database
                string Voucher = ConfigurationManager.AppSettings.GetValues("Voucher")?.FirstOrDefault()?.ToString();
                //this information has to pick from configuration
                string subject = ConfigurationManager.AppSettings.GetValues("ComeBackMailSubject")?.FirstOrDefault()?.ToString();
                string body = $"{ConfigurationManager.AppSettings.GetValues("ComeBackMailBody")?.FirstOrDefault()?.ToString()} <br> Voucher: {Voucher}";
                //this information has to pick from configuration
                int comebackorderdays = int.Parse(ConfigurationManager.AppSettings.GetValues("comebackorderdays")?.FirstOrDefault()?.ToString());

                List<string> toemail = new List<string>();

                //noteligiblecustomer
                List<Order> noteligiblecustomer = DataLayer.ListOrders().Where(p => p.OrderDatetime.Date > DateTime.Now.AddDays(-comebackorderdays).Date)?.ToList();

                //All customers
                List<Customer> customers = DataLayer.ListCustomers();

                if (noteligiblecustomer != null && noteligiblecustomer.Any())
                {
                    toemail = customers?.Select(p => p.Email)?.Distinct()?.ToList()?.Except(noteligiblecustomer.Select(p => p.CustomerEmail)).ToList();
                }
                else
                {
                    toemail = customers?.Select(p => p.Email)?.Distinct()?.ToList();
                }
                if (toemail != null && toemail.Any())
                {
                    EmailHelper.SendEmail(toemail, null, subject, body);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError($"{ex.Message}=> Stack Trace=>{ex.StackTrace}");
            }
        }

    }
}
