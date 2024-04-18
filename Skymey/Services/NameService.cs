namespace Skymey.Services
{
    public interface INameService
    {
        public string Title { get; }
        public string Index { get; }
        public string SignIn { get; }
        public string SignUp { get; }
    }
    public class NameService : INameService
    {
        public string Title { get; } = "Skymey";
        public string Index { get; } = "Cryptocurrency Market";
        public string SignIn { get; } = "Sign In";
        public string SignUp { get; } = "Sign Up";
    }
}
