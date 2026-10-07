using HOPS.Properties;
using System.ComponentModel.DataAnnotations;

namespace HOPS.Models
{
    public class MashStep : IndexedNamedModel
    {
        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Temperature))]
        public double Temperature { get; set; }

        [Display(ResourceType = typeof(Resource), Name = nameof(Resource.Duration))]
        public int Duration { get; set; }

        public MashStep CreateCopy()
        {
            return new MashStep
            {
                Index = Index,
                Name = Name,
                Temperature = Temperature,
                Duration = Duration
            };
        }
    }
}