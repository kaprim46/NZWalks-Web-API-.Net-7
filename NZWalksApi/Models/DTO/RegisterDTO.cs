﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NZWalksApi.Models.DTO
{
	public class RegisterDTO
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

		public string[] Roles { get; set; }
	}
}

