using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using Dapper;
using Newtonsoft.Json;
using specificoperationservice.Model;
using specificoperationservice.Service.Interface;

namespace specificoperationservice.Service
{
    public class WritePlc : IWritePlc
    {
        private readonly IConfiguration _configuration;
        private readonly IOtherApi _otherApi;
        private readonly IInterlevelDb _interleverDb;

        public WritePlc(IConfiguration configuration,IOtherApi otherApi, IInterlevelDb interlevelDb)
        {
            _configuration = configuration;
            _otherApi = otherApi;
            _interleverDb = interlevelDb;
        }

        public async Task<(bool,string)> WriteOrder(ProductionOrder productionOrder)
        {
            try{
            bool returnBool;

            switch(productionOrder.typeDescription.ToString().ToLower())
            {
                case "tira":
                    returnBool = await OpTypeTira(productionOrder);
                    break;
                case "liga":
                    returnBool = true;
                    break;
                default:
                    returnBool = false;
                    break;
            }
            
            return (returnBool,string.Empty);
            }
            catch(Exception ex)
            {
                return (false,ex.ToString());
            }
        }

        private async Task<bool> OpTypeTira(ProductionOrder productionOrder)
        {
            List<Thing> thingsGetList = new List<Thing>();
            var phases = productionOrder.recipe.phases;

            foreach (var phase in phases)
            {
                foreach(var phaseParameter in phase.phaseParameters)
                {
                    string value = phaseParameter.setupValue;

                    var tag = await _otherApi.GetTag(phaseParameter.tag.tagId);

                    var thingGroup = await _otherApi.GetThingGroup(phaseParameter.tag.thingGroup.thingGroupId);

                    foreach(var thingId in thingGroup.thingsIds)
                    {
                        Thing thing;

                        thing = thingsGetList.Where(x=> x.thingId == thingId).FirstOrDefault();
                        if(thing == null)
                        {
                            thing = await _otherApi.GetThing(thingId);
                            if(thing != null)
                                thingsGetList.Add(thing);
                        }
                        if(string.IsNullOrEmpty(thing.physicalConnection))
                            continue;

                        var e = _interleverDb.Write(value,tag.physicalTag,thing.physicalConnection);
                    }

                }
            }
            return true;
        }

            
    }
}