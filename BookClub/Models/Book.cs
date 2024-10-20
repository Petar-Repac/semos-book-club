using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookClub.Models;

public class Book
{
    [JsonIgnore]
    [BindNever]
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Author { get; set; } = String.Empty;
    public string Genre { get; set; } = String.Empty;
    public DateTime PublishDate { get; set; }
}