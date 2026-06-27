using System;
using System.Collections.Generic;

namespace LibraryManagment.Models.Entities;

public partial class Book
{
    public int Id { get; set; }

    public string BookName { get; set; } = null!;

    public double? BookRating { get; set; }

    public int AuthorId { get; set; }

    public virtual Author? Author { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
