using System;
using System.Collections.Generic;
using System.Linq;
using CatalogApi.Entities;

namespace CatalogApi.Models.Films;

public class ParametersForSearching 
{
    public int MinYear { get; set; } = 1900;

    public int MaxYear { get; set; } = DateTime.Now.Year;

    public double MinScore { get; set; } = 0.0;

    public double MaxScore { get; set; } = 10.0;

    public IEnumerable<Category> Categories { get; set; } = (IEnumerable<Category>)Enum.GetValues(typeof(Category));
}