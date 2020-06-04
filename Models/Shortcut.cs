using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace robert_brands_com.Models
{
    public class Shortcut : DocumentDBEntity
    {
        [JsonProperty(PropertyName = "category", NullValueHandling = NullValueHandling.Ignore)]
        [Required(ErrorMessage = "Bitte eine Link-Kategorie angeben")]
        [MaxLength(30, ErrorMessage = "Die Kategorie darf nicht länger als 30 Zeichen sein.")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "nickname", NullValueHandling = NullValueHandling.Ignore)]
        [Required(ErrorMessage = "Bitte einen Link-Kürzel angeben")]
        [MaxLength(40, ErrorMessage = "Das Kürzel darf nicht länger als 40 Zeichen sein.")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "url", NullValueHandling = NullValueHandling.Ignore)]
        [Required(ErrorMessage = "Bitte eine URL eingeben")]
        [MaxLength(255, ErrorMessage = "Der Shortcut darf nicht länger als 255 Zeichen sein.")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "remark", NullValueHandling = NullValueHandling.Ignore)]
        [MaxLength(512, ErrorMessage = "Die Bermerkung darf nicht länger als 512 Zeichen sein.")]
        public string Remark { get; set; }
    }
}
