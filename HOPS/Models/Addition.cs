using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public abstract class Addition : IndexedNamedModel
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Unit))]
        public string Unit { get; set; } = string.Empty;

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Amount))]
        public decimal Amount { get; set; }
    }
}