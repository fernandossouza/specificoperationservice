using System.Threading.Tasks;

namespace specificoperationservice.Service.Interface
{
    public interface IWriteLigaPlc
    {
         Task<(bool,string)> HabilitaForno(int Posicao);
         Task<(bool,string)> IniciaForno(int Posicao);
         Task<(bool,string)> FinalizaForno(int Posicao);
    }
}