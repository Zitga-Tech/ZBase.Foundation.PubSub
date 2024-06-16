﻿namespace ZBase.Foundation.PubSub.Samples
{
    ///*************************************************************************************///
    /// In strict (default) mode, messages must implement <see cref="IMessage"/> interface. ///
    /// To disable this mode, add `ZBASE_FOUNDATION_PUBSUB_RELAX_MODE` to                   ///
    /// Project Settings > Player > Scripting Define Symbols                                ///
    ///*************************************************************************************///

    public struct FooMessage : IMessage
    {
        public string value;
    }

    public struct BarMessage : IMessage
    {
        public int value;
    }

    public struct TimeMessage : IMessage
    {
        public float seconds;
    }

    public struct CancellableTimeMessage : IMessage
    {
        public float seconds;
    }

    public struct FrameMessage : IMessage
    {
        public int frames;
    }

    public struct DeltaTimeMessage : IMessage
    {
        public float value;
    }

    public static class PublishContextExtensions
    {
        public static string ToLog(this CallerInfo self)
            => $"[{self.CallerMemberName} @ {self.CallerFilePath}:{self.CallerLineNumber}]";
    }
}
