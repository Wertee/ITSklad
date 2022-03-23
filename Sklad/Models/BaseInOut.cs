using System;

namespace Sklad.Models
{
    public abstract class BaseInOut
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
    }
}