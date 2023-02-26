using System;
using System.Collections.Generic;
using System.Text;

namespace MonoBundle.Events
{
	public class Scheduler
	{
		public List<Event> ScheduledEvents;
		public List<Event> Queue;

		public Scheduler()
		{
			ScheduledEvents = new();
			Queue = new();
		}

		public void Update()
		{
			List<Event> events_to_remove = new();
			List<Event> events_to_add = new();
			foreach(var _event in ScheduledEvents)
			{
				_event.CallCount++;
				if(_event.CallCount >= _event.Lifetime)
				{
					if (_event.Repeating)
					{
						_event.CallCount = 0;
					}
						
					else
					{
						foreach (var queued in Queue)
						{
							if (queued.Signature == _event.Signature)
							{
								events_to_add.Add(queued);
								break;
							}
						}
						events_to_remove.Add(_event);
					}
					_event.OnFinish(_event);
					continue;
				}
				_event.OnUpdate(_event);
			}
			foreach (var e in events_to_add)
				ScheduledEvents.Add(e);
			foreach (var e in events_to_remove)
				ScheduledEvents.Remove(e);
		}
		public void Add(Event e)
		{
			switch (e.SignatureType)
			{
				case EventSignatureType.Enqueue:
					bool event_found = false;
					foreach (var _event in ScheduledEvents)
					{
						if (_event.Signature == e.Signature)
						{
							event_found = true;
							break;
						}
					}
					if (event_found)
						Queue.Add(e);
					else
						ScheduledEvents.Add(e);
					break;
				case EventSignatureType.Discard:
					bool e_found = false;
					foreach (var _event in ScheduledEvents)
					{
						if (_event.Signature == e.Signature)
						{
							e_found = true;
							break;
						}
					}
					if(!e_found)
						ScheduledEvents.Add(e);
					break;
				case EventSignatureType.Override:
					Event? toremove = null;
					foreach (var _event in ScheduledEvents)
					{
						if (_event.Signature == e.Signature)
						{
							toremove = _event;
							break;
						}
					}
					if (toremove != null)
						ScheduledEvents.Remove(toremove);
					ScheduledEvents.Add(e);
					break;
				case EventSignatureType.Summate:
					ScheduledEvents.Add(e);
					break;
				default:
					break;
			}
		}
	}

	public enum EventSignatureType
	{
		//When event with the same signate is added, it is enqueued instead
		Enqueue,
		//The replacement is discarded, and the active event is left untouched
		Discard,
		//The active event is removed and the replacement is added
		Override,
		//The event is added to active events regardless, creating 2 copies
		Summate
	}
	public class Event
	{
		public int CallCount = 0;
		public int Lifetime;
		public string Signature;
		public EventSignatureType SignatureType;
		public bool Repeating = false;
		
		public float Progress { get => (float)CallCount / Lifetime; }

		public Action<Event> OnUpdate;
		public Action<Event> OnFinish;

		public Event(int lifetime, Action<Event> onupdate,
			EventSignatureType sigtype = EventSignatureType.Summate, string signature = "")
		{
			Lifetime = lifetime;
			OnUpdate = onupdate;
			SignatureType = sigtype;
			Signature = signature;
			OnFinish = (e) => { };
		}

		public static Event Schedule(int lifetime, Action<Event> onfinish)
		{
			var newev = new Event(lifetime, (e) => { });
			newev.OnFinish = onfinish;
			return newev;
		}
		public Event Repeat()
		{
			Repeating = true;
			return this;
		}
	}

	public static class Easing
	{
		public static Func<float, float> InSine = t => (float)(1 - Math.Cos(t * Math.PI / 2));
		public static Func<float, float> OutSine = t => (float)Math.Sin(t * Math.PI / 2);
		public static Func<float, float> InOutSine = t => (float)-(Math.Cos(Math.PI * t) - 1) / 2;

		public static Func<float, float> InCubic = t => t * t * t;
		public static Func<float, float> OutCubic = t => (float)(1 - Math.Pow(1 - t, 3));
		public static Func<float, float> InOutCubic = t => t < 0.5 ? 4 * t * t * t : (float)(1 - Math.Pow(-2 * t + 2, 3) / 2);

		public static Func<float, float> InElastic = t =>
		{
			float c4 = (float)(2 * Math.PI) / 3;

			return (float)(t == 0
			  ? 0
			  : t == 1
				? 1
				: -Math.Pow(2, 10 * t - 10) * Math.Sin((t * 10 - 10.75) * c4));
		};
		public static Func<float, float> OutElastic = t =>
		{
			float c4 = (float)(2 * Math.PI) / 3;

			return (float)(t == 0
			  ? 0
			  : t == 1
			  ? 1
			  : Math.Pow(2, -10 * t) * Math.Sin((t * 10 - 0.75) * c4) + 1);
		};
		public static Func<float, float> InOutElastic = t =>
		{
			float c5 = (float)(2 * Math.PI) / 4.5f;

			return (float)(t == 0
			  ? 0
			  : t == 1
			  ? 1
			  : t < 0.5
			  ? -(Math.Pow(2, 20 * t - 10) * Math.Sin((20 * t - 11.125f) * c5)) / 2
			  : (Math.Pow(2, -20 * t + 10) * Math.Sin((20 * t - 11.125f) * c5)) / 2 + 1);
		};

		public static Func<float, float> InBack = t =>
		{
			float c1 = 1.70158f;
			float c3 = c1 + 1;

			return c3 * t * t * t - c1 * t * t;
		};
		public static Func<float, float> OutBack = t =>
		{
			float c1 = 1.70158f;
			float c3 = c1 + 1;

			return (float)(1 + c3 * Math.Pow(t - 1, 3) + c1 * Math.Pow(t - 1, 2));
		};
		public static Func<float, float> InOutBack = t =>
		{
			float c1 = 1.70158f;
			float c2 = c1 * 1.525f;

			return (float)(t < 0.5
			  ? (Math.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
			  : (Math.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2);
		};
	}

}
