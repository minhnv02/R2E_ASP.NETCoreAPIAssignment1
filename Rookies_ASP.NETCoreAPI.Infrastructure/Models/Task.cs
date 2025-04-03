using System.ComponentModel.DataAnnotations;

namespace Rookies_ASP.NETCoreAPI.Infrastructure.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public bool Status { get; set; }
    }
}
