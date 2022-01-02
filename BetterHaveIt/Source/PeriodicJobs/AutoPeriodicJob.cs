// Copyright Matteo Beltrame

namespace BetterHaveIt.PeriodicJobs;

public class AutoPeriodicJob : PeriodicJob
{
    private CancellationTokenSource? cancellationSource;
    private bool active;

    public override bool Active
    {
        get => active;
        set
        {
            if (value == active) return;
            if (value)
            {
                cancellationSource = new CancellationTokenSource();
                Step(cancellationSource.Token);
            }
            else
            {
                cancellationSource?.Cancel();
            }
            active = value;
        }
    }

    public AutoPeriodicJob(Action job, float intervalMilliseconds) : base(job, intervalMilliseconds)
    {
    }

    public AutoPeriodicJob(Func<Task> asyncJob, float intervalMilliseconds) : base(asyncJob, intervalMilliseconds)
    {
    }

    public override void EditInterval(float intervalMilliseconds)
    {
        base.EditInterval(intervalMilliseconds);
        if (Active)
        {
            Active = false;
            Active = true;
        }
    }

    private async void Step(CancellationToken cancellationToken)
    {
        PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(intervalMilliseconds));
        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                switch (executionContext)
                {
                    case ExecutionContext.Sync:
                        job?.Invoke();
                        break;

                    case ExecutionContext.Async:
                        if (asyncJob is not null)
                        {
                            await asyncJob.Invoke();
                        }
                        break;
                }
            }
        }
        catch (Exception)
        {
        }
    }
}