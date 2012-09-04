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
        
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
        [Display(Name = "메인코드")]
        [StringLength(20, ErrorMessage = "{0}는 {1}자리까지만 입력이 가능합니다.", MinimumLength = 0)]
        public string MainCode { get; set; }
        public char TitleYN { get; set; }
        [Display(Name = "서브코드")]
        [StringLength(20, ErrorMessage = "{0}는 {1}자리까지만 입력이 가능합니다.", MinimumLength = 0)]
        public string SubCode { get; set; }
        public string CodeName { get; set; }
    }
}