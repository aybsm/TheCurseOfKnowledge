using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TheCurseOfKnowledge.Desktop.DevEx.Attributes;
using TheCurseOfKnowledge.Desktop.DevEx.Extensions;

namespace TheCurseOfKnowledge.Desktop.DevEx.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
        }

        public virtual BindingList<ViewControlAttribute> NavigationMenus { get; protected set; }

        public void EventTriggerHandler(Action action)
            => action();
        public async void EventTriggerHandlerAsync(Func<Task> action)
            => await action();
        public async Task InitializeAsync()
        {
            var menus = Assembly.GetExecutingAssembly()
                .GetViewControlAttribute()
                .ToList();
            NavigationMenus = new BindingList<ViewControlAttribute>(menus);
            await Task.CompletedTask;
        }
    }
}
