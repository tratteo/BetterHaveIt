// Copyright Matteo Beltrame

using System;
using System.Threading.Tasks;

namespace NiceToHave;

public abstract class PeriodicJob
{
    protected enum ExecutionContext
    { Sync, Async }

    protected readonly Action? job;
    protected readonly Func<Task>? asyncJob;
    protected readonly ExecutionContext executionContext;
    protected float intervalMilliseconds;

    public virtual bool Active { get; set; } = false;

    protected PeriodicJob(Action job, float intervalMilliseconds) : this(intervalMilliseconds, ExecutionContext.Sync)
    {
        this.job = job;
    }

    protected PeriodicJob(Func<Task> asyncJob, float intervalMilliseconds) : this(intervalMilliseconds, ExecutionContext.Async)
    {
        this.asyncJob = asyncJob;
    }

    private PeriodicJob(float intervalMilliseconds, ExecutionContext executionContext)
    {
        this.intervalMilliseconds = intervalMilliseconds;
        this.executionContext = executionContext;
    }

    public virtual void EditInterval(float intervalMilliseconds) => this.intervalMilliseconds = intervalMilliseconds;
}