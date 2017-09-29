using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public interface INetError {
        NetErrorCode ErrorCode { get; }
        string ErrorMessage { get; }
    }
}
