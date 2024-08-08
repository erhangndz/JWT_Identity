namespace JwtIdentity.API.Models
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            Moderator,
            User
        }
        public const string default_username = "erhangndz";
        public const string default_email = "erhangndz@mail.com";
        public const string default_password = "Password12*";
        public const Roles default_role = Roles.User;
    }
}
