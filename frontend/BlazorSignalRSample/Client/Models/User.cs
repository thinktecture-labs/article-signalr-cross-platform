namespace BlazorSignalRSample.Client.Models 
{
    public class User
    {
        // REVIEW: Das Casing ist sehr ungewohnt für .NET, da öffentliche Properties eigentlich in PascalCase geschrieben werden.
        public string connectionId { get; set; }
        public string name { get; set; }
    }
}
