using System;
using MVC4Base.Models;
namespace MVC4Base.Services
{
    public interface ICodeService
    {
        System.Data.DataSet GetCodeList(CodeSearchModels model);
    }
}
