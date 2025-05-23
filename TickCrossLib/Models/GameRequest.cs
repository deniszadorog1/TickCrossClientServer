namespace TickCrossLib.Models
{
    public class GameRequest
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public char SenderSign { get; set; }
        public char ReceiverSign { get; set; }


        public GameRequest(int id, User sender, User receiver, char senderSign, char receiverSign)
        {
            Id = id;
            Sender = sender;
            Receiver = receiver;
            SenderSign = senderSign;
            ReceiverSign = receiverSign;
        }

    }
}
