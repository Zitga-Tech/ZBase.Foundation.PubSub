namespace ZBase.Foundation.PubSub
{
    public readonly struct CallerInfo
    {
        public string CallerMemberName { get; }

        public string CallerFilePath { get; }

        public int CallerLineNumber { get; }

        public bool IsCreated { get; }

        public CallerInfo(
              int callerLineNumber
            , string callerMemberName
            , string callerFilePath
        )
        {
            CallerLineNumber = callerLineNumber;
            CallerMemberName = callerMemberName;
            CallerFilePath = callerFilePath;
            IsCreated = true;
        }
    }
}
