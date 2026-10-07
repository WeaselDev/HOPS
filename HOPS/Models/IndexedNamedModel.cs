using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public class IndexedNamedModel : NamedModel
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Index))]
        public int Index { get; set; } = 1;
    }
}