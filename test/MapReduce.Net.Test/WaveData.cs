using System;

namespace MapReduce.Net.Test
{
    public class WaveData
    {
        public string Site { get; set; }
        public string SiteNumber { get; set; }
        public int Seconds { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Hsig { get; set; }
        public decimal Hmax { get; set; }
        public decimal Tp { get; set; }
        public decimal Tz { get; set; }
        public decimal Sst { get; set; }
        public decimal Direction { get; set; }
    }
}
