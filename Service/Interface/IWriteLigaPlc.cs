using System.Threading.Tasks;
using specificoperationservice.Model;

namespace specificoperationservice.Service.Interface
{
    public interface IWriteLigaPlc
    {
         Task<(bool,string)> HabilitaForno(int Posicao);
         Task<(bool,string)> IniciaForno(ProductionOrder productionOrder);
         Task<(bool,string)> LabOkForno(ProductionOrder productionOrder);
         Task<(bool,string)> AddCobreFosforosoForno(ProductionOrder productionOrder);
         Task<(bool,string)> FinalizaForno(int Posicao);
    }
}