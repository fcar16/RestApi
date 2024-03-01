using System;
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

public class EmailController : ControllerBase
{
    [HttpPost]
    [Microsoft.AspNetCore.Mvc.Route("SendEmail")]
    public OkResult SendEmail ()
    {
        // Configurar los detalles del servidor SMTP
        string smtpServer = "smtp.gmail.com";
        int puertoSmtp = 587; // Puerto SMTP para envío seguro (TLS)
        string correoEmisor = "javiergonzalezaguilar10@gmail.com";
        string contraseñaEmisor = "qaqcykdzzdmklfwn";

        // Configurar el mensaje
        string correoDestino = "javiergonzalezaguilar10@gmail.com";
        string asunto = "Prueba de correo electrónico";
        string cuerpoMensaje = "Hola, esto es una prueba de correo electrónico.";

        // Crear el objeto MailMessage
        MailMessage mensaje = new MailMessage(correoEmisor, correoDestino, asunto, cuerpoMensaje);

        // Crear el cliente SMTP
        SmtpClient clienteSmtp = new SmtpClient(smtpServer, puertoSmtp);

        // Autenticación (si es necesaria)
        clienteSmtp.UseDefaultCredentials = false;
        clienteSmtp.Credentials = new NetworkCredential(correoEmisor, contraseñaEmisor);
        clienteSmtp.EnableSsl = true; // Habilitar SSL para envío seguro

        try
        {
            // Enviar el correo electrónico
            clienteSmtp.Send(mensaje);
            Console.WriteLine("El correo electrónico fue enviado exitosamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
        }
        finally
        {
            // Liberar recursos
            mensaje.Dispose();
            clienteSmtp.Dispose();
        }
         return Ok();
    }
}
