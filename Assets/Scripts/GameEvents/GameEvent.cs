using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
	private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

	public void Raise()
	{
		for (int i = _listeners.Count - 1; i >= 0; i--)
			_listeners[i].OnEventRaised();
	}

	public void RegisterListener(GameEventListener listener)
	{ _listeners.Add(listener); }

	public void UnregisterListener(GameEventListener listener)
	{ _listeners.Remove(listener); }
}

public abstract class GameEvent<T> : ScriptableObject
{
	private readonly List<GameEventListener<T>> _listeners = new List<GameEventListener<T>>();

	public void Raise(T arg)
	{
		for (int i = _listeners.Count - 1; i >= 0; i--)
			_listeners[i].OnEventRaised(arg);
	}

	public void RegisterListener(GameEventListener<T> listener)
	{ _listeners.Add(listener); }

	public void UnregisterListener(GameEventListener<T> listener)
	{ _listeners.Remove(listener); }
}

