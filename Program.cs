using EmailSenderProgram.Common;
using System;
using EmailSenderProgram.Repository.Interface;
using EmailSenderProgram.Repository.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailSenderProgram
{
    internal class Program
    {

        /// <summary>
        /// This application is run everyday
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            LogHelper.LogInformation("Email Trigger Initiated.");
            try
            {
                var serviceProvider = new ServiceCollection()
            .AddTransient<ICustomerEmail, CustomerEmail>()
            .BuildServiceProvider();
                var customerService = serviceProvider.GetRequiredService<ICustomerEmail>();
                LogHelper.LogInformation("Email Trigger CustomerCreateAlert Initiated.");
                customerService.CustomerCreateAlert();
                LogHelper.LogInformation("Email Trigger CustomerCreateAlert ended.");
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                {
                    LogHelper.LogInformation("Email Trigger ComeBackMail Initiated.");
                    customerService.ComeBackMail();
                    LogHelper.LogInformation("Email Trigger ComeBackMail ended.");
                }
                LogHelper.LogInformation("Email Trigger ended.");
            }

            catch (Exception ex)
            {
                LogHelper.LogError($"{ex.Message}=> Stack Trace=>{ex.StackTrace}");
            }
        }


    }
}