using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class MailkitMessage
    {
        public MailboxAddress Sender { get; set; }
        public MailboxAddress Receiver { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }



        public string renderConfirmMessage()
        {
            return $@"
                <a id='btn-confirm' href='{this.Content}'>Bấm vào đây để xác thực Tài khoản của bạn trên aov-shop.tk</a>
                <h5>This email lasts for 3 days.</h5>

                <style>
                    #btn-confirm {{
                        padding: 10px 20px;

                        background: dodgerblue;
                        color: white;
                        font-size: 20px;
                        text-decoration: none;

                        border-radius: 5px;
                    }}
                </style>";
        }

        public MimeMessage GetMimeMessage()
        {
            var message = this;
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Receiver);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new BodyBuilder { HtmlBody = message.renderConfirmMessage() }.ToMessageBody();

            return mimeMessage;
        }


    }
}
