using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhamacyViewer.Model
{
    public class Doctor
    {
        [JsonProperty("doctorId")]
        public int DoctorId { get; set; }

        [JsonProperty("fio")]
        public string Fio { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }
    }
}
