namespace HikersDiary_Web.Model
{
    public class Follow
    {
        public int Follower_Id { get; set; }
        public int Following_Id { get; set; }

        public User Follower { get; set; }
        public User Following { get; set; }
    }
}
