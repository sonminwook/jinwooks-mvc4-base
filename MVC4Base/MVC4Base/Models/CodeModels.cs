using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC4Base.Models
{
    public class PagingModel
    {
        public PagingModel()
        {
            PageIndex = 1;
            PageSize = 10;
            Order = string.Empty;
            TotlaCount = 0;
        }
        
        public Decimal PageIndex { get; set; }
        public Decimal PageSize { get; set; }
        public string Order { get; set; }
        public Decimal TotlaCount { get; set; }

        /// <summary>
        /// 페이징 갯수
        /// </summary>
        public Decimal PageCount {
            get {
                Decimal v = Math.Ceiling((Decimal)TotlaCount / PageSize);
                return v == 0 ? 1 : v;
            }
        }

        /// <summary>
        /// 페이징 나열 시작 번호
        /// </summary>
        public Decimal PageStart {
            get
            {
                return PageIndex - (PageIndex % 10) + 1;
            }
        }
        /// <summary>
        /// 페이징 나열 끝 번호
        /// </summary>
        public Decimal PageEnd {
            get
            {
                return PageStart + 9 < PageCount ? PageStart : PageCount;
            }
        }
        /// <summary>
        /// 이전버튼 페이징 번호
        /// </summary>
        public Decimal PrevStart {
            get
            {
                return PageStart - 10 > 0 ? PageStart - 10 : 1;
            }
        }
        /// <summary>
        /// 다음버튼 페이징 번호
        /// </summary>
        public Decimal NextStart {
            get
            {
                return PageStart + 10 < PageCount ? PageStart + 10 : PageCount;
            }
        }
        
    }
}