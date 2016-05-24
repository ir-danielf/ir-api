using System;
using System.Runtime.Serialization;

namespace IRLib.Paralela.HammerHead
{
    public class HammerHeadException : Exception
    {

        public HammerHeadException() : base() { }

        public HammerHeadException(string msg) : base(msg) { }

        public HammerHeadException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }


    public class TimeoutException : Exception
    {

        public TimeoutException() : base() { }

        public TimeoutException(string msg) : base(msg) { }

        public TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
