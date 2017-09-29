using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface INetErrorFactory {
        INetError Create(NetErrorCode code, string message);
        INetError Create(string text, string message);
        bool IsErrorText(string text);
    }
}
