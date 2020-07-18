namespace Models.Options
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }      
        public int LifeTime { get; set; }
    }
}
