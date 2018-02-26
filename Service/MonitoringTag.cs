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
    public class MonitoringTag : IMonitoringTag
    {
        private readonly IConfiguration _configuration;
        private readonly IOtherApi _otherApi;
        private readonly IInterlevelDb _interlevelDb;
        public MonitoringTag(IConfiguration configuration,IOtherApi otherApi, IInterlevelDb interlevelDb)
        {
            _configuration = configuration;
            _otherApi = otherApi;
            _interlevelDb = interlevelDb;
        }

        public async Task<(bool,string)> ReadTags()
        {
            DateTime dateMonitoring = DateTime.Now;
            List<Thing> thingList = new List<Thing>();
            // faz get nas tags de input
            var tagsInput = await _otherApi.GetTagList("input");

            foreach(var tag in tagsInput)
            {
                foreach(var thingId in tag.thingGroup.thingsIds)
                {
                    // Verifica se a thing existe na lista..... se não existir faz get e adiciona na lista
                    
                    var thing = thingList.Where(x=>x.thingId == thingId).FirstOrDefault();

                    if(thing == null)
                    {
                        var thingGet = await _otherApi.GetThing(thingId);
                        if(thingGet == null)
                            continue;

                        thing = thingGet;
                        thingList.Add(thing);
                    }

                    var valueTag = await _interlevelDb.Read(tag.physicalTag);

                    var tagHistorian = new {
                        thingId = thingId,
                        tag = tag.tagName,
                        value = valueTag,
                        group = tag.tagGroup,
                        date = dateMonitoring.Ticks
                    };

                    var post = _otherApi.PostHistorian(tagHistorian);
                }

            }

            return(true,string.Empty);
        }
        
    }
}