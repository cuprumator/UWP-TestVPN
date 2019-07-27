using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TestVPN
{
    [DataContract]
    class AssociationParameters
    {
        [DataMember (Name = "EncryptionAlgorithm")]
        public string encryptionAlgorithm;
        [DataMember]
        public string integrityAlgorithm;
        [DataMember]
        public int diffieHellmanGroup;
        [DataMember]
        public int lifetimeMinutes;
    };

    [DataContract]
    class Server
    {
        [DataMember]
        public string country;
        [DataMember]
        public string serverAddress;
        [DataMember]
        public string remoteIdentifier;
        [DataMember (Name = "eap-name")]
        public string eap_name;
        [DataMember (Name = "eap-secret")]
        public string eap_secret;
    }

    [DataContract]
    class ConnectionConfiguration
    {
        [DataMember (Name = "ChildSecurityAssociationParameters")]
        public AssociationParameters childSecurityAssociationParameters;
        [DataMember (Name = "AuthenticationMethod")]
        public string authenticationMethod;
        [DataMember]
        public AssociationParameters ikeSecurityAssociationParameters;
        [DataMember]
        public string deadPeerDetectionRate;
        [DataMember]
        public bool disableMOBIKE;
        [DataMember]
        public bool disableRedirect;
        [DataMember]
        public bool enableRevocationCheck;
        [DataMember]
        public bool enablePFS;
        [DataMember]
        public List<Server> servers;
    }
}