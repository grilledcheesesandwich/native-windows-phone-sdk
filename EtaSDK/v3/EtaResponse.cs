using System;

namespace EtaSDK.v3
{
    public class EtaResponse<T> 
    {
        public T Result { get; private set; }
        public Exception Error { get; private set; }

        public bool HasErrors { get { return Error != null; } }

        public EtaResponse(T result)
            : this(result, null)
        {
        }

        public EtaResponse(Exception error)
            : this(default(T), error)
        {
        }

        public EtaResponse(T result, Exception error)
        {
            Result = result;
            Error = error;
        }
    }
}
