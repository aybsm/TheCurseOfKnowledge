using DevExpress.Utils.MVVM;
using DevExpress.Utils.MVVM.Services;
using DevExpress.Utils.Svg;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheCurseOfKnowledge.Desktop.DevEx.Attributes;
using TheCurseOfKnowledge.Desktop.DevEx.Extensions;
using TheCurseOfKnowledge.Desktop.DevEx.ViewModels;

namespace TheCurseOfKnowledge.Desktop.DevEx
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        readonly IServiceProvider _services;
        //readonly MVVMContext _mvvm;
        public MainForm(IServiceProvider service)
        {
            _services = service;
            InitializeComponent();

            MVVMContext.RegisterFlyoutMessageBoxService();
            var _mvvm = new MVVMContext();
            _mvvm.ContainerControl = this;
            _mvvm.ViewModelType = typeof(MainViewModel);
            //if (!_mvvm.IsDesignMode)
            //    _mvvm.SetViewModel(typeof(MainViewModel), null);
            //_mvvm.RegisterService(DocumentManagerService.Create(tabbedView));

            var fluent = _mvvm.OfType<MainViewModel>();
            fluent.WithEvent(this, nameof(this.Load))
                .EventToCommand(z0 => z0.InitializeAsync);
            fluent.SetTrigger(vm => vm.NavigationMenus, BindNavigationMenus());

            fluent.WithEvent<SelectedElementChangedEventArgs>(accordionControl, "SelectedElementChanged")
                .EventToCommand(z0 => z0.EventTriggerHandlerAsync, HandlerAccordionControlSelectedElementChanged());

            fluent.WithEvent<ItemClickEventArgs>(foraddiconBarButtonItem, "ItemClick")
                .EventToCommand(z0 => z0.EventTriggerHandlerAsync, HandlerBarButtonNavigationItemClick());

            fluent.WithEvent<DocumentEventArgs>(tabbedView, "DocumentAdded")
                .EventToCommand(z0 => z0.EventTriggerHandler, HandlerTabbedViewDocumentAdded());

            fluent.WithEvent<DocumentEventArgs>(tabbedView, "DocumentClosed")
                .EventToCommand(z0 => z0.EventTriggerHandlerAsync, HandlerTabbedViewDocumentClosed());
        }
        private Action<BindingList<ViewControlAttribute>> BindNavigationMenus()
            => new Action<BindingList<ViewControlAttribute>>((menus)
                =>
            {
                barSubItemNavigation.ItemLinks.Clear();
                mainAccordionGroup.Elements.Clear();
                foreach (var menu in menus.Where(wh => wh.Active).OrderBy(ob => ob.Index))
                {
                    var new_menu = mainAccordionGroup.Elements.Add();
                    new_menu.Text = menu.Title;
                    new_menu.ImageOptions.SvgImage = (SvgImage)global::TheCurseOfKnowledge.Desktop.DevEx.Properties.Resources.ResourceManager.GetObject(menu.Icon);
                    new_menu.Style = ElementStyle.Item;
                    new_menu.Tag = menu.Key;
                }
            });
        private Func<SelectedElementChangedEventArgs, Func<Task>> HandlerAccordionControlSelectedElementChanged()
            => new Func<SelectedElementChangedEventArgs, Func<Task>>((e)
                => new Func<Task>(() =>
                {
                    if (e.Element == null) return Task.CompletedTask;
                    var docs = tabbedView.Documents.FirstOrDefault(first => first.Caption == e.Element.Text);
                    if (docs == null)
                    {
                        var usercontrol = _services.GetRequiredKeyedService<XtraUserControl>((string)e.Element.Tag);
                        usercontrol.Name = $"{e.Element.Tag}_xtrausercontrol";
                        usercontrol.Text = e.Element.Text;
                        usercontrol.Dock = DockStyle.Fill;
                        docs = tabbedView.AddDocument(usercontrol);
                    }
                    tabbedView.ActivateDocument(docs.Control);
                    return Task.CompletedTask;
                }));
        private Func<ItemClickEventArgs, Func<Task>> HandlerBarButtonNavigationItemClick()
            => new Func<ItemClickEventArgs, Func<Task>>((e)
                => new Func<Task>(() =>
                {
                    accordionControl.SelectedElement = mainAccordionGroup.Elements.FirstOrDefault(first => first.Text == e.Item.Caption);
                    return Task.CompletedTask;
                }));
        private Func<DocumentEventArgs, Action> HandlerTabbedViewDocumentAdded()
            => new Func<DocumentEventArgs, Action>((e)
                => new Action(() =>
                {
                    if (barSubItemNavigation.ItemLinks.Any(any => any.Caption == e.Document.Caption))
                        return;
                    var new_button_link = new BarButtonItem() { Caption = e.Document.Caption, };
                    new_button_link.ImageOptions.SvgImage = mainAccordionGroup.Elements
                       .FirstOrDefault(first => first.Text == e.Document.Caption)?
                               .ImageOptions.SvgImage;
                    new_button_link.ItemClick += new ItemClickEventHandler(async (sender, ice)
                        => await HandlerBarButtonNavigationItemClick().Invoke(ice).Invoke());
                    barSubItemNavigation.AddItem(new_button_link);
                }));
        private Func<DocumentEventArgs, Func<Task>> HandlerTabbedViewDocumentClosed()
            => new Func<DocumentEventArgs, Func<Task>>((e)
                => new Func<Task>(() =>
                {
                    accordionControl.SelectedElement = tabbedView.Documents.Any()
                        ? mainAccordionGroup.Elements.FirstOrDefault(first => first.Text == tabbedView.ActiveDocument?.Caption)
                        : null;
                    var barlink = barSubItemNavigation.ItemLinks.FirstOrDefault(first => first.Caption == e.Document.Caption);
                    if (barlink != null)
                        barSubItemNavigation.RemoveLink(barlink);
                    return Task.CompletedTask;
                }));
    }
}