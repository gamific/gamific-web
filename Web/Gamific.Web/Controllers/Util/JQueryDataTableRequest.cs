using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Vlast.Gamific.Model.School.DTO
{
    /// <summary>
    /// Objeto de resposta para jquery database
    /// </summary>
    [DataContract]
    public class JQueryDataTableResponse
    {

        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>  
        [DataMember(Name = "draw")]
        public string Draw { get; set; }

        /// <summary>
        /// Total de linhas
        /// </summary>   
        [DataMember(Name = "recordsTotal")]
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Linhas encontradas
        /// </summary>     
        [DataMember(Name = "recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Dados em formato de Array
        /// 
        /// </summary>  
        [DataMember(Name = "data")]
        public string[][] Data { get; set; }


        /// <summary>
        /// Error on request
        /// </summary>     
        [DataMember(Name = "error")]
        public string Error { get; set; }
    }

    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// </summary>
    public class JQueryDataTableRequest
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>  
        [DataMember(Name = "draw")]
        public string Draw { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        [DataMember(Name = "start")]
        public int Start { get; set; }


        /// <summary>
        /// Número da página
        /// </summary>
        [DataMember(Name = "page")]
        public int Page
        {
            get
            {
                if(Length == 0)
                {
                    Length = 1;
                }
                return Start / Length;
            }
        }


        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        [DataMember(Name = "length")]
        public int Length { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        [DataMember(Name = "search")]
        public string Search
        {
            get
            {
                return HttpContext.Current.Request["search[value]"];
            }
        }

        /// <summary>
        /// Column used for order by
        /// </summary>
        [DataMember(Name = "order")]
        public string Order
        {
            get
            {
                return HttpContext.Current.Request["order[0][column]"];
            }
        }

        [DataMember(Name = "selected")]
        public string Selected { get; set; }

        /// <summary>
        /// Type order (asc or desc) used for order by
        /// </summary>
        [DataMember(Name = "type")]
        public string Type
        {
            get
            {
                return HttpContext.Current.Request["order[0][dir]"];
            }
        }

    }
}