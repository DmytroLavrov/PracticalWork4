using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondTask
{
    class Publisher
    {
        public event EventHandler<Event> Handled;
        public void PublishEvent(Event events)
        {
            Handled?.Invoke(this, events);
        }
    }
    abstract class Subscriber
    {
        public abstract void Subscribe(object sender, Event events);
    }
    class PriorityHighSubscribe : Subscriber
    {
        public override void Subscribe(object sender, Event events)
        {
            if (events.Priority > 2)
            {
                Console.OutputEncoding = System.Text.Encoding.Default;
                Console.WriteLine($"'{events.Name}' обробка із високим пріоритетом {nameof(PriorityHighSubscribe)}");
            }
        }
    }
    class PriorityMediumSubscribe : Subscriber
    {
        public override void Subscribe(object sender, Event events)
        {
            if (events.Priority == 2)
            {
                Console.OutputEncoding = System.Text.Encoding.Default;
                Console.WriteLine($"'{events.Name}' обробка із середнім пріоритетом {nameof(PriorityMediumSubscribe)}");
            }
        }
    }
    class PriorityLowSubscribe : Subscriber
    {
        public override void Subscribe(object sender, Event events)
        {
            if (events.Priority < 2)
            {
                Console.OutputEncoding = System.Text.Encoding.Default;
                Console.WriteLine($"'{events.Name}' обробка із низьким пріоритетом {nameof(PriorityLowSubscribe)}");
            }
        }
    }
    class Event : EventArgs
    {
        public int Priority { get; set; }
        public string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Publisher publisher = new Publisher();
            Console.OutputEncoding = System.Text.Encoding.Default;
            PriorityHighSubscribe priorityHighSubscribe = new PriorityHighSubscribe();
            publisher.Handled += priorityHighSubscribe.Subscribe;
            PriorityMediumSubscribe priorityMediumSubscribe = new PriorityMediumSubscribe();
            publisher.Handled += priorityMediumSubscribe.Subscribe;
            Event eventN1 = new Event();
            eventN1.Name = "Подія N1";
            eventN1.Priority = 3;
            Event eventN2 = new Event();
            eventN1.Name = "Подія N2";
            eventN1.Priority = 2;
            publisher.PublishEvent(eventN1);
            publisher.PublishEvent(eventN2);
            Console.ReadKey();
        }
    }
}