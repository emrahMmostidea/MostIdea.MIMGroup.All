using System;
using System.Collections.Generic;
using System.Text;

namespace MostIdea.MIMGroup.Dto
{
    public class HierarchicalDto
    {
        public Guid Id { get; set; }
        public string ParentId { get; set; }
        public string Label { get; set; }
        public string EnumValue { get; set; }
        public bool IsAuthRestriction { get; set; }
        public string AuthorizedUsers { get; set; }
    }
}
