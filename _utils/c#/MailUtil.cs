using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using Com.Everunion.User;
using System.Web;

namespace Com.Everunion.Util
{
    public class MailUtil
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MailUtil));
        /*
         * 客戶完成訂單後,給業者發信.
         */
        public static void orderToTW(string seller, string id)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress fromAddress = new MailAddress("admin@zmo.com.tw", "ZMO進銷存系統");
                mail.From = fromAddress;
                mail.To.Add("zmo@zmo.com.tw");
                mail.Subject = "ZMO進銷存系統---訂單通知";
                mail.Body = "您好:<BR/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;經銷商<strong>" + seller + "</strong>已完成訂單,訂單號:<strong>" + id + "</strong>, 請及時處理!<BR/>";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                mail.Dispose();
                mail = null;
            }
            catch (Exception e)
            {
                log.Error("orderToTW:"+e.Message);
            }
        }

        /*
         * 業者確認後,發信通知倉庫發貨.
         */
        public static void orderToiWH(string seller, string id)
        {
            try
            {
                MailMessage mail = new MailMessage();
                MailAddress fromAddress = new MailAddress("admin@zmo.com.tw", "ZMO進銷存系統");
                mail.From = fromAddress;
                mail.To.Add("jhsjohnson@diamond.taiwin.com.tw");
                mail.Subject = "ZMO進銷存系統---發貨通知";
                mail.Body = "您好:<BR/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;經銷商<strong>" + seller + "</strong>的訂單已確認,訂單號:<strong>" + id + "</strong>, 請及時處理!<BR/>";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                mail.Dispose();
                mail = null;
            }
            catch (Exception e)
            {
                log.Error("orderToiWH:"+e.Message);
            }
        }

        /*
         * 業者確認後,發信通知倉庫發貨.
         */
        public static void register(Com.Everunion.User.User user)
        {
            try
            {
                string domain = HttpContext.Current.Request.Headers["host"].ToString();
                if (HttpContext.Current.Request.IsSecureConnection)
                {
                    domain = "https://" + domain;
                }
                else
                {
                    domain = "http://" + domain;
                }
                
                MailMessage mail = new MailMessage();
                MailAddress fromAddress = new MailAddress("admin@zmo.com.tw", "ZMO會員中心");
                mail.From = fromAddress;
                mail.To.Add(user.Email);
                mail.Subject = "ZMO會員中心---註冊驗證";

                StringBuilder sb = new StringBuilder(200);
                sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.Append("<head>");
                sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");
                sb.Append("<title>ZMO會員中心---註冊驗證</title>");
                sb.Append("</head>");
                sb.Append("<body>");
                sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td><img src=\""+domain+"/images/logos.jpg\" /></td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"20\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-size:13px;\"><p>此為系統自動通知信，請勿直接回信！</p>");
                sb.Append("<p>親愛的ZMO會員您好：</p>");
                sb.Append("<p>這個訊息是發自ZMO，通知您本次的會員程序已經完成！</p>");
                sb.Append("<p>以下是您的聯絡信箱，請依說明完成信箱認證，日後將以此信箱與您聯絡。</p>");
                sb.Append("<p>帳號名稱：<strong>"+user.Account+"</strong></p>");
                sb.Append("<p>聯絡信箱：<a href=\"mailto:"+user.Email+"\" target=\"_blank\" style=\" color: #993300;\">"+user.Email+"</a><a href='"+domain+"/user/verify.aspx?p="+user.Secret+"' target=\"_blank\" style=\"color: #993300;\">認證信箱</a></p>");
                sb.Append("<table width=\"100%\" border=\"0\" cellpadding=\"20\" cellspacing=\"0\" bgcolor=\"#f5f3f1\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td><p>※提醒您，您必須先完成信箱認證，才能登入ZMO頻道服務，未認證前僅能登入ZMO信箱。<br />");
                sb.Append("<br />");
                sb.Append("※ 如果您無法點選以上連結，請直接複製以下網址貼到瀏覽器網址列：<br />");
                sb.Append("<a href=\"#\" style=\"color:#993300;\">                "+domain+"/user/verify.aspx?p="+user.Secret+"</a></p>");
                sb.Append("<p>如果您未申請此帳號，請直接刪除此信件。 若有任何問題，請與&nbsp;<a href=\"mailto:zmoservice@zmo.com.tw\" target=\"_blank\" style=\"color: #993300;\">ZMO客服中心</a>聯絡，謝謝 !<br />");
                sb.Append("</p></td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("<br /></td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");
                
                mail.Body = sb.ToString();
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                mail.Dispose();
                mail = null;
            }
            catch (Exception e)
            {
                log.Error("register:"+e.Message);
            }
        }


        /*
         * 忘記密碼.
         */
        public static int resetPassword(Com.Everunion.User.User user, string newpwd)
        {
            int retValue = 0;
            try
            {
                string domain = HttpContext.Current.Request.Headers["host"].ToString();
                if (HttpContext.Current.Request.IsSecureConnection)
                {
                    domain = "https://" + domain;
                }
                else
                {
                    domain = "http://" + domain;
                }
                MailMessage mail = new MailMessage();
                MailAddress fromAddress = new MailAddress("admin@zmo.com.tw", "ZMO會員中心");
                mail.From = fromAddress;
                mail.To.Add(user.Email);
                mail.Subject = "ZMO會員中心---重新設定密碼";

                StringBuilder sb = new StringBuilder(200);

                sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.Append("<head>");
                sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");
                sb.Append("<title>ZMO會員中心---重新設定密碼</title>");
                sb.Append("</head>");
                sb.Append("<body>");
                sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td><img src=\""+domain+"/images/logos.jpg\" /></td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"20\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-size:13px;\"><p>此為系統自動通知信，請勿直接回信！</p>");
                sb.Append("<p>親愛的ZMO會員您好：</p>");
                sb.Append("<p>這個訊息是發自ZMO，通知您申請密碼！</p>");
                sb.Append("<p>以下是您的聯絡信箱，請依說明完成重新設定密碼,如果您沒有重新設定密碼,請不用理會該信件。</p>");
                sb.Append("<p>名稱：<strong>" + user.Name + "</strong></p>");
                sb.Append("<p>帳號：" + user.Account + " <a href='" + domain + "/user/pwdverify.aspx?u=" + user.Account + "&p=" + newpwd + "' target=\"_blank\" style=\"color:#993300;\">重新設定密碼</a></p>");
                sb.Append("<table width=\"100%\" border=\"0\" cellpadding=\"20\" cellspacing=\"0\" bgcolor=\"#f5f3f1\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td><p>※提醒您，如果不重新設定密碼,舊密碼可以正常登錄。<br />");
                sb.Append("<br />");
                sb.Append("※ 如果您無法點選以上連結，請直接複製以下網址貼到瀏覽器網址列：<br />");
                sb.Append("<a href=\"#\" style=\"color:#993300;\">   " + domain + "/user/pwdverify.aspx?u=" + user.Account + "&p=" + newpwd + "</a></p>");
                sb.Append("<p>如果您未申請重新設定密碼，請直接刪除此信件。</p>");
                sb.Append("<p>若有任何問題，請與&nbsp;<a href=\"mailto:zmoservice@zmo.com.tw\" target=\"_blank\" style=\"color:#993300;\">ZMO客服中心</a>聯絡，謝謝 !<br />");
                sb.Append("</p></td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("<br /></td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");
           
                mail.Body = sb.ToString();
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                mail.Dispose();
                mail = null;
                retValue = 1;
            }
            catch (Exception e)
            {
                log.Error("resetPassword:"+e.Message);
            }
            return retValue;
        }

        /// <summary>
        /// 貨到通知客戶
        /// </summary>
        /// <param name="sendEmail">需要寄出的郵件地址</param>
        /// <param name="proName">貨品名稱</param>
        /// <param name="qty">數量</param>
        /// <returns>結果集</returns>
        public static void ForApprise(Apprise a)
        {
            try
            {
                string domain = HttpContext.Current.Request.Headers["host"].ToString();
                if (HttpContext.Current.Request.IsSecureConnection)
                {
                    domain = "https://" + domain;
                }
                else
                {
                    domain = "http://" + domain;
                }
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("admin@zmo.com.tw", "ZMO會員中心");
                mail.To.Add(a.Email);
                mail.Subject = "ZMO會員中心---貨到通知";

                StringBuilder sb = new StringBuilder(200);

                sb.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
                sb.Append("<head>");
                sb.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");
                sb.Append("<title>ZMO會員中心---貨到通知</title>");
                sb.Append("</head>");
                sb.Append("<body>");
                sb.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td><img src=\"" + domain + "/images/logos.jpg\" /></td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("<table width=\"599\" border=\"0\" cellpadding=\"20\" cellspacing=\"0\">");
                sb.Append("<tbody>");
                sb.Append("<tr>");
                sb.Append("<td style=\"font-size:13px;\"><p>此為系統自動通知信，請勿直接回信！</p>");
                sb.Append("<p>親愛的ZMO會員您好：</p>");
                sb.Append("<p>感謝您的訂購 ! 您的商品已經寄出，訂單編號：<strong>xxxxx</strong></p>");
                sb.Append("<p>為了保護您的個人資料安全，本通知信將不顯示訂單明細。<strong></strong></p>");
                sb.Append("<p>建議您可以登入ZMO網站「訂單查詢」，查詢您的訂單以及處理進度。<br />");
                sb.Append("</p>");
                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");

                mail.Body = sb.ToString();
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
                mail.Dispose();
                mail = null;
            }
            catch ( Exception e )
            {
                log.Error("ForApprise:" + e.Message);
            }
        }

    }
}
