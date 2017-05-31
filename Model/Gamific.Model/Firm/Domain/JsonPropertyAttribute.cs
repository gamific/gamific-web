using System;

namespace Vlast.Gamific.Model.Firm.Domain
{
    internal class JsonPropertyAttribute : Attribute
    {
        private string v;

        public JsonPropertyAttribute(string v)
        {
            this.v = v;
        }
    }
}