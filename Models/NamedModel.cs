using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public class NamedModel : Model
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Name))]
        public string Name { get; set; } = string.Empty;
    }
}