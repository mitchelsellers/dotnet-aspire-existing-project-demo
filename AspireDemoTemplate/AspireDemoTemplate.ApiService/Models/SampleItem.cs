using System.ComponentModel.DataAnnotations;

namespace AspireDemoTemplate.ApiService.Models;

public class SampleItem
{
    public int SampleItemId { get; set; }
    [Required]
    [StringLength(100)]
    public string SampleName { get; set; } = null!;
}
