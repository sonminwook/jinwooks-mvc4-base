using System;
namespace MVC4Base.Services
{
    public interface IDebugWindowService
    {
        string Print();
        void Write(System.Text.StringBuilder writer);
    }
}
