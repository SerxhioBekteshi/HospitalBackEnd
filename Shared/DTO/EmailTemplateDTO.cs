namespace Shared.DTO;

public class EmailTemplateDTO
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CreatedBy { get; set; }
    public string? CreatedByFullName { get; set; }
    public DateTime? DateModified { get; set; }
    public int? ModifiedBy { get; set; }
    public string? ModifiedByFullName { get; set; }
}