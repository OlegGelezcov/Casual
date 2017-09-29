using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Casual.Ravenhill.Net {
    public abstract class BaseRequest {

        protected readonly INetService netService;
        protected readonly string url;
        protected INetErrorFactory errorFactory;

        

        public BaseRequest(INetService netService, string url, INetErrorFactory errorFactory) {
            this.netService = netService;
            this.url = url;
            this.errorFactory = errorFactory;
        }

        protected virtual void MakeRequest(WWWForm form,  System.Action<string> onResult, System.Action<INetError> onError) {
            netService.ExecuteCoroutine(CorMakeRequest(form, onResult, onError));
        }

        private System.Collections.IEnumerator CorMakeRequest(WWWForm form, System.Action<string> onResult, System.Action<INetError> onError) {
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.Send();
            yield return new WaitUntil(() => www.downloadHandler.isDone);

            if(www.isError) {
                onError?.Invoke(errorFactory.Create(NetErrorCode.unityrequest, www.error));
            } else if(errorFactory.IsErrorText(www.downloadHandler.text)) {
                onError?.Invoke(errorFactory.Create(www.downloadHandler.text, string.Empty));
            } else {
                onResult?.Invoke(www.downloadHandler.text);
            }
        }

    }
}
