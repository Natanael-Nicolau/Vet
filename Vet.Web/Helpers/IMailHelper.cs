using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Helpers
{
    public interface IMailHelper
    {
        /// <summary>
        /// Sends an email using the email credentials in the appsettings.json file 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        void SendMail(string to, string subject, string body);
    }
}
