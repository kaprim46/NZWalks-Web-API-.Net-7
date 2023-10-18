using System;
using NZWalksApi.Models.Domain;

namespace NZWalksApi.Models.DTO
{
	public class WalksDTO
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImage { get; set; }


        public DifficultyDTO Difficulty { get; set; }
        public RegionDTO Region { get; set; }
    }
}

