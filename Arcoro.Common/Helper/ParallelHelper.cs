using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Arcoro.Common.Helper
{
    public static class ParallelHelper
    {
        public static Task AsyncParallelForEach<T>(this IEnumerable<T> source, Func<T, Task> body, int maxDegreeOfParallelism = DataflowBlockOptions.Unbounded, TaskScheduler scheduler = null)
        {
            ExecutionDataflowBlockOptions options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
            if (scheduler != null)
            {
                options.TaskScheduler = scheduler;
            }

            ActionBlock<T> block = new ActionBlock<T>(body, options);

            foreach (T item in source)
            {
                block.Post(item);
            }

            block.Complete();
            return block.Completion;
        }
    }
}