using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace robert_brands_com.Models
{
    public class CommentedLinkItem : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "listName"), Display(Name = "Albumname", Prompt = "Name des Albums, z.B. Fotosammlung oder Scuderia"), MaxLength(80)]
        public string ListName { get; set; }
        [JsonProperty(PropertyName = "category"), Display(Name = "Kategorie"), Required]
        public string Category { get; set; } = "Sonstiges";
        [JsonProperty(PropertyName = "date"), Display(Name = "Datum"), UIHint("Date"), Required]
        public DateTime Date { get; set; } = DateTime.Today;
        [JsonProperty(PropertyName = "author"), Display(Name = "Autor")]
        public string Author { get; set; }
        [JsonProperty(PropertyName = "roles"), Display(Name = "Rollen")]
        public string[] Roles { get; set; }
        [JsonProperty(PropertyName = "title"), Display(Name = "Titel", Prompt = "Titel für Link eingeben"), Required(ErrorMessage = "Bitte Titel eingeben.")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "shortTitle"), Display(Name = "Kurztitel")]
        public string ShortTitle { get; set; }
        [JsonProperty(PropertyName = "urlTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel-Link", Prompt = "Kurztitel wie er in der Url auftaucht"), MaxLength(80, ErrorMessage = "Kurztitel zu lang.")]
        [RegularExpression("[a-z0-9-_]*", ErrorMessage = "Bitte nur Kleinbuchstaben und Zahlen für den Titel-Link eingeben.")]
        public string UrlTitle { get; set; }
        [JsonProperty(PropertyName = "description"), Display(Name = "Beschreibung"), MaxLength(2048, ErrorMessage = "Die Beschreibung ist zu lang.")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "plainDescription"), Display(Name = "Kurzbeschreibung", Prompt = "Kurze Beschreibung für Facebook und Co"), MaxLength(512, ErrorMessage = "Kurzbeschreibung ist zu lang.")]
        public string PlainDescription { get; set; }
        [JsonProperty(PropertyName = "longDescription"), Display(Name = "Ausführliche Beschreibung"), MaxLength(10000, ErrorMessage = "Die Beschreibung ist zu lang.")]
        public string LongDescription { get; set; }
        [JsonProperty(PropertyName = "link"), Display(Name = "Url"), Required(ErrorMessage = "Bitte einen Link eingeben."), UIHint("Url")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "imageLink"), Display(Name = "Bild-URL", Prompt = "Link zu einem Image"), UIHint("Url")]
        public string ImageLink { get; set; }
        [JsonProperty(PropertyName = "infos")]
        public ContentItem[] Infos { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public Comment[] Comments { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class Comment : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }
        [JsonProperty(PropertyName = "commentText")]
        public string CommentText { get; set; }
    }

}
