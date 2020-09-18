namespace Models.Domain
{
    public class UserComment
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public string Comment { get; set; }
    }
}
