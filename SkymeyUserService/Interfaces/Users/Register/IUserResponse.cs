namespace SkymeyUserService.Interfaces.Users.Register
{
    public interface IUserResponse : IUserResponseJWT
    {
        public bool ResponseType { get; set; }
        public string Response { get; set; }
    }
}
