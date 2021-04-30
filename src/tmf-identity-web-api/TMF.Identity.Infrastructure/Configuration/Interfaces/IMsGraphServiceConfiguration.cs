namespace TMF.Identity.Infrastructure.Configuration.Interfaces
{
    public interface IMsGraphServiceConfiguration
    {
        string TenantId { get; set; }
        string AppId { get; set; }
        string AppSecret { get; set; }
        string TenantName { get; set; }
    }
}
