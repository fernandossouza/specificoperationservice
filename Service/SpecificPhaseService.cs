using specificoperationservice.Service.Interface;
using specificoperationservice.Model.SpecificPhase;
using specificoperationservice.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace specificoperationservice.Service
{
    public class SpecificPhaseService : ISpecificPhaseService
    {
        public readonly IOtherApi _otherApiService;
        public SpecificPhaseService(IOtherApi otherApiService)
        {
            _otherApiService = otherApiService;
        }

        public async Task<SpecificParameter> AddParameter(SpecificParameter specificParameter)
        {
            var thingGroup = await _otherApiService.GetThingGroup(specificParameter.thingGroupId);
            List<PhaseParameter> PhaseParameterList = new List<PhaseParameter>();
            // LSE
            var parameterLSE = ReturnIdTag(specificParameter.tagGroup,"lse",thingGroup.tags.ToList());
            parameterLSE.setupValue = specificParameter.LSE.ToString();
            PhaseParameterList.Add(parameterLSE);
            //LSC
            var parameterLSC = ReturnIdTag(specificParameter.tagGroup,"lsc",thingGroup.tags.ToList());
            parameterLSC.setupValue = specificParameter.LSC.ToString();
            PhaseParameterList.Add(parameterLSC);
            //LIC
            var parameterLIC = ReturnIdTag(specificParameter.tagGroup,"lic",thingGroup.tags.ToList());
            parameterLIC.setupValue = specificParameter.LIC.ToString();
            PhaseParameterList.Add(parameterLIC);
            //LIE
            var parameterLIE = ReturnIdTag(specificParameter.tagGroup,"lie",thingGroup.tags.ToList());
            parameterLIE.setupValue = specificParameter.LIE.ToString();
            PhaseParameterList.Add(parameterLIE);
            //Nominal
            var parameterVN = ReturnIdTag(specificParameter.tagGroup,"vn",thingGroup.tags.ToList());
            parameterVN.setupValue = specificParameter.value.ToString();
            PhaseParameterList.Add(parameterVN);
            //unidade
            var parameterUN = ReturnIdTag(specificParameter.tagGroup,"unidade",thingGroup.tags.ToList());
            parameterUN.setupValue = specificParameter.Unit.ToString();
            PhaseParameterList.Add(parameterUN);

            foreach (var phaseParameter in PhaseParameterList)
            {                
                var phaseParameterCreated = await _otherApiService.PostPhaseParameter(phaseParameter);
            }

            return specificParameter;
        }

        public async Task<SpecificPhase> GetSpecificPhase(int phaseId)
        {
            SpecificPhase specificPhase = new SpecificPhase();
            specificPhase.parameters = new List<SpecificParameter>();
            var phase = await _otherApiService.GetPhase(phaseId);
            var phaseParameters = phase.phaseParameters.ToList();

            var phaseParameterDistinct = phaseParameters.GroupBy(x=>x.tag.tagGroup);

            foreach (var phaseParameterGroup in phaseParameterDistinct)
            {

                SpecificParameter specificParameter = new SpecificParameter();
                
                foreach (var phaseParameter in phaseParameterGroup)
                {
                    specificParameter.tagGroup = phaseParameter.tag.tagGroup;
                    specificParameter.thingGroupId = phaseParameter.tag.thingGroupId;

                    switch (phaseParameter.tag.tagDescription)
                    {
                        case "vn":
                            specificParameter.value = phaseParameter.setupValue;
                        break;
                        case "lse":
                            specificParameter.LSE = phaseParameter.setupValue;
                        break;
                        case "lsc":
                            specificParameter.LSC = phaseParameter.setupValue;
                        break;
                        case "lic":
                            specificParameter.LIC = phaseParameter.setupValue;
                        break;
                        case "lie":
                            specificParameter.LIE = phaseParameter.setupValue;
                        break;
                        case "unidade":
                            specificParameter.Unit = phaseParameter.setupValue;
                        break;
                    }
                }

                    specificPhase.parameters.Add(specificParameter);
                
            }

            return specificPhase;
        }

        public async Task<(SpecificParameter,string)> UpdateSpecificParameter(int phaseId,SpecificParameter specificParameter)
        {
            var (status,stringErro) =  await DeleteSpecificParameter(phaseId,specificParameter);
            if(status)
            {
                specificParameter = await AddParameter(specificParameter);
                return(specificParameter,string.Empty);
            }
            return(specificParameter,stringErro);
        }

        public async Task<(bool,string)> DeleteSpecificParameter(int phaseId,SpecificParameter specificParameter)
        {
            var phase = await _otherApiService.GetPhase(phaseId);
            var phaseParameterList = phase.phaseParameters.ToList();

            var phaseParameterDistinct = phaseParameterList.GroupBy(x=>x.tag.tagGroup);

            var phaseParameterDeleteList = phaseParameterDistinct.Where(x=>x.Key == specificParameter.tagGroup).FirstOrDefault();

            foreach (var phaseParameter in phaseParameterDeleteList)
            {
                if(! await _otherApiService.DeletePhaseParameter(phaseId,phaseParameter))
                    return (false,"Erro in Delete Tag");
            }
            return (true,string.Empty);
        }

        private PhaseParameter ReturnIdTag (string tagGroup,string tagDescription,List<Tag> tagList)
        {
            var tag = tagList.Where(x => x.tagGroup.ToLower() == tagGroup.ToLower()
                                        && x.tagDescription == tagDescription).FirstOrDefault();

            if(tag == null)
                return null;

            PhaseParameter phaseParameter = new PhaseParameter();
            phaseParameter.tagId = tag.tagId;
            return phaseParameter;
        }
        
    }
}