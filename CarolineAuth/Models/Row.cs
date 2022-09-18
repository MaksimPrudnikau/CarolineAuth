
using Microsoft.EntityFrameworkCore;

namespace CarolineAuth.Models;

public sealed class Row
{
    public int Id { get; set; }
    public string Japanese { get; set; } = string.Empty;
    public string English { get; set; } = string.Empty;
}