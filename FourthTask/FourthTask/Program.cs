using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Publisher
{
    public event EventHandler<ActionEventArgs> ActionEvent;

    public void PerformAction(string actionName)
    {
        var actionEventArgs = new ActionEventArgs(actionName);
        ActionEvent?.Invoke(this, actionEventArgs);
    }
}
class ActionEventArgs : EventArgs
{
    public string ActionName { get; private set; }

    public ActionEventArgs(string actionName)
    {
        ActionName = actionName;
    }
}
class Subscriber
{
    public string Name { get; private set; }
    private List<string> _subscribedActions;

    public Subscriber(string name)
    {
        Name = name;
        _subscribedActions = new List<string>();
    }

    public void SubscribeToAction(string actionName)
    {
        _subscribedActions.Add(actionName);
    }

    public void UnsubscribeFromAction(string actionName)
    {
        _subscribedActions.Remove(actionName);
    }

    public void OnAction(object sender, ActionEventArgs args)
    {
        if (_subscribedActions.Contains(args.ActionName))
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            Console.WriteLine($"{Name} виконує {args.ActionName}");
        }
    }
}
class WorkflowManager
{
    private Publisher _publisher;
    private List<Subscriber> _subscribers;

    public WorkflowManager()
    {
        _publisher = new Publisher();
        _subscribers = new List<Subscriber>();
    }

    public void RegisterSubscriber(Subscriber subscriber)
    {
        _subscribers.Add(subscriber);
        _publisher.ActionEvent += subscriber.OnAction;
    }

    public void UnregisterSubscriber(Subscriber subscriber)
    {
        _subscribers.Remove(subscriber);
        _publisher.ActionEvent -= subscriber.OnAction;
    }

    public void PerformWorkflow()
    {
        _publisher.PerformAction("action1");
        _publisher.PerformAction("action2");
        _publisher.PerformAction("action3");
        _publisher.PerformAction("action4");
        _publisher.PerformAction("action5");
    }
}
class Program
{
    public static void Main()
    {
        var subscriber1 = new Subscriber("Subscriber 1");
        subscriber1.SubscribeToAction("action1");
        subscriber1.SubscribeToAction("action3");
        var subscriber2 = new Subscriber("Subscriber 2");
        subscriber2.SubscribeToAction("action2");
        subscriber2.SubscribeToAction("action4");
        subscriber2.SubscribeToAction("action5");
        var workflowManager = new WorkflowManager();
        workflowManager.RegisterSubscriber(subscriber1);
        workflowManager.RegisterSubscriber(subscriber2);
        workflowManager.PerformWorkflow();
        Console.ReadKey();
    }
}
