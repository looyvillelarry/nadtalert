namespace NADT.Models
{
    public class StorageCreds
    {
        public string Account { get; set; }
        public string Key { get; set; }

        public string ConnectionString {get => $"DefaultEndpointsProtocol=https;AccountName={Account};AccountKey={Key};EndpointSuffix=core.windows.net";}
    
    }
}