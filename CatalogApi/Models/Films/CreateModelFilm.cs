﻿using System;
using System.ComponentModel.DataAnnotations;
using CatalogApi.Entities;

namespace CatalogApi.Models.Films
{
    public class CreateModelFilm
    {
        public string _category;
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreateFilm { get; set; }
        
        [EnumDataType(typeof(Сategory))]
        public string Category
        {
            get => _category;
            set => _category = value;
        }
    }
}