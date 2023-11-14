namespace HikersDiary_Web.Model
{
    public class Like
    {
        public int Like_Id { get; set; }
        public int Lpost_Id { get; set; }
        public int Liker_Id { get; set;}

        public Post? Post { get; set; }
        public User? Liker { get; set; }

    }
}
