using Newtonsoft.Json;

namespace bot.service.manager.Model.KubeService
{
    public class ServiceRootModel
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public Metadata metadata { get; set; }
        public Spec spec { get; set; }
        public Status status { get; set; }
    }

    public class Annotations
    {
        [JsonProperty("kubectl.kubernetes.io/last-applied-configuration")]
        public string kubectlkubernetesiolastappliedconfiguration { get; set; }
    }

    public class LoadBalancer
    {
    }

    public class Metadata
    {
        public Annotations annotations { get; set; }
        public DateTime creationTimestamp { get; set; }
        public string name { get; set; }
        public string @namespace { get; set; }
        public string resourceVersion { get; set; }
        public string uid { get; set; }
    }

    public class Port
    {
        public int nodePort { get; set; }
        public int port { get; set; }
        public string protocol { get; set; }
        public int targetPort { get; set; }
    }


    public class Selector
    {
        public string app { get; set; }
    }

    public class Spec
    {
        public bool allocateLoadBalancerNodePorts { get; set; }
        public string clusterIP { get; set; }
        public List<string> clusterIPs { get; set; }
        public string externalTrafficPolicy { get; set; }
        public string internalTrafficPolicy { get; set; }
        public List<string> ipFamilies { get; set; }
        public string ipFamilyPolicy { get; set; }
        public List<Port> ports { get; set; }
        public Selector selector { get; set; }
        public string sessionAffinity { get; set; }
        public string type { get; set; }
    }

    public class Status
    {
        public LoadBalancer loadBalancer { get; set; }
    }
}
