using Payment.Applications.Interface;

namespace thuongmaidientus1.AccountService
{
    public class ConnectionService : IConnectionService
    {
        private readonly IConfiguration configuration;

        public ConnectionService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string? Database => configuration.GetConnectionString("MyDB");

       
    }
}
