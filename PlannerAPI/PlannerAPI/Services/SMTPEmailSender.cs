using MimeKit;
using MailKit.Net.Smtp;

namespace PlannerAPI.Services {
    public class SMTPEmailSender {
        private readonly MimeMessage _message;
        private readonly SmtpClient _smtpClient;
        private readonly BodyBuilder _bodyBuilder;
        public SMTPEmailSender(MimeMessage message, SmtpClient smtpClient, BodyBuilder bodyBuilder) {
            _message = message;
            _smtpClient = smtpClient;
            _bodyBuilder = bodyBuilder; 
        }

        public void SendVerificationEmail(string firstName, string lastName, string email, string code) {
            //TODO CHANGE FROM ADDRESS
            //TODO CHANGE Image Address when testing and in production
            //baseUrl should be the url of the UI
            string baseUrl = "http://localhost:3000/verify/";
            
            //Creating the message
            _message.From.Add(new MailboxAddress("Backpack", "kdu2030@gmail.com"));
            _message.To.Add(new MailboxAddress($"{firstName} {lastName}", email));
            _message.Subject = $"Hi {firstName}! Welcome to Backpack!";
            
            //Modifying the body of the email
            _bodyBuilder.HtmlBody = ModifyEmailHTML("Static/VerifyTemplate.html", "[verify_link]", baseUrl + code);
            //Adding the images in the email template
            _bodyBuilder.Attachments.Add("Static/images/image-1.png");
            _bodyBuilder.Attachments.Add("Static/images/image-2.png");
            _bodyBuilder.Attachments.Add("Static/images/image-3.png");

            _message.Body = _bodyBuilder.ToMessageBody();


            //Sending the message using SMTP
            //Parameters - email server address, port, whether to use SSL
            _smtpClient.Connect("smtp.gmail.com", 587, false);
            _smtpClient.Authenticate("kdu2030@gmail.com", "SenM@rcoRub1o");
            _smtpClient.Send(_message);
            _smtpClient.Disconnect(true);

        }

        private string ModifyEmailHTML(string templateFilePath, string keyword, string value) {
            StreamReader sr = new StreamReader(templateFilePath);
            string template = sr.ReadToEnd();
            sr.Close();
            template = template.Replace(keyword, value);
            return template;

        }

    }
}
