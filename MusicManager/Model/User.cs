using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace MusicManager.Model
{
    public class Role
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")] // Specify the desired name in the serialized JSON
        public string? Name { get; set; }
    }

    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("roleDTOs")]
        public List<Role>? RoleDTOs { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

}
