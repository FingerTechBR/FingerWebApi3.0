using Refit;
using WebApiFingertec3._0.Models;

namespace WebApiFingertec3._0.Abstraction
{
    public interface IConfigurationApi
    {
        public string BaseUrl { get; set; } 
    }
    public class ApiConfig : IConfigurationApi
    {
        public string BaseUrl { get; set; }
    }
}
