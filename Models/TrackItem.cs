using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace robert_brands_com.Models
{
    public class TrackItem : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "listName"), Display(Name = "Liste", Prompt = "Name der Liste."), MaxLength(80)]
        public string ListName { get; set; } = "Ausfahrten";
        [JsonProperty(PropertyName = "category"), Display(Name = "Kategorie"), Required]
        public string Category { get; set; } = "Sonstiges";
        // Only for displaying not for storing
        [JsonIgnore]
        public string CategoryDisplay { get; set; }
        [JsonProperty(PropertyName = "date"), Display(Name = "Datum"), UIHint("Date"), Required]
        public DateTime Date { get; set; } = DateTime.Today;
        [JsonProperty(PropertyName = "author"), Display(Name = "Autor")]
        public string Author { get; set; }
        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel", Prompt = "Titel für Link eingeben"), MaxLength(120, ErrorMessage = "Titel zu lang."), Required(ErrorMessage = "Bitte Titel eingeben.")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "shortTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Kurztitel"), MaxLength(80, ErrorMessage = "Kurztitel zu lang.")]
        public string ShortTitle { get; set; }
        [JsonProperty(PropertyName = "urlTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel-Link", Prompt = "Kurztitel wie er in der Url auftaucht"), MaxLength(80, ErrorMessage = "UrlTitel zu lang.")]
        [RegularExpression("[a-z0-9-_]*", ErrorMessage = "Bitte nur Kleinbuchstaben und Zahlen für den Titel-Link eingeben.")]
        public string UrlTitle { get; set; }
        [JsonProperty(PropertyName = "tourSet", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Touren-Url", Prompt = "Zusammenstellung mehrere Touren als URL"), MaxLength(80, ErrorMessage = "Touren-Url zu lang.")]
        [RegularExpression("[a-z0-9-_]*", ErrorMessage = "Bitte nur Kleinbuchstaben und Zahlen für den Titel-Link eingeben.")]
        public string TourSet { get; set; }
        [JsonProperty(PropertyName = "length", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Länge und Höhenmeter"), MaxLength(80, ErrorMessage = "Längenangabe zu lang.")]
        public string Length { get; set; }
        [JsonProperty(PropertyName = "description"), Display(Name = "Beschreibung"), MaxLength(5000, ErrorMessage = "Die Beschreibung ist zu lang.")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "plainDescription"), Display(Name = "Kurzbeschreibung", Prompt = "Kurze Beschreibung für Facebook und Co"), MaxLength(512, ErrorMessage = "Kurzbeschreibung ist zu lang.")]
        public string PlainDescription { get; set; }
        [JsonProperty(PropertyName = "photosLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Link zu Fotos"), UIHint("Url")]
        public string PhotosLink { get; set; }
        [JsonProperty(PropertyName = "flickrLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Flickr-URL", Prompt = "Link zu einem Flickr-Album oder Bild"), UIHint("Url")]
        public string FlickrLink { get; set; }
        [JsonProperty(PropertyName = "flickrLinkImage", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Image zur Flickr-URL", Prompt = "Link zu einem Bild zum Flickr-Album oder Bild"), UIHint("Url")]
        public string FlickrLinkImage { get; set; }
        [JsonProperty(PropertyName = "flickrHeader", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Flickr-Header", Prompt = "Soll der Flickr-Header angezeigt werden?")]
        public bool FlickrHeader { get; set; }
        [JsonProperty(PropertyName = "flickrFooter", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Flickr-Footer", Prompt = "Soll der Flickr-Footer angezeigt werden?")]
        public bool FlickrFooter { get; set; }
        [JsonProperty(PropertyName = "videoLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Link zu einem Video", Prompt = "Link zu einem Video"), UIHint("Url")]
        public string VideoLink { get; set; }
        [JsonProperty(PropertyName = "videoEmbedLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Video Embedded", Prompt = "Link zu einem eingebettetn Video"), UIHint("Url")]
        public string VideoEmbedLink { get; set; }
        [JsonProperty(PropertyName = "imageLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "URL für Titelbild", Prompt = "Link zu einem Logo-Image"), UIHint("Url")]
        public string ImageLink { get; set; }
        [JsonProperty(PropertyName = "gpsiesLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "GPSies/GPX", Prompt = "Link zu GPSies Track oder GPS-File"), UIHint("Url")]
        public string GpsiesLink { get; set; }
        [JsonProperty(PropertyName = "gpsiesEmbedLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "GPSies Embedded", Prompt = "Link zu GPSies Track zum Einbetten"), UIHint("Url")]
        public string GpsiesEmbedLink { get; set; }
        public bool IsGpxFile
        {
            get
            {
                return (!String.IsNullOrEmpty(GpsiesLink) && !GpsiesLink.Contains("gpsies"));
            }
        }
        [JsonProperty(PropertyName = "komootLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Komoot", Prompt = "Link zu Komoot Track"), UIHint("Url")]
        public string KomootLink { get; set; }
        [JsonProperty(PropertyName = "komootTourImage", NullValueHandling = NullValueHandling.Ignore), Display(Name = "URL für Komoot-Tourbild", Prompt = "Link zu einem Komoot-Image"), UIHint("Url")]
        public string KomootTourImage { get; set; }
        [JsonProperty(PropertyName = "stravaLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Strava", Prompt = "Link zu Strava"), UIHint("Url")]
        public string StravaLink { get; set; }
        [JsonProperty(PropertyName = "stravaEmbedLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Strava Embedded", Prompt = "Embedded Link zu Strava"), UIHint("Url")]
        public string StravaEmbedLink { get; set; }
        [JsonIgnore]
        public string Language { get; set; } = "de";
        [JsonProperty(PropertyName = "comments", NullValueHandling = NullValueHandling.Ignore)]
        public Comment[] Comments { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
