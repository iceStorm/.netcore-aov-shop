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
                <h5>E-mail này có hiệu lực trong 3 ngày.</h5>

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

        public string renderResetPassewordMessage()
        {
            return $@"
                <a id='btn-confirm' href='{this.Content}'>Bấm vào đây để Đặt lại Mật khẩu của bạn trên aov-shop.tk</a>
                <h5>E-mail này có hiệu lực trong 3 ngày.</h5>

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



        public MimeMessage GetConfirmEmailMimeMessage()
        {
            var message = this;
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Receiver);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new BodyBuilder { HtmlBody = message.renderConfirmMessage() }.ToMessageBody();

            return mimeMessage;
        }

        public MimeMessage GetResetPasswordMimeMessage()
        {
            var message = this;
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Receiver);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new BodyBuilder { HtmlBody = message.renderResetPassewordMessage() }.ToMessageBody();

            return mimeMessage;
        }

    }
}
