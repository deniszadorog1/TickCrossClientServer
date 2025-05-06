using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickCrossLib.Models.NonePlayable
{
    public class FriendRequestModel
    {
        public string SenderLogin { get; set; }

        public string ReceiverLogin { get; set; }

        public FriendRequestModel(string sender, string receiver)
        {
            SenderLogin = sender;
            ReceiverLogin = receiver;
        }


    }
}
