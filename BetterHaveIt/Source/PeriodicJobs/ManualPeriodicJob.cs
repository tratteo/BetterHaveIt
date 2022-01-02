// Copyright Matteo Beltrame

namespace BetterHaveIt.PeriodicJobs;

public class ManualPeriodicJob : PeriodicJob
{
    protected float currentTime = 0F;
    private Task? currentTask = null;

    public ManualPeriodicJob(Action job, float intervalMilliseconds) : base(job, intervalMilliseconds)
    {
        Active = true;
    }

    public ManualPeriodicJob(Func<Task> asyncJob, float intervalMilliseconds) : base(asyncJob, intervalMilliseconds)
    {
        Active = true;
    }

    public override void EditInterval(float intervalMilliseconds)
    {
        base.EditInterval(intervalMilliseconds);
        currentTime = 0F;
    }

    public float GetIntervalProgress() => currentTime / intervalMilliseconds;

    public void Step(float deltaTime)
    {
        if (!Active) return;
        if (executionContext == ExecutionContext.Async && currentTask != null && !currentTask.IsCompleted) return;

        currentTime += deltaTime;
        if (ShouldExecute())
        {
            switch (executionContext)
            {
                case ExecutionContext.Sync:
                    job?.Invoke();
                    break;

                case ExecutionContext.Async:
                    currentTask = asyncJob?.Invoke();
                    break;
            }
            currentTime = 0F;
        }
    }

    protected bool ShouldExecute() => currentTime >= intervalMilliseconds;
}