using System;
using NZWalksApi.Models.Domain;

namespace NZWalksApi.Repository
{
	public interface IImageRepository
	{
		Task<Image>Upload(Image image);
	}
}

