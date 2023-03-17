using System;
using System.Collections.Generic;
using System.Threading;

namespace FirstTask
{
    class EventThrottler<T> where T : EventArgs
    {
        private object lockObj = new object();
        private Queue<T> eventQueue = new Queue<T>();
        private List<EventHandler<T>> eventHandlers = new List<EventHandler<T>>();
        private int throttling;
        public EventThrottler(int throttling)
        {
            throttling = throttling;
            new Thread(EventThread).Start();
        }

        public void Register(EventHandler<T> handler)
        {
            lock (lockObj)
            {
                eventHandlers.Add(handler);
            }
        }

        public void Unregister(EventHandler<T> handler)
        {
            lock (lockObj)
            {
                eventHandlers.Remove(handler);
            }
        }

        public void Raise(object sender, T args)
        {
            lock (lockObj)
            {
                eventQueue.Enqueue(args);
            }
        }

        private void EventThread()
        {
            while (true)
            {
                T[] eventsToProcess;

                lock (lockObj)
                {
                    eventsToProcess = eventQueue.ToArray();
                    eventQueue.Clear();
                }

                foreach (var e in eventsToProcess)
                {
                    lock (lockObj)
                    {
                        foreach (var handler in eventHandlers)
                        {
                            handler?.Invoke(this, e);
                            Thread.Sleep(throttling);
                        }
                    }
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var eventBus = new EventThrottler<EventArgs>(500);
            EventHandler<EventArgs> handler1 = (sender, args) => Console.WriteLine("Handler1 called");
            EventHandler<EventArgs> handler2 = (sender, args) => Console.WriteLine("Handler2 called");
            EventHandler<EventArgs> handler3 = (sender, args) => Console.WriteLine("Handler3 called");
            eventBus.Register(handler1);
            eventBus.Register(handler2);
            eventBus.Register(handler3);
            eventBus.Raise(null, EventArgs.Empty);
            Thread.Sleep(2000);
            eventBus.Unregister(handler2);
            eventBus.Raise(null, EventArgs.Empty);
            Console.ReadKey();
        }
    }
}