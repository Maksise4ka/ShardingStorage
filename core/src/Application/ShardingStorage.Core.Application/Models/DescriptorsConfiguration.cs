using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ShardingStorage.Core.Application.Models;

public class DescriptorsConfiguration
{
    public DescriptorsConfiguration(IList<DescriptorItem> descriptors)
    {
        Descriptors = descriptors;
    }

    public IList<DescriptorItem> Descriptors { get; }

    public static DescriptorsConfiguration FromYamlConfiguration(string configurationPath)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        using var sr = File.OpenText(configurationPath);
        var descriptors = deserializer.Deserialize<List<DescriptorItem>>(sr);

        return new DescriptorsConfiguration(descriptors);
    }
    
    public class DescriptorItem
    {
        public string Key { get; set; } = null!;
    
        public string? Value { get; set; }

        [YamlMember(Alias = "rate_limit", ApplyNamingConventions = false)]
        public RateLimit RateLimit { get; set; } = null!;
    }

    public class RateLimit
    {
        public Unit Unit {get; set;}

        [YamlMember(Alias = "requests_per_unit", ApplyNamingConventions = false)]
        public int RequestsPerUnit { get; set; }

        public TimeSpan ToTimeSpan()
        {
            TimeSpan span = Unit switch
            {
                Unit.Second => TimeSpan.FromSeconds(1),
                Unit.Minute => TimeSpan.FromMinutes(1),
                Unit.Hour => TimeSpan.FromHours(1),
                _ => throw new ArgumentOutOfRangeException()
            };

            return span;
        }
    }

    public enum Unit
    {
        Second,
        Minute,
        Hour,
    }
}