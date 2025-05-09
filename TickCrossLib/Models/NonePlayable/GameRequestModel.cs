using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickCrossLib.Models.NonePlayable
{
    public class GameRequestModel
    {
        public string SenderLogin { get; set; }

        public string ReceiverLogin { get; set; }

        public char? SenderSign { get; set; }
        
        public char? ReceiverSign { get; set; }

        private Enums.RequestStatus Status { get; set; }
        
        public GameRequestModel(string senderLogin, string receiverLogin, 
            char? senderSign, char? receiverSign, Enums.RequestStatus status)
        {
            SenderLogin = senderLogin;
            ReceiverLogin = receiverLogin;
            SenderLogin = senderLogin;
            ReceiverLogin = receiverLogin;
            Status = status;
        }
    }
}
