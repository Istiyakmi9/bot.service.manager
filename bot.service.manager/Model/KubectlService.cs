using Newtonsoft.Json;

namespace bot.service.manager.Model
{

    public class PodRootModel
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public Metadata metadata { get; set; }
        public List<Items> items { get; set; }
    }


    public class Items
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public Metadata metadata { get; set; }
        public Spec spec { get; set; }
        public Status status { get; set; }
    }


    #region STATUS inside items

    public class Status
    {
        public List<Condition> conditions { get; set; }
        public List<ContainerStatus> containerStatuses { get; set; }
        public string hostIP { get; set; }
        public string phase { get; set; }
        public string podIP { get; set; }
        public List<PodIP> podIPs { get; set; }
        public string qosClass { get; set; }
        public DateTime startTime { get; set; }
    }

    public class PodIP
    {
        public string ip { get; set; }
    }

    public class ContainerStatus
    {
        public string containerID { get; set; }
        public string image { get; set; }
        public string imageID { get; set; }
        public LastState lastState { get; set; }
        public string name { get; set; }
        public bool ready { get; set; }
        public int restartCount { get; set; }
        public bool started { get; set; }
        public State state { get; set; }
    }

    public class LastState
    {
        public Terminated terminated { get; set; }
    }

    public class Terminated
    {
        public string containerID { set; get; }
        public int exitCode { set; get; }
        public DateTime finishedAt { set; get; }
        public string reason { set; get; }
        public DateTime startedAt { set; get; }
    }

    public class State
    {
        public Running running { get; set; }
    }

    public class Running
    {
        public DateTime startedAt { get; set; }
    }

    public class Condition
    {
        public object lastProbeTime { get; set; }
        public DateTime lastTransitionTime { get; set; }
        public string status { get; set; }
        public string type { get; set; }
    }

    #endregion 


    #region METADATA inside items

    public class Metadata
    {
        public Annotations annotations { get; set; }
        public DateTime creationTimestamp { get; set; }
        public string generateName { get; set; }
        public Labels labels { get; set; }
        public string name { get; set; }
        public string @namespace { get; set; }
        public List<OwnerReference> ownerReferences { get; set; }
        public string resourceVersion { get; set; }
        public string uid { get; set; }
    }

    // Define classes to represent the JSON structure
    public class Annotations
    {
        [JsonProperty("cni.projectcalico.org/containerID")]
        public string cniprojectcalicoorgcontainerID { get; set; }

        [JsonProperty("cni.projectcalico.org/podIP")]
        public string cniprojectcalicoorgpodIP { get; set; }

        [JsonProperty("cni.projectcalico.org/podIPs")]
        public string cniprojectcalicoorgpodIPs { get; set; }
    }

    public class Labels
    {
        public string app { get; set; }

        [JsonProperty("pod-template-hash")]
        public string podtemplatehash { get; set; }
    }

    public class OwnerReference
    {
        public string apiVersion { get; set; }
        public bool blockOwnerDeletion { get; set; }
        public bool controller { get; set; }
        public string kind { get; set; }
        public string name { get; set; }
        public string uid { get; set; }
    }

    #endregion


    #region SPEC inside items

    public class Spec
    {
        public List<Container> containers { get; set; }
        public string dnsPolicy { get; set; }
        public bool enableServiceLinks { get; set; }
        public string nodeName { get; set; }
        public string preemptionPolicy { get; set; }
        public int priority { get; set; }
        public string restartPolicy { get; set; }
        public string schedulerName { get; set; }
        public SecurityContext securityContext { get; set; }
        public string serviceAccount { get; set; }
        public string serviceAccountName { get; set; }
        public int terminationGracePeriodSeconds { get; set; }
        public List<Toleration> tolerations { get; set; }
        public List<Volume> volumes { get; set; }
    }

    public class SecurityContext
    {
    }

    public class Volume
    {
        public string name { get; set; }
        public Projected projected { get; set; }
    }

    public class Projected
    {
        public int defaultMode { get; set; }
        public List<Source> sources { get; set; }
    }

    public class Source
    {
        public ServiceAccountToken serviceAccountToken { get; set; }
        public ConfigMap configMap { get; set; }
        public DownwardAPI downwardAPI { get; set; }
    }

    public class DownwardAPI
    {
        public List<Item> items { get; set; }
    }

    public class ConfigMap
    {
        public List<Item> items { get; set; }
        public string name { get; set; }
    }

    public class Item
    {
        public string key { get; set; }
        public string path { get; set; }
        public FieldRef fieldRef { get; set; }
    }

    public class FieldRef
    {
        public string apiVersion { get; set; }
        public string fieldPath { get; set; }
    }

    public class ServiceAccountToken
    {
        public int expirationSeconds { get; set; }
        public string path { get; set; }
    }

    public class Container
    {
        public string image { get; set; }
        public string imagePullPolicy { get; set; }
        public string name { get; set; }
        public List<Port> ports { get; set; }
        public Resources resources { get; set; }
        public string terminationMessagePath { get; set; }
        public string terminationMessagePolicy { get; set; }
        public List<VolumeMount> volumeMounts { get; set; }
    }

    public class VolumeMount
    {
        public string mountPath { get; set; }
        public string name { get; set; }
        public bool readOnly { get; set; }
    }

    public class Port
    {
        public int containerPort { get; set; }
        public string protocol { get; set; }
        public string name { get; set; }
    }

    public class Resources
    {
    }

    public class Toleration
    {
        public string effect { get; set; }
        public string key { get; set; }
        public string @operator { get; set; }
        public int tolerationSeconds { get; set; }
    }

    #endregion
}