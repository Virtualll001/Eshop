﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Data.Models
{
    public class CategoryProduct
    {
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
