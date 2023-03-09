using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace API.REST
{
    public class RestSharpClient
    {
        private readonly RestClient client;
        private readonly string domain;

        public RestSharpClient(string domain)
        {
            this.domain = domain;
            this.client = new RestClient(this.domain);
        }

        private T? DeserializeResponse<T>(RestResponse response, string? rootElement = null)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(response.Content);

            if (!string.IsNullOrEmpty(rootElement))
            {
                return JsonSerializer.Deserialize<T>(json.GetProperty(rootElement));
            }

            return JsonSerializer.Deserialize<T>(json);
        }

        public RestResponse Execute(RestRequest request, Method method, Dictionary<string, string> headers = default!)
        {
            if (headers is not null)
            {
                headers.ToList()
                    .ForEach(x =>
                        this.client.DefaultParameters.RemoveParameter(x.Key)
                    );

                this.client.AddDefaultHeaders(headers);
            }

            return this.client.ExecuteAsync(request, method).Result;
        }

        public RestResponse Get(RestRequest request, Dictionary<string, string> headers = default!)
        {
            return this.Execute(request, Method.Get, headers);
        }

        public RestResponse Get(string resource, Dictionary<string, string> headers = default!)
        {
            return this.Get(new RestRequest(resource), headers);
        }

        public T? Get<T>(RestRequest request, Dictionary<string, string> headers = default!, string? rootElement = null)
        {
            var response = this.Get(request, headers);
            return this.DeserializeResponse<T>(response, rootElement);
        }

        public T? Get<T>(string resource, Dictionary<string, string> headers = default!, string? rootElement = null)
        {
            return this.Get<T>(new RestRequest(resource), headers, rootElement);
        }

        public RestResponse Post(RestRequest request, Dictionary<string, string> headers = default!)
        {
            return this.Execute(request, Method.Post, headers);
        }

        public RestResponse Post(string resource, object jsonPayload, Dictionary<string, string> headers = default!)
        {
            var request = new RestRequest(resource);
            request.AddJsonBody(jsonPayload);

            return this.Post(request, headers);
        }

        public T? Post<T>(RestRequest request, Dictionary<string, string> headers = default!, string? rootElement = null)
        {
            var response = this.Post(request, headers);
            return this.DeserializeResponse<T>(response, rootElement);
        }

        public T? Post<T>(string resource, object payload, Dictionary<string, string> headers = default!, string? rootElement = null)
        {
            var request = new RestRequest(resource);
            request.AddJsonBody(payload);

            var response = this.Post(request, headers);
            return this.DeserializeResponse<T>(response, rootElement);
        }

        public RestResponse Put(RestRequest request, Dictionary<string, string> headers = default!)
        {
            return this.Execute(request, Method.Put, headers);
        }

        public RestResponse Put(string resource, object jsonPayload, Dictionary<string, string> headers = default!)
        {
            var request = new RestRequest(resource);
            request.AddJsonBody(jsonPayload);

            return this.Put(request, headers);
        }

        public T? Put<T>(RestRequest request, Dictionary<string, string> headers = default!, string? rootElement = null)
        {
            var response = this.Put(request, headers);
            return this.DeserializeResponse<T>(response, rootElement);
        }

        public T? Put<T>(string resource, object payload, Dictionary<string, string> headers = default!, string? rootElement = null)
        {
            var request = new RestRequest(resource);
            request.AddJsonBody(payload);

            var response = this.Put(request, headers);
            return this.DeserializeResponse<T>(response, rootElement);
        }

        public RestResponse Delete(RestRequest request, Dictionary<string, string> headers = default!)
        {
            return this.Execute(request, Method.Delete, headers);
        }

        public RestResponse Delete(string resource, Dictionary<string, string> headers = default!)
        {
            return this.Delete(new RestRequest(resource, Method.Delete), headers);
        }
    }
}