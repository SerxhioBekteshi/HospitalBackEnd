using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class TimeZoneDetails
{
    [Key]
    public int Id { get; set; }
    [MaxLength(5)]
    public string Abbreviation { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(20)]
    public string GmtOffset { get; set; }
}