using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using System.Net;
using System.Net.Mail;

namespace Domain.Concreate
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        public void ProccessFeedBack(FeedBack feedBack)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("riskyfel@gmail.com", "rubinforever29");
            MailMessage msg = new MailMessage();
            msg.To.Add("riskyfel@gmail.com");
            msg.From = new MailAddress("riskyfel@gmail.com");
            msg.Subject = "Обратная связь с ИМ";

            StringBuilder body = new StringBuilder()
                .AppendLine("Новое письмо поступило")
                .AppendLine("---")
                .AppendLine("ФИО клиента:" + feedBack.Name)
                .AppendLine("---")
                .AppendLine("Номер телефона клиента: " + feedBack.PhoneNumber)
                .AppendLine("---")
                .AppendLine("Email клиента: " + feedBack.Email)
                .AppendLine("---")
                .AppendLine("Сообщение: " + feedBack.Message);
                
            msg.Body = body.ToString();

            client.Send(msg);
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("riskyfel@gmail.com", "rubinforever29");
            MailMessage msg = new MailMessage();
            msg.To.Add("riskyfel@gmail.com");
            msg.From = new MailAddress("riskyfel@gmail.com");

            msg.Subject = "Заказ с ИМ";
            
            StringBuilder body = new StringBuilder()
                .AppendLine("Новый заказ обработан")
                .AppendLine("---")
                .AppendLine("Товары:");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Phone.Price * line.Quantity;
                body.AppendFormat("{0} x {1} {2} | ", line.Quantity, line.Phone.Mark, line.Phone.Model)
                .AppendFormat(" Итого: " + subtotal);
            }

            body.AppendLine("---")
                .AppendFormat("Общая стоимость: "+ cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Данные о доставке и клинете:")
                .AppendLine("ФИО: " + shippingDetails.Name)
                .AppendLine("Номер телефона: " + shippingDetails.PhoneNumber)
                .AppendLine("Email: " + shippingDetails.Email)
                .AppendLine("Адрес доставки: " + shippingDetails.Line);

            msg.Body = body.ToString();

            client.Send(msg);
        }
    }
}
