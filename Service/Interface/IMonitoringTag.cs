using System.Threading.Tasks;
using specificoperationservice.Model;
namespace specificoperationservice.Service.Interface
{
    public interface IMonitoringTag
    {
         Task<(bool,string)> ReadTags();
    }
}