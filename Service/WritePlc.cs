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

namespace specificoperationservice.Service{
    public class WritePlc : IWritePlc
    {
        private readonly IConfiguration _configuration;
        private readonly IOtherApi _otherApi;
        private readonly IInterlevelDb _interleverDb;

        public WritePlc(IConfiguration configuration,IOtherApi otherApi, IInterlevelDb interlevelDb){
            _configuration = configuration;
            _otherApi = otherApi;
            _interleverDb = interlevelDb;
        }

        public async Task<(bool,string)> WriteOrder(ProductionOrder productionOrder){
            try{
                bool returnBool;

                switch(productionOrder.typeDescription.ToString().ToLower())
                {
                    case "tira":
                        Console.WriteLine("Começou");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("");
                        Console.WriteLine("Era de tira");
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
            var trigger = _configuration["TagIdTrigger"];
            // Finaliza Ordem anterior
            var triggerPlc = _interleverDb.Write("2",trigger,"Linha");


            List<Thing> thingsGetList = new List<Thing>();
            var trigger = _configuration["TagIdTrigger"];
            _interleverDb.Write("2",trigger,"Linha");
            var phases = productionOrder.recipe.phases;
            /*Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Phase: ------------------------------------------------------------ ");
            Console.WriteLine("Phase-> " +JsonConvert.SerializeObject(phases).ToString() );
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Loop de phases");
            Console.WriteLine(""); */           
            foreach (var phase in phases)
            {
                foreach(var phaseParameter in phase.phaseParameters)
                {
                    /*Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("phaseParameter: ------------------------------------------------------------ ");
                    Console.WriteLine("phaseParameter-> " +JsonConvert.SerializeObject(phaseParameter).ToString() );
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");*/
                    string value = phaseParameter.setupValue;

                    var tag = await _otherApi.GetTag(phaseParameter.tag.tagId);
                    //var tag = phaseParameter.tag;                    
                    //Console.WriteLine("tag: ------------------------------------------------------------ ");
                    //Console.WriteLine("tag-> " +JsonConvert.SerializeObject(tag).ToString() );
                    var thingGroup = await _otherApi.GetThingGroup(tag.thingGroupId);
                    //Console.WriteLine("Loop thingGroup: ------------------------------------------------------------ ");

                    foreach(var thingId in thingGroup.thingsIds)
                    {
                        Console.WriteLine("Foi no Foreach");
                        Console.WriteLine("");                        
                        Thing thing =null;
                        
                        thing = thingsGetList.Where(x=> x.thingId == thingId).FirstOrDefault();
                        Console.WriteLine("thingId" +thingId.ToString());
                        if(thing == null)
                        {
                            Console.WriteLine("get na thing");
                            thing = await _otherApi.GetThing(thingId);
                            //Console.WriteLine(JsonConvert.SerializeObject(await _otherApi.GetThing(thingId)).ToString() );
                            if(thing != null)
                                thingsGetList.Add(thing);
                        }
                        //Console.WriteLine(JsonConvert.SerializeObject(thing).ToString() );
                        if(string.IsNullOrEmpty(tag.physicalTag)){                                
                            Console.WriteLine("");
                            Console.WriteLine("");
                            Console.WriteLine("Deu ruim");
                            Console.WriteLine("");
                            Console.WriteLine("");
                            continue;
                        }
                        Console.Write("Physical da tag :");
                        Console.WriteLine(tag.physicalTag);
                        Console.Write("Valor da tag :");
                        Console.WriteLine(value);
                        Console.Write("phisical connection : ");
                        Console.WriteLine(thing.physicalConnection);                        
                        Console.WriteLine("");
                        var e = _interleverDb.Write(value,tag.physicalTag,thing.physicalConnection);
                        Console.WriteLine("Fim");
                    }                    
                }
            }

            // envia o trigger para o PLC           
            var triggerPlc = _interleverDb.Write("5",trigger,"Linha");
            return true;
        }

            
    }
}