﻿using AutoMapper;

namespace Session1.Api.Services.Mappers
{
    public class AnimalMapper : Profile
    {
        public AnimalMapper()
        {
            CreateMap<Dal.Dto.Animal, Model.Animal>();
            CreateMap<Model.Animal, Dal.Dto.Animal>();
        }
    }
}
