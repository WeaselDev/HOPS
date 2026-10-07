using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public class BrewRecipe : NamedModel
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Style))]
        public string Style { get; set; } = string.Empty;

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Description))]
        public string Description { get; set; } = string.Empty;

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.IsDeleted))]
        public bool IsDeleted { get; set; }

        public List<BrewRecipeVersion> Versions { get; set; } = [];
    }
}