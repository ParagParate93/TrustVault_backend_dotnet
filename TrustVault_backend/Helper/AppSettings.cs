namespace TrustVault_backend.Helper
{
    public class AppSettings
    {

        private IConfiguration configuration;

        public AppSettings()
        {

        }
        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Secret = configuration["AppSettings:Secret"];

        }
         

        public string Secret { get; set; }
    }
}
