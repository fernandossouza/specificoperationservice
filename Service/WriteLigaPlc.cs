using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using specificoperationservice.Model;
using specificoperationservice.Service.Interface;

namespace specificoperationservice.Service
{
    public class WriteLigaPlc : IWriteLigaPlc
    {
        private readonly IConfiguration _configuration;
        private readonly IInterlevelDb _interleverDb;

         public WriteLigaPlc(IConfiguration configuration, IInterlevelDb interlevelDb)
        {
            _configuration = configuration;
            _interleverDb = interlevelDb;
        }

        public async Task<(bool,string)> HabilitaForno(int Posicao)
        {
            string tagName = string.Empty;
            string workStation = _configuration["WorkStation"];
            if(Posicao == 1)
                tagName = _configuration["HabilitaForno1"];
            else if(Posicao == 2)
                tagName = _configuration["HabilitaForno2"];
            else
                return (false,"Não encontrado forno nesta posição");
            
            var tagValue = await _interleverDb.Read(tagName);

            var tagNewValue = WriteBit(Convert.ToInt32(tagValue),3).ToString();

            if(! await _interleverDb.Write(tagNewValue,tagName,workStation))
                return(false,"Erro ao tentar escrever no banco de dados");
            
            return(true,string.Empty);
        }

        public async Task<(bool,string)> IniciaForno(ProductionOrder productionOrder)
        {
            string tagName = string.Empty;            
            string workStation = _configuration["WorkStation"];

            int Posicao = 0;

            if(productionOrder.currentThing.thingName.IndexOf("1")>0)
                Posicao = 1;
            else if(productionOrder.currentThing.thingName.IndexOf("2")>0)
                Posicao = 2;

            var(returnHabilitaForno,stringErro) = await HabilitaForno(Posicao);
            System.Threading.Thread.Sleep(500);
            if(!returnHabilitaForno)
                return(false,stringErro);

            if(Posicao == 1)
                tagName = _configuration["IniciaForno1"];
            else if(Posicao == 2)
                tagName = _configuration["IniciaForno2"];
            else
                return (false,"Não encontrado forno nesta posição");
            
            var tagValue = await _interleverDb.Read(tagName);

            var tagNewValue = WriteBit(Convert.ToInt32(tagValue),0).ToString();

            if(! await _interleverDb.Write(tagNewValue,tagName,workStation))
                return(false,"Erro ao tentar escrever no banco de dados");            


            return(true,string.Empty);

        }
        
        public async Task<(bool,string)> FinalizaForno(int Posicao)
        {
            string tagName = string.Empty;
            string workStation = _configuration["WorkStation"];
            if(Posicao == 1)
                tagName = _configuration["FinalizaForno1"]; 
            else if(Posicao == 2)
                tagName = _configuration["FinalizaForno2"];
            else
                return (false,"Não encontrado forno nesta posição");
            
            var tagValue = await _interleverDb.Read(tagName);

            var tagNewValue = WriteBit(Convert.ToInt32(tagValue),1).ToString();

            if(! await _interleverDb.Write(tagNewValue,tagName,workStation))
                return(false,"Erro ao tentar escrever no banco de dados");
            
            return(true,string.Empty);

        }

        public async Task<(bool,string)> AddCobreFosforosoForno(ProductionOrder productionOrder)
        {
            string tagName = string.Empty;
            string workStation = _configuration["WorkStation"];

            int Posicao = 0;

            if(productionOrder.currentThing.thingName.IndexOf("1")>0)
                Posicao = 1;
            else if(productionOrder.currentThing.thingName.IndexOf("2")>0)
                Posicao = 2;

            if(Posicao == 1)
                tagName = _configuration["AddCobreForno1"]; 
            else if(Posicao == 2)
                tagName = _configuration["AddCobreForno2"];
            else
                return (false,"Não encontrado forno nesta posição");
            
            var tagValue = await _interleverDb.Read(tagName);

            var tagNewValue = WriteBit(Convert.ToInt32(tagValue),2).ToString();

            if(! await _interleverDb.Write(tagNewValue,tagName,workStation))
                return(false,"Erro ao tentar escrever no banco de dados");
            
            return(true,string.Empty);

        }
        public async Task<(bool,string)> LabOkForno(ProductionOrder productionOrder)
        {
            string tagName = string.Empty;
            string workStation = _configuration["WorkStation"];

            int Posicao = 0;

            if(productionOrder.currentThing.thingName.IndexOf("1")>0)
                Posicao = 1;
            else if(productionOrder.currentThing.thingName.IndexOf("2")>0)
                Posicao = 2;

            if(Posicao == 1)
                tagName = _configuration["LabOkForno1"]; 
            else if(Posicao == 2)
                tagName = _configuration["LabOkForno2"];
            else
                return (false,"Não encontrado forno nesta posição");
            
            var tagValue = await _interleverDb.Read(tagName);

            var tagNewValue = WriteBit(Convert.ToInt32(tagValue),2).ToString();

            if(! await _interleverDb.Write(tagNewValue,tagName,workStation))
                return(false,"Erro ao tentar escrever no banco de dados");
            
            return(true,string.Empty);

        }

        private int WriteBit(int oldValue,int positionWrite)
        {
            string sNewValue = string.Empty;
            BitArray bit = new BitArray(new int[] { oldValue });

            bit[positionWrite] = true;

            for (int i = bit.Length -1; i >= 0; i--)
            {
                sNewValue += (bit[i]== true)?"1":"0" ;
            }

            var newValue = Convert.ToInt32(sNewValue, 2);

            return newValue;
        }


    }
}