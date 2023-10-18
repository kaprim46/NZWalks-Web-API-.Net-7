using System;
using Microsoft.AspNetCore.Identity;

namespace NZWalksApi.Repository.IRepository
{
	public interface ITokenRepository
	{
		string CreateJwtToken(IdentityUser user, List<string> roles);
	}
}

