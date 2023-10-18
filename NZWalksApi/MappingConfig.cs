using System;
using AutoMapper;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;

namespace NZWalksApi
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<Region, RegionCreateDTO>().ReverseMap();
            CreateMap<Region, RegionUpdateDTO>().ReverseMap();

            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyCreateDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyUpdateDTO>().ReverseMap();

            CreateMap<Walk, WalksDTO>().ReverseMap();
            CreateMap<Walk, WalksCreateDTO>().ReverseMap();
            CreateMap<Walk, WalksUpdateDTO>().ReverseMap();
        }
	}
}

