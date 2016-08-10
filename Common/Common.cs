using System;

namespace Common
{
    public enum TaskResultType
    {
        SUCCEEDED,
        EREQUIRED,
        EBADPARAM,
        EFAILED
    }

    public class TaskResult
    {
        public TaskResultType result { get; set; }
        public string propertyName { get; set; }
    }
}
