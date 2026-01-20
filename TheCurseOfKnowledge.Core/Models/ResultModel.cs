using System;
using System.Linq;
using System.Net;

namespace TheCurseOfKnowledge.Core.Models
{
    public class ResultModel<TModel>
    {
        readonly HttpStatusCode[] statuscodes = new HttpStatusCode[] {
            HttpStatusCode.OK,
            HttpStatusCode.Created,
            HttpStatusCode.Accepted,
            HttpStatusCode.NonAuthoritativeInformation,
            HttpStatusCode.NoContent,
            HttpStatusCode.ResetContent,
            HttpStatusCode.PartialContent,
            HttpStatusCode.MultiStatus,
            HttpStatusCode.AlreadyReported,
        };
        public ResultModel()
        {
            Id = default(string);
            IsSuccess = default(bool);
            StatusCode = default(HttpStatusCode);
            Model = default(TModel);
            Exception = default(Exception);
        }

        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public TModel Model { get; set; }
        public Exception Exception { get; set; }
        public bool IsSuccessStatusCode
            => statuscodes.Any(any => any == StatusCode);
    }
}
