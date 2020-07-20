using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mapping
{
    public class ObjectMapper : Profile
    {
        public ObjectMapper()
        {
            CreateMap<Domain.Car, Models.Domain.Car>();
            CreateMap<Models.Domain.Car, Domain.Car>();
            CreateMap<Domain.RefreshToken, Models.Domain.RefreshToken>();
            CreateMap<Models.Domain.RefreshToken, Domain.RefreshToken>();
        }
    }
}
