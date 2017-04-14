using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DogCare
{
    public class Owner
    {

            string id;
            string ownerName;
            string userName;
            string password;
            string imageO;

            [JsonProperty(PropertyName = "id")]
            public string Id
            {
                get { return id; }
                set { id = value; }
            }

            [JsonProperty(PropertyName = "ownerName")]
            public string OwnerName
        {
                get { return ownerName; }
                set { ownerName = value; }
            }

            [JsonProperty(PropertyName = "userName")]
            public string UserName
        {
                get { return userName; }
                set { userName = value; }
            }

            [JsonProperty(PropertyName = "password")]
            public string Password
        {
                get { return password; }
                set { password = value; }
            }

            [JsonProperty(PropertyName = "image")]
            public string Image
            {
                get { return imageO; }
                set { imageO = value; }
            }

            [Version]
            public string Version { get; set; }
        }
}
