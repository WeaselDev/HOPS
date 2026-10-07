using System.ComponentModel.DataAnnotations;
using HOPS.Properties;

namespace HOPS.Models
{
    public class Model
    {
        public Guid Id { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.IsDestroyed))]
        public bool IsDestroyed { get; set; }
    }
}