using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MVC4Base.Services
{
    public class MemberService : MVC4Base.Services.IMemberService
    {
        public DataSet GetMemberList()
        {
            return new DataSet();
        }
    }
}