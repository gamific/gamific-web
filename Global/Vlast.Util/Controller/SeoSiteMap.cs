using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Vlast.Util.Controller
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SEOSitemap
    {
        private ArrayList map;

        public SEOSitemap()
        {
            map = new ArrayList();
        }

        [XmlElement("url")]
        public Location[] Locations
        {
            get
            {
                Location[] items = new Location[map.Count];
                map.CopyTo(items);
                return items;
            }
            set
            {
                if (value == null)
                    return;
                Location[] items = (Location[])value;
                map.Clear();
                foreach (Location item in items)
                    map.Add(item);
            }
        }

        public int Add(Location item)
        {
            return map.Add(item);
        }
    }

    // Items in the shopping list
    public class Location
    {
        public enum eChangeFrequency
        {
            always,
            hourly,
            daily,
            weekly,
            monthly,
            yearly,
            never
        }

        [XmlElement("loc")]
        public string Url { get; set; }

        [XmlElement("changefreq")]
        public eChangeFrequency? ChangeFrequency { get; set; }
        public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

        [XmlElement("lastmod")]
        public DateTime? LastModified { get; set; }
        public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

        [XmlElement("priority")]
        public double? Priority { get; set; }
        public bool ShouldSerializePriority() { return Priority.HasValue; }

        [XmlElement("news", Namespace = "http://www.google.com/schemas/sitemap-news/0.9")]
        public SEONews News { get; set; }
    }
   
    [XmlType(TypeName = "news",Namespace = "http://www.google.com/schemas/sitemap-news/0.9" )]
    public class SEONews
    {
        [XmlElement("publication")]
        public Publication Publication { get; set; }

        [XmlElement("publication_date")]
        public string PublicationDate { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("keywords")]
        public string Keywords { get; set; }
    }

    [XmlType(TypeName = "publication", Namespace = "http://www.google.com/schemas/sitemap-news/0.9")]
    public class Publication
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("lang")]
        public string Lang = "pt";
    }
}
