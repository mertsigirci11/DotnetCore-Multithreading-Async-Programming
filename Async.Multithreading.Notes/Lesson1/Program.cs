#region Thread Class
//There're 3 types of creating thread

//Thread Creation
Thread thread1 = new Thread(() =>
{
    Console.WriteLine("Default thread worked.");
});

Thread thread2 = new Thread(ThreadMethod);

string x = "test";
Thread thread3 = new Thread((x) =>
{
    Console.WriteLine($"Object parameter thread worked => {x}");
});

//Thread Execution
thread1.Start();
thread2.Start();
thread3.Start();

void ThreadMethod()
{
    Console.WriteLine("Method parameter thread worked.");
}
#endregion

#region Thread Id
//We can obtain a thread id in 2 ways
//Environment.CurrentManagedThreadId
//or
//Thread.CurrentThread.ManagedThreadId

Console.WriteLine("Main Thread ID: " + Environment.CurrentManagedThreadId);
Console.WriteLine("Main Thread ID: " + Thread.CurrentThread.ManagedThreadId);
Thread threadId1 = new Thread(() =>
{
    Console.WriteLine("Worker Thread 1 ID: " + Environment.CurrentManagedThreadId);
    Console.WriteLine("Worker Thread 1 ID: " + Thread.CurrentThread.ManagedThreadId);
});
Thread threadId2 = new Thread(() =>
{
    Console.WriteLine("Worker Thread 2 ID: " + Environment.CurrentManagedThreadId);
    Console.WriteLine("Worker Thread 2 ID: " + Thread.CurrentThread.ManagedThreadId);
});

threadId1.Start();
threadId2.Start();
#endregion

#region IsBackground
//Thanks to IsBackground property, it can specify that whether thread run in background.
//A thread that will run in background will be depend on main thread.
//It means that when main thread terminates, related thread will terminates automatically,
//even though it doesn't complete its job.
//Default value of IsBackground property is false.

//P:S. : When you test this region, you should convert to commentline other codes
//because this code block may affect by other code lines. 

int counter = 10;
Thread threadIsBackground = new Thread(() =>
{
    while (counter >= 0)
    {
        counter--;
        Thread.Sleep(1000);
    }
    Console.WriteLine("threadIsBackground completed its mission.");
});

threadIsBackground.IsBackground = true;

threadIsBackground.Start();
Console.WriteLine("Main Thread completed its mission");
#endregion

#region Thread State
//There are 9 thread states
//1. Running          => It shows thread is running.
//2. StopRequested    => The thread has been requested to be stopped. 
//3. SuspendRequested => Thread suspension has been requested.
//4. Background       => It shows thread is working in background.
//5. Unstarted        => The thread has not started yet.
//6. Stopped          => The thread has been terminated.
//7. WaitSleepJoin    => The thread does wait other thread(s) or sleep. 
//8. Suspended        => The thread is pending.
//9. AbortRequested   => Thread termination has been requested.

int counter2 = 10;
Thread threadStateSample = new Thread(() =>
{
    while (counter2 >= 0)
    {
        counter2--;
        Thread.Sleep(500);
    }
    Console.WriteLine("threadIsBackground completed its mission.");
});

threadStateSample.Start();
var state = ThreadState.Running;

while (true)
{
    if (threadStateSample.ThreadState == ThreadState.Stopped)
    {
        break;
    }
    if (state != threadStateSample.ThreadState)
    {
        state = threadStateSample.ThreadState;
        Console.WriteLine(threadStateSample.ThreadState);
    }
}
Console.WriteLine("Main Thread completed its mission");
#endregion

#region Locking
//More than one threads may use same source(s) at the same time
//We should synchronize those threads
//However this may cause problem for our program
//We call this problem as race condition in multithread programming
//To prevent race condition(same source sharing conflict) there are some ways
//In this section we'll use lock mechanism
//We should write code to inside lock scope as less as possible
//We should write only necessary parts

object controller = new();
int counter3 = 1;

Thread threadLocking1 = new Thread(() =>
{
    lock (controller)
    {
        while (counter3 <= 10)
        {
            counter3++;
            Console.WriteLine($"ThreadLocking1 : {counter3}");
        }
    }
});

Thread threadLocking2 = new Thread(() =>
{
    lock (controller)
    {
        while (counter3 > -10)
        {
            counter3--;
            Console.WriteLine($"ThreadLocking2 : {counter3}");
        }
    }
});

threadLocking1.Start();
threadLocking2.Start();
#endregion

#region Sleep
//Another way to prevent race condition is sleep method.
//According to given parameter to sleep method, the thread will wait
Thread threadSleep = new Thread(() =>
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine("Sleep" + i);
        Thread.Sleep(1000);
    }
});
threadSleep.Start();

/* BEST PRACTICE
 * 
 * In an ideal thread operation, it is recommended to use the Sleep method 
 * even if it is given 0 seconds. Especially if there is a concern about 
 * the processing speed, you can create a relaxation time for the 
 * relevant thread and CPU by saying Thread.Sleep(0).
 */
#endregion

#region Join
//Another way to prevent race condition is join method.
//This is a method for wait another thread to finish its mission.
//The main thread is also blocked.
//Hence we can provide synchronization thanks to join method. 

Thread threadJoin1 = new Thread(() =>
{
    for (int i = 0; i < 10 ; i++) 
    {
        Console.WriteLine($"Join1 : " + i);
    }
});
Thread threadJoin2 = new Thread(() =>
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine($"Join2 : " + i);
    }
});
threadJoin1.Start();
threadJoin2.Start();
threadJoin2.Join();
#endregion

#region Thread Cancellation
//To cancel thread we used to use abort method but it is depriciated.
//Now there is more safe approach, point & terminate(graceful shutdown)
//We check condition or flag inside the thread and when that condition
//or flag happen, the thread is terminated.

bool isTerminate = false;
Thread threadCancellation = new Thread(() =>
{
    while(!isTerminate)
    {
        Console.WriteLine("threadCancellation is working.");
        Thread.Sleep(1000);
    }
    Console.WriteLine("threadCancellation terminated.");
});

Thread threadCancellationToken = new Thread((cancellationToken) =>
{
    var cancel = (CancellationTokenSource) cancellationToken;
    while (true)
    {
        if (cancel.IsCancellationRequested)
        {
            break;
        }
        Console.WriteLine("threadCancellationToken is working");
        Thread.Sleep(1000);
    }
    Console.WriteLine("threadCancellationToken terminated.");
});
CancellationTokenSource cancellationToken = new CancellationTokenSource();
threadCancellation.Start();
threadCancellationToken.Start(cancellationToken);
Thread.Sleep(5000);
cancellationToken.Cancel();
isTerminate = true;
#endregion

#region Interrupt
//This is the method used to wake up a thread from a waiting state and interrupt its running state.

//Note: If the woken thread is still in a waiting state (such as sleep or wait),
//a "ThreadInterruptException" error is thrown.

//With the Interrupt method, we can force a thread to finish while it is in a waiting state. 
//Or we can wake up a sleeping thread and allow it to continue its operations.

Thread threadInterrupt = new Thread(() =>
{
    try
    {
        Console.WriteLine("threadInterrupt is waiting.");
        Thread.Sleep(Timeout.Infinite);
    }
    catch (ThreadInterruptedException ex)
    {
        Console.WriteLine("ThreadInterruptedException has thrown.");
    }
});

threadInterrupt.Start();
threadInterrupt.Interrupt();
#endregion