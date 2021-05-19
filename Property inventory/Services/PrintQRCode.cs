using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Property_inventory.Models;

namespace Property_inventory.Services
{
    public class PrintQRCode
    {
        private string IP => "192.168.0.22";
        private int PORT => 6101;

        private IPEndPoint tcpEndPoint;
        private Socket tcpSocket;

        public string EN_OrgName { get; }
        public string OrgTelephone { get; }
        public string OrgEmail { get; }
        public List<Equip> EquipList { get; }

        public PrintQRCode()
        {
            tcpEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            EN_OrgName = "MKOU TSH 20";
            OrgEmail = "TSOSH20@MAIL.RU";
            OrgTelephone = "8 (39162) 26606";

        }

        public void Print()
        {
            tcpSocket.Connect(tcpEndPoint);

            var codes = GenerateCode();
            foreach (var code in codes)
            {
                tcpSocket.Send(GetBytes(code));
            }

            var buffer = new byte[4096];
            var size = 0;
            var answer = new StringBuilder();

            do
            {
                size = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (tcpSocket.Available > 0);

            Debug.WriteLine($"[PrintQRCode]: printer answer: {answer}");

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }

        private IEnumerable<string> GenerateCode()
        {
            //Ограничить количество символов на название организации.
            //Пишем только англ. буквами.

            var codes = new List<string>();

            foreach (var equip in EquipList)
            {
                var code = "^XA" +
                           "^FO025" +
                           "^BQN,2,10" +
                           $"^FDMM,Ainv:{equip.Id}^FS" +
                           "^FO250,35" +
                           "^ASN,30,30" +
                           $"^FDINV:{equip.InvNum}^FS" +
                           "^FO250,75" +
                           "^ASN,30,30" +
                           $"^FDORG: {EN_OrgName}^FS" +
                           "^FO250,115" +
                           "^ASN,30,30" +
                           $"^FDEMAIL: {OrgEmail}^FS" +
                           "^FO250,155" +
                           "^ASN,30,30" +
                           $"^FDPHONE: {OrgTelephone}^FS" +
                           "^XZ";
                codes.Add(code);
            }
            return codes;
        }

        private byte[] GetBytes(string str) => Encoding.UTF8.GetBytes(str);
    }
}
