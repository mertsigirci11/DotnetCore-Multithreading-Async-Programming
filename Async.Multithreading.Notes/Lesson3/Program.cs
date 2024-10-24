#region Semaphore & SemaphoreSlim Classes Information
/*
 *Semaphore and SemaphoreSlim classes give specified number of permission to threads 
 *for accessing specific sources and manage these operations.
 *
 *These classes give & take back permission to different threads accessing 
 *specific sources for runnig concurrently.
 *
 *Before threads access the source(s), they need to permission by one of these classes.
 *If one of these class doesn't give permission, the thread(s) waits permission.
 *
 *While one of these classes is creatig, we should give 2 mandatory parameters.
 *These are initial count and maximum count.
 *Initial count specifies how many permission the semaphore has.
 *Maximum count specifies how many thread can work concurrently.
 *
 *This approach is slower than other synchronization approaches. If there are huge
 *operations, this approach is suitable.
 */
#endregion

#region Semaphore & SemaphoreSlim Classes Comparisation
/*
 *Semaphore class works at cpu-level -> SemaphoreSlim class works at .net app-level.
 * 
 *SemaphoreSlim works faster and consume low memory but Semaphore has more handling options.
 * 
 *Semapohore works synchronously but SemaphoreSlim can works synchronously or asynchronously.
 */
#endregion

#region Semaphore Example
List<int> numbers = new();
Semaphore semaphore = new Semaphore(2, 2);

Thread threadSemaphore1 = new Thread(() =>
{
    semaphore.WaitOne();//Thread asked permission
    int i = 0;
    while (i < 10)
    {
        Console.WriteLine("threadSemaphore1 added : {0}", i++);
        numbers.Add(i);
        Thread.Sleep(100);
    }
    semaphore.Release();//Permisson given back
});

Thread threadSemaphore2 = new Thread(() =>
{
    semaphore.WaitOne();//Thread asked permission
    int i = 10;
    while (i < 20)
    {
        Console.WriteLine("threadSemaphore2 added : {0}", i++);
        numbers.Add(i);
        Thread.Sleep(100);
    }
    semaphore.Release();
});

Thread threadSemaphore3 = new Thread(() =>
{
    semaphore.WaitOne();//Thread asked permission
    int i = 20;
    while (i < 30)
    {
        Console.WriteLine("threadSemaphore3 added : {0}", i++);
        numbers.Add(i);
        Thread.Sleep(100);
    }
    semaphore.Release();//Permisson given back
});

threadSemaphore1.Start();
threadSemaphore2.Start();
threadSemaphore3.Start();
#endregion

#region SemaphoreSlim Example
List<int> numbers2 = new();
SemaphoreSlim semaphoreSlim = new SemaphoreSlim(2, 2);

Thread threadSemaphoreSlim1 = new Thread(async () =>
{
    await semaphoreSlim.WaitAsync();//Thread asked permission
    int i = 0;
    while (i < 10)
    {
        Console.WriteLine("threadSemaphoreSlim1 added : {0}", i++);
        numbers2.Add(i);
        Thread.Sleep(100);
    }
    semaphoreSlim.Release();//Permisson given back
});

Thread threadSemaphoreSlim2 = new Thread(async () =>
{
    await semaphoreSlim.WaitAsync();//Thread asked permission
    int i = 10;
    while (i < 20)
    {
        Console.WriteLine("threadSemaphoreSlim2 added : {0}", i++);
        numbers2.Add(i);
        Thread.Sleep(100);
    }
    semaphoreSlim.Release();
});

Thread threadSemaphoreSlim3 = new Thread(async () =>
{
    await semaphoreSlim.WaitAsync();//Thread asked permission
    int i = 20;
    while (i < 30)
    {
        Console.WriteLine("threadSemaphoreSlim3 added : {0}", i++);
        numbers2.Add(i);
        Thread.Sleep(100);
    }
    semaphoreSlim.Release();//Permisson given back
});

threadSemaphoreSlim1.Start();
threadSemaphoreSlim2.Start();
threadSemaphoreSlim3.Start();
#endregion