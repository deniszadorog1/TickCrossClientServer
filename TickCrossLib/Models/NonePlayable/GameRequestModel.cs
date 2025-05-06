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
        private string SenderLogin { get; set; }

        private string ReceiverLogin { get; set; }

        private char? SenderSign { get; set; }
        
        private char? ReceiverSign { get; set; }

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
