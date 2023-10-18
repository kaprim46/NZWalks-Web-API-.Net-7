using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NZWalksUI.Models.DTO;

namespace NZWalksUI.Models
{
	public class AddWalksViewModel
	{
        public WalksCreateDTO walksCreateDTO { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> RegionsList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> DifficultiesList { get; set; }
    }
}

