using System;
using System.ComponentModel.DataAnnotations;

namespace SpreadSheet.Models
{
    public class Cell
    {
        [Key]
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
