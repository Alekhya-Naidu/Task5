using System;

namespace EMS.Common.Helpers;

public interface IWriter
{
    void PrintMsg(string msg);
    void PrintError(string message);
    void PrintSuccess(string message);
}