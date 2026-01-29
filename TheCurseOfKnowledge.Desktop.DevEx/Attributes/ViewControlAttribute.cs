using System;

namespace TheCurseOfKnowledge.Desktop.DevEx.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ViewControlAttribute : Attribute
    {
        public string Key { get; }
        public string Title { get; }
        public int Index { get; }
        public string Icon { get; }
        public bool Active { get; }
        public object State { get; set; }
        public ViewControlAttribute(string key, string title, int index, string icon, bool active, object state = null)
        {
            Key = key;
            Title = title;
            Index = index;
            Icon = icon;
            Active = active;
            State = state;
        }
    }
}
