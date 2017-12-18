/// CSIC-0640 Operating Systems - Summer/2016 - Prof. Dr.Simco
/// Assignment #2
/// Andre Vieira - N01693058
////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SIMMAC
{
    class RoundRobin
    {
        // Queue of Processes that wants to work.
        public Queue<Job> queueOfProcesses = new Queue<Job>();

        //Queue of Threads which will handle each process (each process shall run on an independent thread).
        public Queue<Thread> queueOfThreads = new Queue<Thread>();

        // Lock object, something to contest about.
        object lockObj = new object();

        //Job counter
        int jobCounter = 0;

        public RoundRobin(List<Job> jList)
        {
            jobCounter = jList.Count;           

            foreach (Job j in jList)
            {
                //Create a local copy for processing
                Job localCopy = j;
                               
                //Create a new thread to process the Job
                Thread t = new Thread(() => localCopy.process(j.instList));

                //Enqueue Processes and Threads
                this.EnqueueProcesses(j);
                this.EnqueueThreads(t);                          
            }           
        }

        private bool ProcessesDone()
        {
            bool ret = true;

            if(queueOfProcesses.Count != 0)
                ret = false;                   
            
            return ret;       
        }

        private void ContinueProcessJob(Job working, Thread workingThread)
        {
            workingThread = new Thread(() => working.process(working.instList));
            workingThread.Start();
        }

        private void StartProcess(Job working, Thread workingThread)
        {
            // If the process did not yet finish its current work, let it continue
            if (!working.getTempDone())
            {
                // In the very 1st iteration this if condition not necessary, but starting from iteration N+1 it matters
                // Usually the thread goes into the "Stopped" state once it finishes its task within the assoicated time slice
                // So, if it stopped we instaninate a new one and let it work, otherwise its a brand new thread so we just start it for the 1st time
                if (workingThread.ThreadState != ThreadState.Stopped)
                {
                    Console.WriteLine("Thread of process: " + working.getProcessID() + " state before starting: " + workingThread.ThreadState);
                    workingThread.Start();
                    Console.WriteLine("Thread of process #: " + working.getProcessID() + " is starting.");
                }
                else if (workingThread.ThreadState == ThreadState.Stopped)
                {
                    ContinueProcessJob(working, workingThread);
                }
            }
        }

        private void DoProcessJob(Job working, Thread workingThread)
        {
            // If this thread did not exceed its time quantum limit, go ahead and give it a chance to work
            if (working.getTimeQuantum() > 0)
            {
                // Secure this working zone so no other threads can interupt us
                lock (lockObj)
                {
                    StartProcess(working, workingThread);
                }
            }
        }

        private void EnqueueProcesses(Job aJob)
        {
            queueOfProcesses.Enqueue(aJob);               
        }

        private void EnqueueThreads(Thread aThread)
        {
            queueOfThreads.Enqueue(aThread);
        }

        private void SetProcessTimeQuantum(Job working, int time)
        {
            // Assigning a Time quantum for this thread of "2".
            working.setTimeQuantum(working.getTimeQuantum() + time);
            working.setStartPoint(working.getTimeQuantum() - time);
        }

        private void SleepOurMainThread(Job working)
        {
            // This disables the Main Thread from working while the sub-thread is working
            // Because the main thread will iterate this entire loop and dequeue another working process & thread if we did not tell him
            // We are busy, so wait and stay here till we are really done.
            while (!working.getTempDone())
            {
                Thread.Sleep(1000);
            }
        }

        private void CheckIfProcessStillHaveJobLeft(Job working, Thread workingThread, Queue<Job> queueOfProcesses, Queue<Thread> queueOfThreads)
        {
            // Here we check if have done our time sliced job, but yet, we are not completely done (we need more time slices)
            // Put the process at the end of working processes queue, also put the Thread which handle our process at the end of Threads queue.
            // Otherwise, the thread time slice was equal or less than its burst time (numberOfWordsToType), so we dont schedule it again.
            if (working.getTempDone() && !working.isDone())
            {
                queueOfProcesses.Enqueue(working);
                queueOfThreads.Enqueue(workingThread);
                Console.WriteLine("Enqueuing: " + working.getProcessID() + "\n");
            }
        }

        public void doRoundRobin(int quantum)
        {
            Job working;
            Thread workingThread;

            while (!ProcessesDone())
            {
                working = queueOfProcesses.Dequeue();
                workingThread = queueOfThreads.Dequeue();

                SetProcessTimeQuantum(working, quantum);
                DoProcessJob(working, workingThread);
                SleepOurMainThread(working);
                CheckIfProcessStillHaveJobLeft(working, workingThread, queueOfProcesses, queueOfThreads);
            }
        }
    }
}
