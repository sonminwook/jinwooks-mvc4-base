﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MVC4Base.Services
{
    public class MemberService
    {
        private Dao.MemberDao memberDao = null;

        public DataSet GetMemberList()
        {
            memberDao.GetMemberList();
            return new DataSet();
        }
    }
}