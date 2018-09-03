using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Npgsql;
using specificoperationservice.Service.Interface;

namespace specificoperationservice.Service
{
    public class InterlevelDb : IInterlevelDb
    {
        private readonly IConfiguration _configuration;
        public List<dynamic> _table;
        public InterlevelDb(IConfiguration configuration)
        {
            _configuration = configuration;
            ReadTable();
            
        }
        public async Task<bool> Write(string value, string tag, string workstation){
            Console.Write("Fila do IL");
            string command = string.Empty;
 
            command = "SELECT public.spi_sp_write_per_name('" + tag + "','" + workstation + "','" + value + "')";
            var result = await ExecuteCommand(command);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write(result.ToString());
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            return true;
        }

        public void ReadTable()
        {
            string command = string.Empty;
            try
            {
                _table = new List<dynamic>();
                 command = "SELECT \"TagValue\",lower(\"TagName\") FROM public.\"SPI_TB_IL_ADDRESS\"";
                
                var result = ExecuteCommand(command).Result;
                 foreach(var tag in result)
                {
                 var details = ((IDictionary<string, object>)tag);   
                    var t = new {
                        tagName = details["lower"].ToString(),
                        tagValue = (details["TagValue"] == null)?"": details["TagValue"].ToString() 
                    };

                    _table.Add(t);
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Não encontrado o valore da tag ");
                Console.WriteLine(ex.ToString());
            }
           
        }
        // public async Task<string> Read(string tagName)
        // {
        //     string command = string.Empty;
        //     try
        //     {
        //          command = "SELECT \"TagValue\" FROM public.\"SPI_TB_IL_ADDRESS\" WHERE lower(\"TagName\") = '"+ tagName +"'";
        //         var result = await ExecuteCommand(command);
        //         var details = ((IDictionary<string, object>)result.AsList()[0]);            
        //         return details["TagValue"].ToString();
        //     }
        //     catch (System.Exception)
        //     {
        //         Console.WriteLine("Não encontrado o valore da tag " + tagName);
        //         return string.Empty;
        //     }
           
        // }

        public async Task<string> Read(string tagName)
        {
            string command = string.Empty;
            try
            {
                
                var result = _table.Where(x=>x.tagName == tagName.ToLower()).FirstOrDefault();
               
               if(result == null)
               {
                   Console.WriteLine("Não encontrado o valore da tag " + tagName.ToLower());
                return string.Empty;
               }

               return result.tagValue.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Não encontrado o valore da tag " + tagName);
                Console.WriteLine(ex);
                return string.Empty;
            }
           
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
    }
}