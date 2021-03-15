using Newtonsoft.Json;
using System;

namespace PhamacyViewer.Model
{
    public class Card
    {
        [JsonProperty("doctorId")]
        public int DoctorId { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("recomendation")]
        public string Recomendation { get; set; }
        public string DoctorFIO { get; set; }

        public override string ToString()
        {
            return $"Время визита: {UnixTimeStampToDateTime(double.Parse(Date))}\n" +
                $"Информация: {Info}\n" +
                $"Реакомендация: {Recomendation}\n" +
                $"Врач: {DoctorFIO}";
        }
        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
