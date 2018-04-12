using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using specificoperationservice.Model;
using specificoperationservice.Service.Interface;

namespace specificoperationservice.Service
{
    public class OtherApi : IOtherApi
    {
         private readonly IConfiguration _configuration;

        public OtherApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Thing> GetThing(int thingId)
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

        public async Task<List<Thing>> GetAlarm()
        {
            HttpClient client = new HttpClient();
            List<Thing> returnThingAlarm = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["AlarmServiceEndpoint"]);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnThingAlarm = JsonConvert.DeserializeObject<List<Thing>>(await client.GetStringAsync(url));
                    return returnThingAlarm;
                case HttpStatusCode.NotFound:
                    return returnThingAlarm;
                case HttpStatusCode.InternalServerError:
                    return returnThingAlarm;
            }
            return returnThingAlarm;
        }

        public async Task<Tag> GetTag(int tagId)
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
        public async Task<List<Tag>> GetTagList(string tagType)
        {
            HttpClient client = new HttpClient();
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["tagServiceEndpoint"]+"?quantity=1000&fieldFilter=tagType&fieldValue="+ tagType);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    var returnTag = JObject.Parse(await client.GetStringAsync(url));

                    List<Tag> taglist = new List<Tag>(); 
                    foreach(var tagJObject in returnTag["values"])
                    {
                        taglist.Add(tagJObject.ToObject<Tag>());
                    }
                    return taglist;
                case HttpStatusCode.NotFound:
                    return null;
                case HttpStatusCode.InternalServerError:
                    return null;
            }
            return null;
            
        }
        public async Task<ThingGroup> GetThingGroup(int groupId)
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

        public async Task<Phase> GetPhase(int phaseId)
        {
          HttpClient client = new HttpClient();
            Phase returnPhase = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["GetPhase"] + phaseId);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    returnPhase = JsonConvert.DeserializeObject<Phase>(await client.GetStringAsync(url));
                    return returnPhase;
                case HttpStatusCode.NotFound:
                    return returnPhase;
                case HttpStatusCode.InternalServerError:
                    return returnPhase;
            }
            return returnPhase;
        }

        public async Task<bool> PostHistorian(dynamic json)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(json).ToString(), Encoding.UTF8,"application/json");
            var builder = new UriBuilder(_configuration["HistorianBigTableServiceEndpoint"]);
            string url = builder.ToString();
            var result = await client.PostAsync(url,contentPost);
            
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:                    
                    return true;
                case HttpStatusCode.Created:
                    return true;
                case HttpStatusCode.InternalServerError:
                    return false;
            }
            return false;

        }

        public async Task<PhaseParameter> PostPhaseParameter(PhaseParameter phaseParameter)
        {
            PhaseParameter phaseParameterNew =null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(phaseParameter).ToString(), Encoding.UTF8,"application/json");
            var builder = new UriBuilder(_configuration["PostPhaseParameter"]+ _configuration["PhaseLinhaId"]);
            string url = builder.ToString();
            var result = await client.PostAsync(url,contentPost);
            
            if (result.StatusCode == HttpStatusCode.OK)
            {
                phaseParameterNew = JsonConvert.DeserializeObject<PhaseParameter>(await result.Content.ReadAsStringAsync());
            }
            return phaseParameterNew;
        }

    }
}