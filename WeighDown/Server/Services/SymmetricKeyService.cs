using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WeighDown.Server.Services
{
    public class SymmetricKeyService
    {
        private readonly string _symmetricKey;

        public SymmetricKeyService(IConfiguration config)
        {
            _symmetricKey = config["SymmetricKey"];
        }

        public SymmetricSecurityKey GetSymmetricKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_symmetricKey));
        }
    }
}
