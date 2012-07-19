using System;

namespace EtaSDK.v3
{
    public class EtaResponse<T> 
    {
        public T Result { get; private set; }
        public Uri Uri { get; private set; }
        public Exception Error { get; private set; }

        public bool HasErrors { get { return Error != null; } }

        public EtaResponse(Uri uri,T result)
            : this(uri,result, null)
        {
        }

        public EtaResponse(Uri uri,Exception error)
            : this(uri, default(T), error)
        {
        }

        public EtaResponse(Uri uri,T result, Exception error)
        {
            Result = result;
            Error = error;
            Uri = uri;
        }
    }

    
}
