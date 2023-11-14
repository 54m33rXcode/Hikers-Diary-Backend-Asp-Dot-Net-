namespace HikersDiary_Web.Model
{
    public class Photo
    {
        public int Photo_Id { get; set; }
        public int Ppost_Id { get; set; }
        public string? Photo_Url { get; set; }
        public Post? Post { get; set; }
            
    }
}
