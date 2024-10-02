namespace UserAuthentication.DTOs
{
    public class UserLoginDto
    {
        public UserDto User { get; set; }
        public string Username {  get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        
    }
}
