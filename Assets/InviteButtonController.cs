using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Facebook.Unity;
using System.Net;
using System.Net.Mail;

/*namespace GmailInvitation
{
    class Program
    {
        static string username;
        static string password;
        static void Main(string[] arg)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com",587))
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(username, password);
                    MailMessage messageObject = new MailMessage();
                    messageObject.To.Add("ashmikattel@gmail.com");
                    messageObject.From = new MailAddress(username);
                    messageObject.Subject = "Invitation to play kitti game";
                    messageObject.Body = "Hey!! come play this awesome game.";
                    client.Send(messageObject);
                }

            }
            catch
            {

            }
        }
    }
}
*/
namespace kitti
{
    public class InviteButtonController : MonoBehaviour
    {
        // Start is called before the first frame update
        public void OnClickinvitationButton(
            )
        {
            Application.OpenURL("mailto:?subject=Invitation to play kitti game&body=Hey! Come and play this awesome game! https://bilson4321.github.io/kitti_game/");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}