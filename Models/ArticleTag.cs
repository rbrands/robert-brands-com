using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace robert_brands_com.Models
{
    public class ArticleTag : DocumentDBEntity
    {
        /// <summary>
        /// Name of blog for which the tag is stored
        /// </summary>
        [JsonProperty(PropertyName = "listName"), Display(Name = "News-Liste", Prompt = "Name der News-Liste."), MaxLength(80)]
        public string ListName { get; set; }
        [JsonProperty(PropertyName = "tag"), Display(Name = "Bitte Tag-Namen eingeben."), MaxLength(40, ErrorMessage = "Tag zu lang.")]
        public string Tag { get; set; }
        [JsonProperty(PropertyName = "tagCount")]
        public int TagCount;
    }
}
