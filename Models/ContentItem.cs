using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace robert_brands_com.Models
{
    public class ContentItem
    {
        [JsonProperty(PropertyName = "contentType"), Display(Name = "Inhaltstyp", Prompt = "Type des Inhalts auswählen"), Required(ErrorMessage = "Bitte einen Inhaltstyp auswählen")]
        public ContentType ContentType { get; set; }
        [JsonProperty(PropertyName = "header", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Header", Prompt = "Kurze Überschrift"), MaxLength(120, ErrorMessage = "Kurzüberschrift zu lang.")]
        public string Header { get; set; }
        [JsonProperty(PropertyName = "columnWidth", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Anzahl Spalten (von 12))", Prompt = "Spaltenanzahl zwischen 2 und 12 eingeben"),]
        [Range(2, 12, ErrorMessage = "Bitte als Spaltenweite einen Wert zwischen 2 und 12 eingeben.")]
        public int ColumnWidth { get; set; } = 4;
        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel", Prompt = "Titel des Infoblocks"), MaxLength(120, ErrorMessage = "Titel zu lang.")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "uniqueId", NullValueHandling = NullValueHandling.Ignore)]
        public string UniqueId { get; set; }
        [JsonProperty(PropertyName = "sortOrder"), Display(Name = "Sortierwert (0 für Headline)", Prompt = "Ordinalwert für die Sortierreihenfolge, 0 für Headline"), Required(ErrorMessage = "Bitte einen Sortierwert eingeben.")]
        public int SortOrder { get; set; }
        [JsonProperty(PropertyName = "description"), Display(Name = "Beschreibung"), MaxLength(5000, ErrorMessage = "Die Beschreibung ist zu lang.")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "imageLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "URL für Inhaltsbild", Prompt = "Link zum Inhaltsbild"), DataType(DataType.ImageUrl, ErrorMessage = "Bitte URL zu gültigem Image eingeben.")]
        public string ImageLink { get; set; }
        [JsonProperty(PropertyName = "embeddedLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "URL für Inhalt", Prompt = "Link zum Inhaltselement"), UIHint("Url")]
        public string EmbeddedLink { get; set; }
        [JsonProperty(PropertyName = "link", NullValueHandling = NullValueHandling.Ignore), Display(Name = "URL für weiteren Inhalt", Prompt = "Link zu weiterem Inhalt"), DataType(DataType.Url, ErrorMessage = "Bitte eine gültige URL als Link eingeben.")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "linkTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel des Links", Prompt = "Bezeichnung zum Link"), MaxLength(40, ErrorMessage = "Link zu lang.")]
        public string LinkTitle { get; set; }
        [JsonIgnore]
        public string ReferenceId { get; set; }
    }

    public enum ContentType
    {
        Text = 0,
        Flickr = 1,
        [Display(Name = "Flickr mit Header")]
        FlickrWithHeader = 2,
        [Display(Name = "Flickr mit Header und Footer")]
        FlickrWithHeaderAndFooter = 8,
        [Display(Name = "Flickr mit Footer")]
        FlickrWithFooter = 9,
        [Display(Name = "Link mit Bild")]
        EmbeddedLink = 3,
        Gpsies = 4,
        Vimeo = 5,
        Strava = 6,
        Lightroom = 7
    }
}
