using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CheckPlease.Repositories
{
    public class BaseRepository
    {

        private readonly IConfiguration _config;

        public BaseRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
    }
}