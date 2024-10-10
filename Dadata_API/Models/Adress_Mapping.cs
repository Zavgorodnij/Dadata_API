using System;
using AutoMapper;

namespace Dadata_API.Models
{
    public class Adress_Mapping : Profile
    {
        public Adress_Mapping() 
        {
            CreateMap<AdressModel, AdressModelDTO>().ReverseMap();
        }
    }
}
