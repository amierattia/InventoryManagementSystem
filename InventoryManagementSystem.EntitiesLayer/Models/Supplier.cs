﻿using InventoryManagementSystem.EntitiesLayer.Models;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string Email { get; set; }
    public string phone { get; set; }

    public ICollection<Product>? Products { get; set; } 
}
