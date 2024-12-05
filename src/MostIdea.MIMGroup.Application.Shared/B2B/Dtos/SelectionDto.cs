using System;
using System.Collections.Generic;
using System.Text;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class SelectionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
    }

    public class SelectionInput
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
    }
}
