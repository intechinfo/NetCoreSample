namespace ITI.PrimarySchool.DAL
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public byte[] Password { get; set; }

        public string GithubAccessToken { get; set; }
        
        public string GoogleRefreshToken { get; set; }
    }
}
