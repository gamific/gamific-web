using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Util.Parameter
{
    public class ParameterEntity
    {
        public String Name{get;set;}

        public String Description { get; set; }

        public String Value { get; set; }

        public String Scope { get; set; }

        public ParameterEntity()
        {

        }

        public ParameterEntity(String parameterName, String scope, String value)
        {
            this.Name = parameterName;
            this.Scope = scope;
            this.Value = value;
        }

        public ParameterEntity(String parameterName, String scope, String value, String description)
        {
            this.Name = parameterName;
            this.Scope = scope;
            this.Value = value;
            this.Description = description;
        }

        public ParameterEntity(String parameterName, String value)
        {
            this.Name = parameterName;
            this.Scope = "";
            this.Value = value;
        }
    }
}
