namespace HikersDiary_Web.Model
{
    public class Notification
    {
        public int Notify_Id { get; set; }
        public int Sender_Id { get; set; }
        public int Receiver_Id { get; set; }
        public string? Notify_Type { get; set; }
        public String? Notification_Message {get; set; } 

        public User? Sender { get; set; }
        public User? Receiver { get; set; }
    }
}
