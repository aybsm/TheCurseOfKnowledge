using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TheCurseOfKnowledge.Desktop.DevEx.Attributes;

namespace TheCurseOfKnowledge.Desktop.DevEx.Extensions
{
    public static class AssemblyExtension
    {
        const string targetNamespace = "TheCurseOfKnowledge.Desktop.DevEx.Views";
        public static IEnumerable<ViewControlAttribute> GetViewControlAttribute(this Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(targetNamespace))
                .Where(t => typeof(XtraUserControl).IsAssignableFrom(t))
                .Select(t => new
                {
                    att = t.GetCustomAttribute<ViewControlAttribute>(),
                    typ = t,
                })
                .Where(t => t.att != default(ViewControlAttribute))
                .ToList();
            types.ForEach(each => each.att.State = each.typ);
            return types.Select(se => se.att);
        }
    }
}
