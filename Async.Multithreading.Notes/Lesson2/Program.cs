// Thread Synchronization & Blocking Synchronization Structures

#region Spinning
/*
 *This approach aims that due to spesific condition, waiting/blocking threads 
 *by loop mechanism.
 *
 *This provides that the thread execution and prevent switch to other thread(s) 
 *until meet the condition.
 *
 *This waiting process named as busy-waiting or spinning.
 *
 *This approach may cause using cpu relentlesly, so that cpu sources are may used harsly.
 *If operations last not long, this approach can implement. 
*/

bool threadSpinningCondition = true;
int i = 0;

Thread threadSpinning1 = new(() =>
{
    while (true)
    {
        if(threadSpinningCondition)
        {
            Console.WriteLine("ThreadSpinning 1 : {0}", i++);
            Thread.Sleep(100);
            if (i == 10)
            {
                threadSpinningCondition = false;
                break;
            }
        }
    }
});

Thread threadSpinning2 = new(() =>
{
    while (true)
    {
        if (!threadSpinningCondition)
        {
            Console.WriteLine("ThreadSpinning 2 : {0}", i--);
            Thread.Sleep(100);
            if (i == 0) 
            {
                threadSpinningCondition = true;
                break;
            }
        }
    }
});

threadSpinning1.Start();
threadSpinning2.Start();
#endregion

#region Monitor.Enter & Monitor.Exit -> LockTaken
/*
 *Monitor.Enter and Monitor.Exit methods are functional version of locking mechanism.
 *
 *Monitor.Enter tries locking through an/a object/variable, just as lock mechanism.
 *So, other threads can't access that critical area during locking.
 *
 *Monitor.Exit unlocks the locked object/variable.
 *
 *Monitor.Enter & Monitor.Exit methods must add into all threads which use same sources.
 *If you add into only one thread these methods, synchronization will not be provided.
 *
 *After using Monitor.Enter method, we must perform operations inside try block and
 *write Monitor.Exit method inside the finally block. Because if exception occurs,
 *the locked object must unlocked for other threads' execution.
 *
 *Rarely, Monitor.Enter method fails locking. There is an overload for 
 *Monitor.Enter method. Thanks to this overload we can check the object is locked or not.
 *
 *Deficieny of this approach is we are not able to set threads' working sequence.
 *
 */
Object lockObject = new();
int counter = 0;
Thread threadMonitor1 = new Thread(() =>
{
    try
    {
        bool lockTaken = false;
        Monitor.Enter(lockObject, ref lockTaken);

        if (lockTaken)
        {
            for (int i = 0; i < 100; i++)
            {
                counter++;
                Console.WriteLine("threadMonitor 1: {0}", counter);
            }
        }
    }
    finally
    {
        Monitor.Exit(lockObject);
    }
});
Thread threadMonitor2 = new Thread(() =>
{
    
    try
    {
        bool lockTaken = false;
        Monitor.Enter(lockObject);

        if (lockTaken)
        {
            for (int i = 0; i < 100; i++)
            {
                counter--;
                Console.WriteLine("threadMonitor 2: {0}", counter);
            }
        }
    }
    finally
    {
        Monitor.Exit(lockObject);
    }
});

threadMonitor1.Start();
threadMonitor2.Start();
#endregion

#region Monitor.TryEnter
/*
 * Mock.TryEnter is a synchronization method that attempts to lock an object and 
 * checks whether it is successful.
 * 
 * The locking time is also given as a parameter to this method (in milliseconds).
 * If the lock cannot be achieved within the given time, false is returned.
 * 
 * And of course we should also use Monitor.Exit in the finally scope.
 * 
 */
object lockObject2 = new object();
int counter2 = 0;

Thread threadMonitorTryEnter1 = new(() =>
{
    bool lockTaken = false;
    try
    {
        Monitor.TryEnter(lockObject2, 10, ref lockTaken);
        if (lockTaken)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("threadMonitorTryEnter 1: {0}", counter2);
                Thread.Sleep(100);
                counter2++;
            }
        }
    }
    finally
    {
        if (lockTaken) { Monitor.Exit(lockObject2); }
    }
});

Thread threadMonitorTryEnter2 = new(() =>
{
    bool lockTaken = false;
    try
    {
        lockTaken = Monitor.TryEnter(lockObject2,100);
        if (lockTaken)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("threadMonitorTryEnter 2: {0}", counter2);
                Thread.Sleep(100);
                counter2++;
            }
        }
    }
    finally
    {
        if (lockTaken) {  Monitor.Exit(lockObject2); }
    }
});

threadMonitorTryEnter1.Start();
threadMonitorTryEnter2.Start();
#endregion

#region Mutex Class
/*
 *Unlike other types of synchronization, it is a mechanism that 
 *can perform inter-process locking at the computer (process) level, 
 *not at the thread level.
 *
 *
 */
Mutex mutex = new Mutex();
int counter3 = 0;
Thread threadMutex1 = new(() =>
{
    mutex.WaitOne();
    for (int i = 0;i < 10; i++)
    {
        Console.WriteLine("threadMutex 1: {0}", counter3);
        counter3++;
        Thread.Sleep(100);
    }
    mutex.ReleaseMutex();
});

Thread threadMutex2 = new(() =>
{
    mutex.WaitOne();
    for(int i = 0;i < 10; i++)
    {
        Console.WriteLine("threadMutex 2: {0}", counter3);
        counter3--;
        Thread.Sleep(100);
    }
    mutex.ReleaseMutex();
});

threadMutex1.Start();
threadMutex2.Start();
#endregion


#region Single Instance Application With Mutex
/*
 *With Single Instance Application, we can ensure that only a single instance 
 *of a compiled application is run.
 */
Mutex _mutex;
string _programName = "Example Project";
Mutex.TryOpenExisting(_programName, out _mutex);
if(_mutex == null)
{
    _mutex = new(true, _programName);
    Console.WriteLine("Program is working");
    Console.Read();
}
else
{
    _mutex.Close();
}
#endregion