using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC4Base.Models
{
    public class CodeSearchModels
    {
        public CodeSearchModels()
        {
            PageIndex = 1;
            PageSize = 10;
            Order = string.Empty;
            MainCode = string.Empty;
            TitleYN = 'Y';
            SubCode = string.Empty;
            CodeName = string.Empty;
        }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public string Order { get; private set; }
        public string MainCode { get; private set; }
        public char TitleYN { get; private set; }
        public string SubCode { get; private set; }
        public string CodeName { get; private set; }
    }
}