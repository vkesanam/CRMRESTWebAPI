using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CRMRESTWebAPI.Models
{
    [DataContract]
    public class BaseInputType
    {
        [DataMember]
        public String CreatedDate { get; set; }
        [DataMember]
        public String CreatedBy { get; set; }
        [DataMember]
        public String UpdatedDate { get; set; }
        [DataMember]
        public String UpdatedBy { get; set; }
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public String Status { get; set; }

    }
}