namespace Tesis.Models
{
    public class CloudConnectionConfig
    {
        public string CloudUrl { get; set; }
        public string CloudUsername { get; set; }
        public string CloudPassword { get; set; }
        public bool NetProxy { get; set; }
        public string ProxyIP { get; set; }
        public string ProxyPort { get; set; }
        public string UserProxy { get; set; }
        public string ProxyPassword { get; set; }
    }
}
