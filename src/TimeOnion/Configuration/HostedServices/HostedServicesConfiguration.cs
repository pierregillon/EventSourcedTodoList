using System.ComponentModel.DataAnnotations;

namespace TimeOnion.Configuration.HostedServices;

public class HostedServicesConfiguration
{
    public const string SectionName = "HostedServices";

    [Required] public CommitterHostedServiceConfiguration Committer { get; set; } = default!;
}

public class CommitterHostedServiceConfiguration
{
    [Required] public double? IntervalInSeconds { get; set; }
}