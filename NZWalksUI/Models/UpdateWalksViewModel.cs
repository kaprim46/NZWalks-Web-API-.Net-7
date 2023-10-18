using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NZWalksUI.Models.DTO;

namespace NZWalksUI.Models
{
	public class UpdateWalksViewModel
    {
        public WalksUpdateDTO walksUpdateDTO { get; set; }
        public WalksDTO WalksDTO { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> RegionsList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> DifficultiesList { get; set; }
    }
}

