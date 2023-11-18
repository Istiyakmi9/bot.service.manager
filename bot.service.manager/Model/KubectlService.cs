using Newtonsoft.Json;

namespace bot.service.manager.Model
{
    // Define classes to represent the JSON structure
    public class PodsDetail
    {
        public Annotations Annotations { get; set; }
        public string CreationTimestamp { get; set; }
        public string GenerateName { get; set; }
        public Labels Labels { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public OwnerReference[] OwnerReferences { get; set; }
        public string ResourceVersion { get; set; }
        public string Uid { get; set; }
    }

    public class Annotations
    {
        [JsonProperty("cni.projectcalico.org/containerID")]
        public string ContainerId { get; set; }

        [JsonProperty("cni.projectcalico.org/podIP")]
        public string PodIP { get; set; }

        [JsonProperty("cni.projectcalico.org/podIPs")]
        public string PodIPs { get; set; }
    }

    public class Labels
    {
        public string App { get; set; }
        [JsonProperty("pod-template-hash")]
        public string PodTemplateHash { get; set; }
    }

    public class OwnerReference
    {
        public string ApiVersion { get; set; }
        public bool BlockOwnerDeletion { get; set; }
        public bool Controller { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
    }

    public class Spec
    {
        public Container[] Containers { get; set; }
        public string DnsPolicy { get; set; }
        public bool EnableServiceLinks { get; set; }
        public string NodeName { get; set; }
        public string PreemptionPolicy { get; set; }
        public int Priority { get; set; }
        public string RestartPolicy { get; set; }
        public string SchedulerName { get; set; }
        public string ServiceAccount { get; set; }
        public string ServiceAccountName { get; set; }
        public int TerminationGracePeriodSeconds { get; set; }
        public Tolerance[] Tolerations { get; set; }
        public Volume[] Volumes { get; set; }
    }

    public class Container
    {
        public string Image { get; set; }
        public string ImagePullPolicy { get; set; }
        public string Name { get; set; }
        public Port[] Ports { get; set; }
        public string TerminationMessagePath { get; set; }
        public string TerminationMessagePolicy { get; set; }
        public VolumeMount[] VolumeMounts { get; set; }
    }

    public class Port
    {
        public int ContainerPort { get; set; }
        public string Protocol { get; set; }
    }

    public class VolumeMount
    {
        public string MountPath { get; set; }
        public string Name { get; set; }
        public bool ReadOnly { get; set; }
    }

    public class Tolerance
    {
        public string Effect { get; set; }
        public string Key { get; set; }
        public string Operator { get; set; }
        public int TolerationSeconds { get; set; }
    }

    public class Volume
    {
        public string Name { get; set; }
        public Projected Projected { get; set; }
    }

    public class Projected
    {
        public int DefaultMode { get; set; }
        public Source[] Sources { get; set; }
    }

    public class Source
    {
        public ServiceAccountToken ServiceAccountToken { get; set; }
        public ConfigMap ConfigMap { get; set; }
        public DownwardAPI DownwardAPI { get; set; }
    }

    public class ServiceAccountToken
    {
        public int ExpirationSeconds { get; set; }
        public string Path { get; set; }
    }

    public class ConfigMap
    {
        public Item[] Items { get; set; }
        public string Name { get; set; }
    }

    public class Item
    {
        public string Key { get; set; }
        public string Path { get; set; }
    }

    public class DownwardAPI
    {
        public Item1[] Items { get; set; }
    }

    public class Item1
    {
        public FieldRef FieldRef { get; set; }
        public string Path { get; set; }
    }

    public class FieldRef
    {
        public string ApiVersion { get; set; }
        public string FieldPath { get; set; }
    }

    public class Status
    {
        public Condition[] Conditions { get; set; }
        public ContainerStatus[] ContainerStatuses { get; set; }
        public string HostIP { get; set; }
        public string Phase { get; set; }
        public string PodIP { get; set; }
        public PodIP[] PodIPs { get; set; }
        public string QosClass { get; set; }
        public string StartTime { get; set; }
    }

    public class Condition
    {
        public DateTime? LastProbeTime { get; set; }
        public DateTime LastTransitionTime { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }

    public class ContainerStatus
    {
        public string ContainerID { get; set; }
        public string Image { get; set; }
        public string ImageID { get; set; }
        public string Name { get; set; }
        public bool Ready { get; set; }
        public int RestartCount { get; set; }
        public bool Started { get; set; }
        public State State { get; set; }
    }

    public class State
    {
        public Running Running { get; set; }
    }

    public class Running
    {
        public DateTime StartedAt { get; set; }
    }

    public class PodIP
    {
        public string Ip { get; set; }
    }
}