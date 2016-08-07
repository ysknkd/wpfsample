using System;

namespace Common
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

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
