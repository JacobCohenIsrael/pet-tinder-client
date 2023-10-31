using Proyecto26;
using UnityEngine;
using RSG;

namespace Gamefather.Services
{
    [CreateAssetMenu(menuName = "Services/Http", fileName = "HttpService")]
    public class HttpService: ScriptableObject
    {
        [SerializeField] private string baseUrl;
        
        public IPromise<TResponse> Get<TResponse>(string queryString)
        {
            var promise = new Promise<TResponse>();
            RestClient.Get<TResponse>($"{baseUrl}{queryString}").Then(
                response =>
                {
                    promise.Resolve(response);
                }
            ).Catch(promise.Reject);
            return promise;
        }

        public IPromise<TResponse> Post<TRequest, TResponse>(TRequest request, string path)
        {
            var url = $"{baseUrl}{path}";
            var promise = new Promise<TResponse>();
            RestClient.Post<TResponse>(url, request).Then(response =>
            {
                promise.Resolve(response);
            });
            return promise;
        }
    }
}
