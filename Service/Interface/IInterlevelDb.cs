using System.Threading.Tasks;

namespace specificoperationservice.Service.Interface
{
    public interface IInterlevelDb
    {
         Task<bool> Write(string value, string tag, string workstation);
         Task<string> Read(string tagName);
    }
}