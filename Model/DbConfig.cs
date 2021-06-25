namespace PassKeeper.Model
{
    public class DbConfig
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string EncrptKey { get; set; }
        public bool IsUseGoogleAuthenticator { get; set; }
        public string GoogleAuthenticatorKey { get; set; }
    }
}
