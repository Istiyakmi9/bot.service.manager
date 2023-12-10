using YamlDotNet.Serialization;

namespace bot.service.manager.Model
{
    public class YamlModel
    {
        [YamlMember(Alias = "apiVersion")]
        public string ApiVersion { get; set; }

        [YamlMember(Alias = "kind")]
        public string Kind { get; set; }

        [YamlMember(Alias = "metadata")]
        public YmlMetadata Metadata { get; set; }

        [YamlMember(Alias = "spec")]
        public YmlSpec Spec { get; set; }
    }

    public class YmlSpec { }

    public class YmlMetadata
    {
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        [YamlMember(Alias = "labels")]
        public YmlLabels Labels { get; set; }
    }

    public class YmlLabels
    {
        [YamlMember(Alias = "app")]
        public string App { get; set; }
    }
}
