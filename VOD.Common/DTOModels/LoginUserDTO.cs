namespace VOD.Common.DTOModels
{
    public class LoginUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
    }
}
