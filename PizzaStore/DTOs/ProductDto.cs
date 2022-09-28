﻿namespace PizzaStore.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public byte[] Image { get; set; }
    }
}
