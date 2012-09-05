using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MVC4Base.Models;

namespace MVC4Base.Services
{
    public class CodeService : MVC4Base.Services.ICodeService
    {
        private Dao.CodeDao codeDao = null;

        public DataSet GetCodeList(PagingModel model, string titleYN, string mainCode, string subCode, string codeName)
        {
            return codeDao.GetSYSCode(model, titleYN, mainCode, subCode, codeName);
        }
    }
}