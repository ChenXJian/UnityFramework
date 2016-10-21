using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public interface IPoolAgent
{
    void OnInit(string tag, Component pool);
    void OnReset();
    void OnRelease();
    void OnClear();
}

public class ObjectPool<T> where T : class, IPoolAgent, new()
{
    private Stack<T> objectStack;

    public string tag;
    public int countAll { get; private set; }
    public int countActive { get { return countAll - countInactive; } }
    public int countInactive { get { return objectStack.Count; } }

    public ObjectPool(string objectName, int size)
    {
        objectStack = new Stack<T>(size);
        tag = objectName;
    }

    public T Get(Component who)
    {
        if (objectStack.Count > 0)
        {
            T agent = objectStack.Pop();
            agent.OnReset();
            return agent;
        }
        else
        {
            
            T agent = new T();
            agent.OnInit(tag, who);
            
            countAll++;
            return agent;
        }
    }

    public void Release(T obj)
    {
        if (objectStack.Count > 0 && ReferenceEquals(objectStack.Peek(), obj))
            Debug.LogError("ObjectPool Trying to destroy object that is already released to pool.");
        obj.OnRelease();
        objectStack.Push(obj);
    }

    public void Clear()
    {
        while(objectStack.Count > 0)
        {
            T agent = objectStack.Pop();
            agent.OnClear();
        }
        objectStack.Clear();
        objectStack.TrimExcess();
    }
}