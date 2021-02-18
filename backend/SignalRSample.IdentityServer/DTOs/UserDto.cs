namespace SignalRSample.IdentityServer.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public string AvatarUrl { get; set; }
    }
}