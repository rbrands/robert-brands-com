using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace robert_brands_com.Models
{
    public class CalendarItem : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "calendarName"), Display(Name = "Kalender", Prompt = "Name des Kalenders, z.B. Radevents, Wanderungen"), Required(ErrorMessage = "Bitte einen Kalender auswählen."), MaxLength(80)]
        public string CalendarName { get; set; }
        [JsonProperty(PropertyName = "title", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel", Prompt = "Titel der Veranstaltung"), MaxLength(120, ErrorMessage = "Titel zu lang."), Required(ErrorMessage = "Bitte Titel eingeben.")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "urlTitle", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Titel-Link", Prompt = "Kurztitel wie er in der Url auftaucht"), MaxLength(80, ErrorMessage = "Kurztitel zu lang.")]
        [RegularExpression("[a-z0-9-_]*", ErrorMessage = "Bitte nur Kleinbuchstaben und Zahlen für den Titel-Link eingeben.")]
        public string UrlTitle { get; set; }
        [JsonProperty(PropertyName = "startDate"), Display(Name = "Start", Prompt = "Bitte Startdatum eingeben."), Required(ErrorMessage = "Bitte den Beginn der Veranstaltung angeben.")]
        public DateTime StartDate { get; set; } = DateTime.Today;
        [JsonProperty(PropertyName = "endDate"), Display(Name = "Ende", Prompt = "Bitte den Endzeitpunkt der Veranstaltung eingeben."), Required(ErrorMessage = "Bitte den Endzeitpunkt der Veranstaltung angeben.")]
        public DateTime EndDate { get; set; } = DateTime.Today;
        [JsonProperty(PropertyName = "wholeDate"), Display(Name = "Ganztägig")]
        public bool WholeDay { get; set; }
        [JsonProperty(PropertyName = "place"), Display(Name = "Ort", Prompt = "Wo findet die Veranstaltung statt bzw. wo ist der Start"), MaxLength(256)]
        public string Place { get; set; }
        [JsonProperty(PropertyName = "host", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Gastgeber")]
        public string Host { get; set; }
        [JsonProperty(PropertyName = "summary", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Zusammenfassung", Prompt = "Kurze Zusammenfassung des Termins"), MaxLength(5000, ErrorMessage = "Zusammenfassung zu lang.")]
        public string Summary { get; set; }
        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Beschreibung", Prompt = "Ausführliche Beschreibung des Termins"), MaxLength(12000, ErrorMessage = "Beschreibung zu lang.")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "plainDescription"), Display(Name = "Kurzbeschreibung", Prompt = "Kurze Beschreibung für Facebook und Co"), MaxLength(512, ErrorMessage = "Kurzbeschreibung ist zu lang.")]
        public string PlainDescription { get; set; }
        [JsonProperty(PropertyName = "publicListing", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Öffentlich anzeigen", Prompt = "Termin öffentlich auflisten")]
        public bool PublicListing { get; set; }
        [JsonProperty(PropertyName = "registrationOpen", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Registrierung möglich", Prompt = "Registrierung offen")]
        public bool RegistrationOpen { get; set; }
        [JsonProperty(PropertyName = "registrationDescription"), Display(Name = "Einladungstext", Prompt = "Beschreibung zur Registrierung"), MaxLength(6000, ErrorMessage = "Einladungstext ist zu lang.")]
        public string RegistrationDescription { get; set; }
        [JsonProperty(PropertyName = "allowFriends", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Registrierung Freunde")]
        public bool AllowFriends { get; set; }
        [JsonProperty(PropertyName = "maxRegistrationsCount", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Maximale Anzahl Teilnehmer", Prompt = "Anzahl eingeben")]
        public int MaxRegistrationsCount { get; set; } = 20;
        [JsonProperty(PropertyName = "registrationKeyRequired", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Registratrion-Key erforderlich", Prompt = "Registrierung nur mit Schlüssel?")]
        public bool RegistrationKeyRequired { get; set; }
        [JsonProperty(PropertyName = "showRegistrations)", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Teilnehmer anzeigen")]
        public bool ShowRegistrations { get; set; }
        [JsonProperty(PropertyName = "imageLink", NullValueHandling = NullValueHandling.Ignore), Display(Name = "URL für Titelbild", Prompt = "Link zu einem Logo-Image"), UIHint("Url")]
        public string ImageLink { get; set; }
        [JsonProperty(PropertyName = "enableTranslations", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Übersetzungen")]
        public bool EnableTranslations { get; set; }
        [JsonIgnore]
        public string Language { get; set; } = "de";
        [JsonProperty(PropertyName = "infos")]
        public ContentItem[] Infos { get; set; }
        [JsonProperty(PropertyName = "registrationKeys")]
        public RegistrationKey[] RegistrationKeys { get; set; }
        [JsonProperty(PropertyName = "members")]
        public Member[] Members { get; set; }

        public int GetRegisteredMembersCount()
        {
            int count = 0;
            if (null != Members)
            {
                foreach (Member m in Members) count += m.Count;
            }
            return count;
        }
        public string GetMembersAsList()
        {
            StringBuilder sb = new StringBuilder();
            if (null != Members)
            {
                foreach (Member m in Members)
                {
                    sb.Append(m.Name);
                    if (m != Members.Last())
                    {
                        sb.Append(", ");
                    }
                }
            }
            return sb.ToString();
        }
        public string GetEventDateAsString()
        {
            string calendarString = String.Empty;
            if (WholeDay)
            {
                calendarString += StartDate.ToString("dd.MM.yyyy");
                if (StartDate.DayOfYear != EndDate.DayOfYear)
                {
                    calendarString += " - " + EndDate.ToString("dd.MM.yyyy");
                }
            }
            else
            {
                calendarString += StartDate.ToString("dd.MM.yyyy HH:mm") + " - ";
                if (StartDate.DayOfYear == EndDate.DayOfYear)
                {
                    calendarString += EndDate.ToString("HH:mm") + " Uhr";
                }
                else
                {
                    calendarString += EndDate.ToString("dd.MM.yyyy HH:mm") + " Uhr ";
                }
            }
            return calendarString;
        }
    }

    public class RegistrationKey
    {
        [JsonProperty(PropertyName = "uniqueId", NullValueHandling = NullValueHandling.Ignore)]
        public string UniqueId { get; set; }
        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore), Required(ErrorMessage = "Bitte einen Registration-Key eingeben."), MaxLength(50, ErrorMessage = "Der Registration-Key darf nicht länger als 50 Zeichen sein.")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public class Member
    {
        [JsonProperty(PropertyName = "uniqueId", NullValueHandling = NullValueHandling.Ignore)]
        public string UniqueId { get; set; }
        [JsonProperty(PropertyName = "registrationDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RegistrationDate { get; set; }
        [JsonProperty(PropertyName = "registrationKey", NullValueHandling = NullValueHandling.Ignore), Display(Name = "Registrierungsschlüssel", Prompt = "Separat erhaltenen Registrierungsschlüssel eingeben.")]
        public string RegistrationKey { get; set; }
        [JsonProperty(PropertyName = "name"), Display(Name = "Name"), Required(ErrorMessage = "Vor- und Nachname"), MaxLength(80, ErrorMessage = "Name zu lang.")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "email"), Display(Name = "E-Mail", Prompt = "E-Mail Adresse "), DataType(DataType.EmailAddress, ErrorMessage = "Bitte eine gültige E-Mail-Adresse eingeben."), Required(ErrorMessage = "Bitte eine E-Mail Adresse angeben."), MaxLength(80, ErrorMessage = "E-Mail zu lang.")]
        public string EMail { get; set; }
        [JsonProperty(PropertyName = "count"), Display(Name = "Anzahl", Prompt = "Anzahl Personen"), Range(1, 4, ErrorMessage = "Bitte Anzahl Personen zwischen {1} und {2}")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "remark"), Display(Name = "Bemerkung", Prompt = "Optionale Anmerkung"), MaxLength(250, ErrorMessage = "Die Bemerkung ist zu lang.")]
        public string Remark { get; set; }
    }
}
