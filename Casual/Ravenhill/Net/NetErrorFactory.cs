﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casual.Ravenhill.Net {
    public class NetErrorFactory : INetErrorFactory {

        private readonly Dictionary<NetErrorCode, string> netErrorMap = new Dictionary<NetErrorCode, string> {
            [NetErrorCode.unknown] = "Unknown",
            [NetErrorCode.json] = "Parse json error",
            [NetErrorCode.sender_not_founded] = "Sender of gift not founded in users collection",
            [NetErrorCode.receiver_not_founded] = "Receiver of gift not founded in users collection",
            [NetErrorCode.gift_not_found] = "Gift not founded in gifts collection",
            [NetErrorCode.take_gift_fail] = "Take gift error"
        };

        public INetError Create(NetErrorCode code, string message) {
            return Create(code.ToString(), message);
        }

        public INetError Create(string text, string message) {
            NetErrorCode code;
            if(Enum.TryParse<NetErrorCode>(text, out code)) {
                if(netErrorMap.ContainsKey(code)) {
                    return new NetError(code, netErrorMap[code]);
                } else {
                    return new NetError(code, message);
                }
            }
            return new NetError(NetErrorCode.unknown, message);
        }

        public bool IsErrorText(string text) {
            NetErrorCode code;
            if(Enum.TryParse<NetErrorCode>(text, out code)) {
                return true;
            }
            if(text.Trim().ToLower() == "error") {
                return true;
            }
            return false;
        }
    }
}
