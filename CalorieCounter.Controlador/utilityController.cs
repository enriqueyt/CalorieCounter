using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EASendMail;

namespace CalorieCounter.Controlador {
    public class utilityController {

        public bool EnviarEmail(string _Server, int _Puerto, string _From, string _PasswordFrom, string _To, string _Titulo, string _Mensaje) {
            try {
                System.Net.Mail.MailMessage Mail;
                Mail = new System.Net.Mail.MailMessage(_From, _To, _Titulo, _Mensaje);

                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient(_Server);

                Mail.IsBodyHtml = true;
                smtpMail.EnableSsl = false;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Port = _Puerto;
                smtpMail.Host = _Server;
                smtpMail.Credentials = new System.Net.NetworkCredential(_From, _PasswordFrom);

                smtpMail.Send(Mail);
                return true;
            }
            catch(Exception) {
                return false;
            }
        }

        public bool EnviarNotificacionEXT(string De, string Para, string Titulo, string Contenido,ref string ErrMess) {
	            bool bSSLConnection = true;
	            EASendMail.SmtpMail oMail = new EASendMail.SmtpMail("TryIt");
	            EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();
	            string err = "";
	            try {
	                oMail.From = De == "" ? "eyt.projects@gmail.com" : De;
	                oMail.To = Para;
	                oMail.Cc = "";
	                oMail.Subject = Titulo;
	                if(Contenido.Contains("<html>"))oMail.HtmlBody = Contenido;
	                else oMail.TextBody = Contenido;
	                EASendMail.SmtpServer oServer = new EASendMail.SmtpServer("smtp.gmail.com");
                    oServer.User = "eyt.projects@gmail.com";
                    oServer.Password = "eytprojects";
	                oServer.Port = 587;
	                oServer.ProxyProtocol = SocksProxyProtocol.Http;
	                oServer.SocksProxyServer = "http2.bpv";
	                oServer.SocksProxyPort = 80;
	                oServer.SocksProxyUser = @"BPV\VC11660"; oServer.SocksProxyPassword = "USVC1234";
	                //oServer.SocksProxyUser = @"BPV\VP46327"; oServer.SocksProxyPassword = "%Adolfo42%";
	                if (bSSLConnection)
	                    oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
	                oSmtp.SendMail(oServer, oMail);
	                return true;
	            }
	            catch (SmtpTerminatedException exp) {
	                ErrMess = exp.Message;
	            }
	            catch (SmtpServerException exp) {
	                ErrMess = String.Format("Exception: Server Respond: {0}", exp.ErrorMessage);
	            }
	            catch (System.Net.Sockets.SocketException exp) {
	                ErrMess = String.Format("Exception: Networking Error: {0} {1}", exp.ErrorCode, exp.Message);
	            }
	            catch (System.ComponentModel.Win32Exception exp) {
	                ErrMess = String.Format("Exception: System Error: {0} {1}", exp.ErrorCode, exp.Message);
	            }
	            catch (System.Exception exp) {
	                ErrMess = String.Format("Exception: Common: {0}", exp.Message 
	                    + " StackTrace: " + exp.StackTrace + " " +exp.GetBaseException().Message);
	            }
	            return false;
	        }

        public string DecryptBase64(string cadena) {
            var encoder = new System.Text.UTF8Encoding();
            var utf8Decode = encoder.GetDecoder();

            byte[] cadenaByte = Convert.FromBase64String(cadena);
            int charCount = utf8Decode.GetCharCount(cadenaByte, 0, cadenaByte.Length);
            char[] decodedChar = new char[charCount];
            utf8Decode.GetChars(cadenaByte, 0, cadenaByte.Length, decodedChar, 0);
            return new String(decodedChar);
        }

    }
}
