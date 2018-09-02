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
        List<Thing> thingList;
        public MonitoringTag(IConfiguration configuration,IOtherApi otherApi, IInterlevelDb interlevelDb)
        {
            _configuration = configuration;
            _otherApi = otherApi;
            _interlevelDb = interlevelDb;
            thingList = new List<Thing>();
        }

        public async Task<(bool,string)> ReadTags()
        {
            string numeroRolo="";
            string numeroOP = "";
            DateTime dateMonitoring = DateTime.Now;
            // faz get nas tags de input            
            var tagsInput = await _otherApi.GetTagList("input");

            //Faz get dos alarm
            var thingAlarms = await _otherApi.GetAlarm();

            //Faz get da tag com o número do rolo
            var tagRolo = await _otherApi.GetTag(Convert.ToInt32(_configuration["IdTagRolo"]));
            if(tagRolo != null)
                numeroRolo = await _interlevelDb.Read(tagRolo.physicalTag);
            //Faz get da tag com o número da OP
            var tagOP = await _otherApi.GetTag(Convert.ToInt32(_configuration["IdTagOP"]));
            if(tagOP != null)
                numeroOP = await _interlevelDb.Read(tagOP.physicalTag);
            


            foreach(var tag in tagsInput)
            {
                foreach(var thingId in tag.thingGroup.thingsIds)
                {
                    DateTime dt = DateTime.Now;
                    // Verifica se a thing existe na lista..... se não existir faz get e adiciona na lista
                    Console.WriteLine("antes thing" + new TimeSpan((DateTime.Now - dt).Ticks).TotalMilliseconds.ToString());
                    var thing = ReturnThing(thingId);
                    if(thing == null)
                    {
                        return (false,"Thing não encontrada. Thing Id = " + thingId.ToString());
                    }
                    Console.WriteLine("depois thing" + new TimeSpan((DateTime.Now - dt).Ticks).TotalMilliseconds.ToString());
                    var valueTag = await _interlevelDb.Read(tag.physicalTag);
                    Console.WriteLine("depois select" + new TimeSpan((DateTime.Now - dt).Ticks).TotalMilliseconds.ToString());
                    var tagHistorian = new {
                        thingId = thingId,
                        tag = tag.tagName,
                        value = valueTag,
                        group = tag.tagGroup,
                        date = dateMonitoring.Ticks
                    };
                    Console.WriteLine("antes post" + new TimeSpan((DateTime.Now - dt).Ticks).TotalMilliseconds.ToString());
                    var post = await _otherApi.PostHistorian(tagHistorian);
                    Console.WriteLine("depois post" + new TimeSpan((DateTime.Now - dt).Ticks).TotalMilliseconds.ToString());
                    var tagHistorianRolo = new {
                        thingId = thingId,
                        tag = "rolo",
                        value = numeroRolo,
                        group = "Linha",
                        date = dateMonitoring.Ticks
                    };

                    var post2 = await _otherApi.PostHistorian(tagHistorianRolo);

                    var tagHistorianOP = new {
                        thingId = thingId,
                        tag = "ordem",
                        value = numeroOP,
                        group = "Linha",
                        date = dateMonitoring.Ticks
                    };

                    var post3 = await _otherApi.PostHistorian(tagHistorianOP);
                }

            }

           

            foreach(var thingAlarm in thingAlarms)
            {
                foreach(var alarm in thingAlarm.alarms)
                {
                     var tagHistorian = new {
                        thingId = alarm.thingId,
                        tag = alarm.alarmName,
                        value = alarm.alarmDescription,
                        group = "Alarme",
                        date = dateMonitoring.Ticks
                    };

                    var post = _otherApi.PostHistorian(tagHistorian);
                }
            }


            return(true,string.Empty);
        }

        private async Task<Thing> ReturnThing(int thingId)
        {
            var thing = thingList.Where(x=>x.thingId == thingId).FirstOrDefault();
            if(thing == null)
                {
                    var thingGet = await _otherApi.GetThing(thingId);
                
                    if(thingGet == null)
                        return null;

                    thingList.Add(thingGet);                        
                    return thingGet;
                }
            return thing;

        }
        
    }
}