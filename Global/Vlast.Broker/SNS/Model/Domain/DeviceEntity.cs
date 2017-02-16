 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vlast.Util.Data;

namespace Vlast.Broker.SNS.Model
{
    /// <summary>
    /// Informações sobre o dispositivo.
    /// </summary>
    [Table("Push_Device")]
    [DataContract]
    public class DeviceEntity
    {
        [Key]
        [DataMember(Name = "id")]
        public long Id { get; set; }


        [DataMember(Name = "userId")]
        public long UserId { get; set; }

        /// <summary>
        /// Registro nos dispositivos para recebimento de push nas 
        /// plataformas especificas.
        /// </summary>
        [IgnoreDataMember]
        public string Endpoint { get; set; }

        [IgnoreDataMember]
        public string GlobalTopic { get; set; }

        /// <summary>
        /// Endpoint de notificação da Amazon.
        /// </summary>
        [IgnoreDataMember]
        public string BrokerEndpoint { get; set; }

        [DataMember(Name = "status")]
        public GenericStatus Status { get; set; }

        [DataMember(Name = "deviceType")]
        public DeviceType DeviceType { get; set; }

        [DataMember(Name = "lastUpdate")]
        public DateTime LastUpdate { get; set; }

    }

    /// <summary>
    /// Tipos de dispositivos conhecidos
    /// </summary>
    public enum DeviceType
    {
        ANDROID,
        IOS,
        WINDOWS_PHONE
    }

}
