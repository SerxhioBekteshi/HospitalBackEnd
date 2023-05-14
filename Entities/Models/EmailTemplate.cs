using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class EmailTemplate : BaseCreatedAndModified
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}