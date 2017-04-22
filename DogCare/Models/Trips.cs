using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCare.Models
{
    public class Trips
    {
        string id;
        string dogName;
        string owner;
        int distance;
        bool poop;
        bool pee;
        string date;
        string time;

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

        [JsonProperty(PropertyName = "owner")]
        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        [JsonProperty(PropertyName = "distance")]
        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        [JsonProperty(PropertyName = "poop")]
        public bool Poop
        {
            get { return poop; }
            set { poop = value; }
        }

        [JsonProperty(PropertyName = "pee")]
        public bool Pee
        {
            get { return pee; }
            set { pee = value; }
        }

        [JsonProperty(PropertyName = "date")]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }


        [JsonProperty(PropertyName = "time")]
        public string Time
        {
            get { return time; }
            set { time = value; }
        }

    }
}
