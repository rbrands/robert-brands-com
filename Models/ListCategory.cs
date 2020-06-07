using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace robert_brands_com.Models
{
    public class ListCategory : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "listName"), Display(Name = "Liste", Prompt = "Name der Liste."), MaxLength(80)]
        public string ListName { get; set; }
        [JsonProperty(PropertyName = "category"), Display(Name = "Bitte Namen der Kategorie eingeben."), MaxLength(40, ErrorMessage = "Kategorie zu lang."), Required(ErrorMessage = "Bitte Kategorienamen eingeben.")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "categoryDisplay", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Bitte Anzeigetext für Kategorie eingeben."), MaxLength(80, ErrorMessage = "Kategorieanzeigetext zu lang.")]
        public string CategoryDisplay { get; set; }
    }
}
