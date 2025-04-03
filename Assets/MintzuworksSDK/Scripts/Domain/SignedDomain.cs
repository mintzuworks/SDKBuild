using System;


namespace Mintzuworks.Domain
{
    [Serializable]
    public class SignedData<T>
    {
        public long time;
        public T data;
    }
}