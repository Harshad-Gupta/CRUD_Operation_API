using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRUD.Model
{
    public class ResultResponse
    {
        [JsonPropertyName("FLAG")]
        public bool FLAG { get; set; }

        [JsonPropertyName("MESSAGE")]
        public string? MESSAGE { get; set; }
    }
}
