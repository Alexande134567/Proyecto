using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;

namespace GestionProyectosCiencias.Datos
{
    public class ServicioMensajeria
    {
        // 1. Envío automático por Correo Electrónico (SMTP)
        public static async Task<bool> EnviarCorreoAutomaticoAsync(string correoDestino, string asunto, string mensajeBody)
        {
            try
            {
                // Configura aquí la cuenta oficial que enviará los correos del sistema
                string correoEmisor = "notificaciones.proyecto@gmail.com";
                string contrasenaApp = "abcd efgh ijkl mnop"; // Contraseña de aplicación de Google de 16 caracteres

                using (MailMessage correo = new MailMessage())
                {
                    correo.From = new MailAddress(correoEmisor, "Centro de Notificaciones");
                    correo.To.Add(correoDestino);
                    correo.Subject = asunto;
                    correo.Body = mensajeBody;
                    correo.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                    {
                        smtp.Port = 587;
                        smtp.Credentials = new NetworkCredential(correoEmisor, contrasenaApp.Replace(" ", ""));
                        smtp.EnableSsl = true;

                        await smtp.SendMailAsync(correo);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SMTP: " + ex.Message);
                return false;
            }
        }

        // 2. Envío automático al Celular en segundo plano (vía API SMS / Twilio)
        public static async Task<bool> EnviarSMSAutomaticoAsync(string numeroTelefono, string mensaje)
        {
            try
            {
                // Claves de la API para SMS / WhatsApp automático
                string accountSid = "TU_ACCOUNT_SID";
                string authToken = "TU_AUTH_TOKEN";
                string numeroTwilio = "+1234567890";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{accountSid}:{authToken}")));

                    var values = new Dictionary<string, string>
                    {
                        { "From", numeroTwilio },
                        { "To", numeroTelefono },
                        { "Body", mensaje }
                    };

                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync($"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json", content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        // 3. Método alternativo para abrir WhatsApp Web con mensaje listo
        public static void AbrirWhatsAppDirecto(string numeroTelefono, string mensaje)
        {
            string numLimpio = numeroTelefono.Replace("+", "").Replace(" ", "").Replace("-", "");
            string url = $"https://api.whatsapp.com/send?phone={numLimpio}&text={Uri.EscapeDataString(mensaje)}";
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}