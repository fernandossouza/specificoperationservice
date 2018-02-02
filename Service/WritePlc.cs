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

        public WritePlc(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> WriteOrder(ProductionOrder productionOrder)
        {
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
            
            return returnBool;
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

                    var tag = await GetTagApi(phaseParameter.tag.tagId);

                    var thingGroup = await GetThingGroupApi(phaseParameter.tag.thingGroup.thingGroupId);

                    foreach(var thingId in thingGroup.thingsIds)
                    {
                        Thing thing;

                        thing = thingsGetList.Where(x=> x.thingId == thingId).FirstOrDefault();
                        if(thing == null)
                        {
                            thing = await GetThingApi(thingId);
                            if(thing != null)
                                thingsGetList.Add(thing);
                        }
                        if(string.IsNullOrEmpty(thing.physicalConnection))
                            continue;

                        var e = CreateCommand(value,tag.physicalTag,thing.physicalConnection);
                    }

                }
            }
            return true;
        }

        private async Task<bool> CreateCommand(string value, string tag, string workstation)
        {
            string command = string.Empty;

            command = "SELECT public.spi_sp_write_per_name('" + tag + "','" + workstation + "','" + value + "')";
            var result = await ExecuteCommand(command);

            return true;

        }

        private async Task<IEnumerable<dynamic>> ExecuteCommand(string commandSQL)
        {
          
            IEnumerable<dynamic> dbResult;
            using(IDbConnection dbConnection = new NpgsqlConnection(_configuration["stringInterlevelConnection"]))
            {
                dbConnection.Open();
                dbResult = await dbConnection.QueryAsync<dynamic>(commandSQL);
            }

            return dbResult;
           
            
        }

        private async Task<Tag> GetTagApi(int tagId)
        {
            HttpClient client = new HttpClient();
            Tag returnTag = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["tagServiceEndpoint"]+ tagId.ToString());
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnTag = JsonConvert.DeserializeObject<Tag>(await client.GetStringAsync(url));
                    return returnTag;
                case HttpStatusCode.NotFound:
                    return returnTag;
                case HttpStatusCode.InternalServerError:
                    return returnTag;
            }
            return returnTag;
        }

        private async Task<Thing> GetThingApi(int thingId)
        {
            HttpClient client = new HttpClient();
            Thing returnThing = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["thingServiceEndpoint"]+ thingId.ToString());
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnThing = JsonConvert.DeserializeObject<Thing>(await client.GetStringAsync(url));
                    return returnThing;
                case HttpStatusCode.NotFound:
                    return returnThing;
                case HttpStatusCode.InternalServerError:
                    return returnThing;
            }
            return returnThing;
        }

        private async Task<ThingGroup> GetThingGroupApi(int groupId)
        {
            HttpClient client = new HttpClient();
            ThingGroup returnGroups = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["thingGroupServiceEndpoint"] + groupId);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnGroups = JsonConvert.DeserializeObject<ThingGroup>(await client.GetStringAsync(url));
                    return returnGroups;
                case HttpStatusCode.NotFound:
                    return returnGroups;
                case HttpStatusCode.InternalServerError:
                    return returnGroups;
            }
            return returnGroups;
        }
        
    }
}