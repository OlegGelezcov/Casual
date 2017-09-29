namespace Casual.Ravenhill.Net {
    public class NetError : INetError {

        private NetErrorCode errorCode;
        private string errorMessage;

        public NetError(NetErrorCode errorCode, string errorMessage ) {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }

        public NetErrorCode ErrorCode => errorCode;

        public string ErrorMessage => errorMessage;

        public override string ToString() {
            return $"Code: {ErrorCode} => {ErrorMessage}";
        }
    }
}
