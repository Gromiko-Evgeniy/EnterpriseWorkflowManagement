namespace ProjectManagementService.Application.Configuration;

public class MongoDBConfiguration
{
    public string ConnetionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public Dictionary<string, string> Collections { get; set; }
}
