﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;


namespace robert_brands_com.Models
{
    public class DocumentDBEntity     
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type
        {
            get
            {
                return this.GetType().Name;
            }
            set
            {

            }
        }
        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey
        {
            get
            {
                return this.Type;
            }
            set
            {

            }
        }
        /// <summary>
        /// Logical key to be used for this entity. If this member is set the id of the document
        /// is created as according to the pattern type-key and should be unique.
        /// </summary>
        [JsonProperty(PropertyName = "key", NullValueHandling = NullValueHandling.Ignore)]
        public string LogicalKey { get; set; }
        // used to set expiration policy
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TimeToLive { get; set; }
        public void SetUniqueKey()
        {
            if (null != LogicalKey)
            {
                Id = Type + "-" + LogicalKey;
            }
        }
    }
}
