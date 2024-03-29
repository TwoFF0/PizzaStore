﻿using PizzaStore.Entities;
using System.Collections.Generic;

namespace PizzaStore.DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<ProductSizeDto> ProductSize { get; set; }
    }
}
