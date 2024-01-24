using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRUD.Model
{
    public class LoginData
    {
        [JsonPropertyName("USER_NAME")]
        public string? USER_NAME { get; set; }

        [JsonPropertyName("PASSWORD")]
        public string? PASSWORD { get; set; }
    }

    public class User
    {
        [JsonPropertyName("USER_ID")]
        public int USER_ID { get; set; }

        [JsonPropertyName("FULL_NAME")]
        public string? FULL_NAME { get; set; }

        [JsonPropertyName("MOBILE_NO")]
        public string? MOBILE_NO { get; set; }

        [JsonPropertyName("EMAIL")]
        public string? EMAIL { get; set; }

        [JsonPropertyName("USER_NAME")]
        public string? USER_NAME { get; set; }

        [JsonPropertyName("PASSWORD")]
        public string? PASSWORD { get; set; }
    }

    public class LoggedInUser
    {
        [JsonPropertyName("USER_ID")]
        public int USER_ID { get; set; }

        [JsonPropertyName("USER_NAME")]
        public string? USER_NAME { get; set; }

        [JsonPropertyName("FULL_NAME")]
        public string? FULL_NAME { get; set; }

        [JsonPropertyName("MOBILE_NO")]
        public string? MOBILE_NO { get; set; }

        [JsonPropertyName("EMAIL")]
        public string? EMAIL { get; set; }

        [JsonPropertyName("RESULT")]
        public ResultResponse RESULT { get; set; }

        [JsonPropertyName("TOKEN")]
        public string? TOKEN { get; set; }
    }

    public class UserList
    {
        [JsonPropertyName("USER_LIST")]
        public List<User> USER_LIST { get; set; }

        [JsonPropertyName("RESULT")]
        public ResultResponse RESULT { get; set; }
    }
}
