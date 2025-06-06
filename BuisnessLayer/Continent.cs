﻿using System;
using System.Collections.Generic;

namespace BusinessLayer;

public partial class Continent
{
        public string ContinentCode { get; set; } = null!;

        public string ContinentName { get; set; } = null!;

        public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

        public virtual ICollection<Trophy> Trophies { get; set; } = new List<Trophy>();
}
