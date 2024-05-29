using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendMail(Email email)
		{
			/*
			 * كل الاعدادات الي انت عايزها هو قايلك عليها مباشر 
			 */
			var Client = new SmtpClient("smtp.gmail.com" , 587); //بتاعك Host ده ال
			Client.EnableSsl = true; //علشان خاطر يعمل تشفير للايميل بتاعي  SSl لل Enaple قولتله يعمل 
			Client.Credentials = new NetworkCredential("msakr012041383@gmail.com", "bdwyyadmvyrcgwgh"); // 
			Client.Send("msakr012041383@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
