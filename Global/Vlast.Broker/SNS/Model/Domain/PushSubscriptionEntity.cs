using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Vlast.Util.Data;

namespace Vlast.Broker.SNS.Model
{
    /// <summary>
    /// Informações sobre a assinaturas de email e push.
    /// </summary>
    [Table("Push_Subscription")]
    public class PushSubscriptionEntity
    {
        /// <summary>
        /// Id único  
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "userId")]
        public long UserId { get; set; }

        [DataMember(Name = "deviceId")]
        public long DeviceId { get; set; }

        /// <summary>
        /// 0 -> Inativo
        /// 1 -> Ativo
        /// </summary>
        [DataMember(Name = "status")]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "lastUpdate")]
        public DateTime LastUpdate { get; set; }
 
        /// <summary>
        /// Grupo da notificação
        /// Ex; GLOBAL
        /// </summary>
        [DataMember(Name = "topic")]
        public string Topic { get; set; }

        [DataMember(Name = "topicArn")]
        public string SubscriptionArn { get; set;}
    }

}
