using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare
{
    public class Dog
    {
        string id;
        string dogName;
        string gender;
        string race;
        string owner;
        string imageD;
        int walk;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "dogName")]
        public string DogName
        {
            get { return dogName; }
            set { dogName = value; }
        }

        [JsonProperty(PropertyName = "gender")]
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [JsonProperty(PropertyName = "race")]
        public string Race
        {
            get { return race; }
            set { race = value; }
        }

        [JsonProperty(PropertyName = "image")]
        public string ImageD
        {
            get { return imageD; }
            set { imageD = value; }
        }
        
        [JsonProperty(PropertyName = "owner")]
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        [JsonProperty(PropertyName = "walk")]
        public int Walk
        {
            get { return walk; }
            set { walk = value; }
        }
        [Version]
        public string Version { get; set; }
    }
}

