using MessagePack;
using Mgr.Core.Interfaces;
using System.Net;

namespace Mgr.Core.Entities
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MethodResult<T> : IMethodResult<T>
    {
        public bool Success { get; set; } = true;

        public T Data { get; set; } 

        public string Message { get; set; }

        public int? Status { get; set; }

        public int TotalRecord { get; set; }

        public static MethodResult<T> ResultWithData(
          T data,
          string message = "",
          int totalRecord = 0)
        {
            return new MethodResult<T>()
            {
                Data = data,
                Message = message,
                TotalRecord = totalRecord,
                Status = (int)HttpStatusCode.OK
            };
        }

        public static MethodResult<T> ResultWithError(
          int? status = null,
          string message = "")
        {
            return new MethodResult<T>()
            {
                Success = false,
                Message = message,
                Status = status
            };
        }
        public static MethodResult<T> ResultWithError(
             string message = "")
        {
            return new MethodResult<T>()
            {
                Success = false,
                Message = message,
                Status = (int)HttpStatusCode.Conflict
            };
        }
        public static MethodResult<T> ErrorBussiness(string message="")
        {
            if (string.IsNullOrEmpty(message))
                message = "Error bussiness";
            return new MethodResult<T> { Success = false, Message= message, Status = (int)HttpStatusCode.BadRequest };
        }
        public static MethodResult<T> ErrorBussiness(string[] messages)
        {
            string msg = string.Join(System.Environment.NewLine, messages);   
            return new MethodResult<T> { Success = false, Message = msg, Status = (int)HttpStatusCode.BadRequest };
        }
        public static MethodResult<T> ErrorExist()
        {
            return new MethodResult<T> { Success = false, Message = "Object exists", Status=(int)HttpStatusCode.NotModified };
        }
        public static MethodResult<T> ErrorNotFoundData()
        {
            return new MethodResult<T> { Success = false, Message = "Object not Found", Status=(int)HttpStatusCode.NotFound };
        }
        public MethodResult<T> Cast()
        {
            return this;
        }
    }
    public class MethodResult : IMethodResult
    {
        public bool Success { get; set; } = true;

        public string? Message { get; set; }

        public int? Status { get; set; }

        public static MethodResult ResultWithError(
          string message = "",
          int? status = (int)HttpStatusCode.BadRequest)
        {
            return new MethodResult()
            {
                Success = false,
                Message = message,
                Status = status
            };
        }
        public static MethodResult ResultWithError(
       string[] messages,
       int? status = (int)HttpStatusCode.BadRequest)
        {
            return new MethodResult()
            {
                Success = false,
                Message = string.Join(Environment.NewLine, messages),
                Status = status
            };
        }
        public static MethodResult ResultWithSuccess(string message = "")
        {
            return new MethodResult()
            {
                Success = true,
                Message = message,
                Status = (int)HttpStatusCode.OK
            };
        }
        public MethodResult Cast()
        {
            return this;
        }
    }
}
