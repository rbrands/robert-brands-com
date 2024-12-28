using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace robert_brands_com.Models
{
    public class Article : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "listName"), Display(Name = "News-Liste", Prompt = "Name der News-Liste."), MaxLength(80)]
        public string ListName { get; set; } = "News";
        [JsonProperty(PropertyName = "date"), Display(Name = "Datum"), UIHint("Date"), Required]
        public DateTime Date { get; set; } = DateTime.Today;
        [JsonProperty(PropertyName = "author", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Autor")]
        public string Author { get; set; }
        [JsonProperty(PropertyName = "nickname", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Spitzname", Prompt = "Name wie er als Autor angezeigt werden soll."), MaxLength(30, ErrorMessage = "Spitzname bitte nicht länger als 30 Zeichen.")]
        public string Nickname { get; set; }
        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel", Prompt = "Titel des Artikels"), MaxLength(240, ErrorMessage = "Titel zu lang."), Required(ErrorMessage = "Bitte Titel eingeben.")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "subTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Untertitel", Prompt = "Untertitel des Artikels"), MaxLength(512, ErrorMessage = "Untertitel zu lang.")]
        public string SubTitle { get; set; }
        [JsonProperty(PropertyName = "urlTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel-Link", Prompt = "Kurztitel wie er in der Url auftaucht"), MaxLength(80, ErrorMessage = "Kurztitel zu lang.")]
        [RegularExpression("[a-z0-9-_]*", ErrorMessage = "Bitte nur Kleinbuchstaben und Zahlen für den Titel-Link eingeben.")]
        public string UrlTitle { get; set; }
        [JsonProperty(PropertyName = "summary", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Zusammenfassung", Prompt = "Kurze Zusammenfassung des Artikels"), MaxLength(10000, ErrorMessage = "Zusammenfassung zu lang.")]
        public string Summary { get; set; }
        [JsonProperty(PropertyName = "plainSummary", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Zusammenfassung für Facebook", Prompt = "Kurze Zusammenfassung des Artikels für Facebook und Co"), MaxLength(10000, ErrorMessage = "Zusammenfassung zu lang.")]
        public string PlainSummary { get; set; }
        [JsonProperty(PropertyName = "articleContent"), Display(Name = "Artikel"), MaxLength(64000, ErrorMessage = "Der Text ist zu lang.")]
        public string ArticleContent { get; set; }
        [JsonProperty(PropertyName = "translationIsEnabled"), Display(Name = "Übersetzung?")]
        public Boolean TranslationIsEnabled { get; set; }
        [JsonProperty(PropertyName = "tags", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Tags zum Artikel")]
        public string Tags { get; set; }
        [JsonProperty(PropertyName = "featuredLevel", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Featured Level", Prompt = "Falls Artikel gefeatured werden soll - Level 1 - 3")]
        public int FeaturedLevel { get; set; } = 0;
        [DataType(DataType.Url, ErrorMessage = "Bitte eine gültige URL eingeben.")]
        [JsonProperty(PropertyName = "link", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Link", Prompt = "Optionaler Link zu weiterführenden Infos."), UIHint("Url")]
        public string Link { get; set; }
        [JsonProperty(PropertyName = "linkTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel des Links", Prompt = "Titel zu dem weiterführenden Link."), MaxLength(80, ErrorMessage = "Titel zu dem Link zu lang.")]
        public string LinkTitle { get; set; }
        [JsonProperty(PropertyName = "imageLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titelbild", Prompt = "Optionaler Link zu einem Logo-Image"), UIHint("Url")]
        public string ImageLink { get; set; }
        [JsonProperty(PropertyName = "flickrLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Flickr-URL", Prompt = "Link zu einem Flickr-Album oder Bild"), UIHint("Url")]
        public string FlickrLink { get; set; }
        [JsonProperty(PropertyName = "flickrLinkImage", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Image zur Flickr-URL", Prompt = "Link zu einem Bild zum Flickr-Album oder Bild"), UIHint("Url")]
        public string FlickrLinkImage { get; set; }
        [JsonProperty(PropertyName = "flickrHeader", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Flickr-Header", Prompt = "Soll der Flickr-Header angezeigt werden?")]
        public bool FlickrHeader { get; set; }
        [JsonProperty(PropertyName = "flickrFooter", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Flickr-Footer", Prompt = "Soll der Flickr-Footer angezeigt werden?")]
        public bool FlickrFooter { get; set; }
        [JsonProperty(PropertyName = "comments", NullValueHandling = NullValueHandling.Ignore)]
        public Comment[] Comments { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
