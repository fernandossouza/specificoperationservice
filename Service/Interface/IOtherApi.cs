using System.Threading.Tasks;
using System.Collections.Generic;
using specificoperationservice.Model;
namespace specificoperationservice.Service.Interface
{
    public interface IOtherApi
    {
         Task<Thing> GetThing(int thingId);
         Task<Tag> GetTag(int tagId);
         Task<List<Tag>> GetTagList(string tagType);
         Task<ThingGroup> GetThingGroup(int groupId);
         Task<bool> PostHistorian(dynamic json);
    }
}