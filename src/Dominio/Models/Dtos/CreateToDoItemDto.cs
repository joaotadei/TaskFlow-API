﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.Dtos
{
    public class CreateToDoItemDto
    {
        [Required]
        public string Description { get; set; }
        
        [Required]
        public DateTime Expiration { get; set; }
    }
}