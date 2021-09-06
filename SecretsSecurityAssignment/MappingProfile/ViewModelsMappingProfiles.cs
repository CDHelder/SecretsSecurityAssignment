using AutoMapper;
using SecretsSecurityAssignment.Core;
using SecretsSecurityAssignment.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.WebApi.MappingProfile
{
    public class ViewModelsMappingProfiles : Profile
    {
        public ViewModelsMappingProfiles()
        {
            CreateMap<SensitiveSecret, Secret>();
            CreateMap<StateSecret, Secret>();
            CreateMap<TopSecret, Secret>();

            CreateMap<Secret, SensitiveSecret>();
            CreateMap<Secret, StateSecret>();
            CreateMap<Secret, TopSecret>();
        }
    }
}
