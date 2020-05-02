using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

namespace HttpClientLibGenerator
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = true)]
    [CodeGenerationAttribute("HttpClientLibGenerator.Generators.HttpClientLibGenerator, " +
        "HttpClientLibGenerator.Generators")]
    [Conditional("CodeGeneration")]
    public class HttpClientLibAttribute : Attribute
    {
        public HttpClientLibAttribute(string serviceUrl)
        {
            ServiceUrl = serviceUrl;
        }

        public string ServiceUrl { get; }
    }

    public class RequestMappingAttribute: Attribute
    {
        public string EndpointUrl { get; }

        public RequestMappingAttribute(string endPointUrl)
        {
            EndpointUrl = EndpointUrl;
        }
    }
}
