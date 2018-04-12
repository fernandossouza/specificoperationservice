using System.Threading.Tasks;
using specificoperationservice.Model.SpecificPhase;
namespace specificoperationservice.Service.Interface
{
    public interface ISpecificPhaseService
    {
         Task<SpecificParameter> AddParameter(SpecificParameter specificParameter);
         Task<SpecificPhase> GetSpecificPhase(int phaseId);
    }
}