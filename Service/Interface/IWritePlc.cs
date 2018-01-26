using System.Threading.Tasks;
using specificoperationservice.Model;

namespace specificoperationservice.Service.Interface
{
    public interface IWritePlc
    {
         Task<bool> WriteOrder(ProductionOrder productionOrder);
    }
}