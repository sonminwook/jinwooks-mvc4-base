using System;
using MVC4Base.Models;
namespace MVC4Base.Services
{
    public interface ICodeService
    {
        System.Data.DataSet GetCodeList(PagingModel model, string titleYN, string mainCode, string subCode, string codeName);
    }
}
