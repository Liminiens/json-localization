using System.ComponentModel.DataAnnotations;

namespace JsonFileLocalization.Example.Model
{
    public class IndexModel
    {
        [Required(ErrorMessage = "Property.Message")]
        public string Value { get; set; }
    }
}
