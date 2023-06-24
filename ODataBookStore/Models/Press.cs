﻿using System.ComponentModel.DataAnnotations;

namespace ODataBookStore.Models
{
    public class Press
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }

    }
    public enum Category
    {
        Book,
        Magazine,
        Ebook
    }
}
