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
                .EventToCommand(z0 => z0.EventTriggerHandler, HandlerBarButtonNavigationItemClick());

            fluent.WithEvent<DocumentEventArgs>(tabbedView, "DocumentAdded")
                .EventToCommand(z0 => z0.EventTriggerHandler, HandlerTabbedViewDocumentAdded());

            fluent.WithEvent<DocumentCancelEventArgs>(tabbedView, "DocumentClosing")
                .EventToCommand(z0 => z0.EventTriggerHandler, HandlerTabbedViewDocumentClosing());

            fluent.WithEvent<DocumentEventArgs>(tabbedView, "DocumentClosed")
                .EventToCommand(z0 => z0.EventTriggerHandler, HandlerTabbedViewDocumentClosed());
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
                => new Func<Task>(()
                    =>
                {
                    if (e.Element == null) return Task.CompletedTask;
                    var docs = tabbedView.Documents.FirstOrDefault(first => $"{first.Control.Tag}" == $"{e.Element.Tag}");
                    if (docs == null)
                    {
                        var usercontrol = _services.GetRequiredKeyedService<XtraUserControl>($"{e.Element.Tag}");
                        usercontrol.Name = $"{e.Element.Tag}_xtrausercontrol";
                        usercontrol.Text = e.Element.Text;
                        usercontrol.Dock = DockStyle.Fill;
                        usercontrol.Tag = $"{e.Element.Tag}";
                        docs = tabbedView.AddDocument(usercontrol);
                        docs.Tag = $"{e.Element.Tag}";
                    }
                    tabbedView.ActivateDocument(docs.Control);
                    return Task.CompletedTask;
                }));
        private Func<ItemClickEventArgs, Action> HandlerBarButtonNavigationItemClick()
            => new Func<ItemClickEventArgs, Action>((e)
                => new Action(() => accordionControl.SelectedElement = mainAccordionGroup.Elements.FirstOrDefault(first => $"{first.Tag}" == $"{e.Item.Tag}")));
        private Func<DocumentEventArgs, Action> HandlerTabbedViewDocumentAdded()
            => new Func<DocumentEventArgs, Action>((e)
                => new Action(()
                    =>
                {
                    if (barSubItemNavigation.ItemLinks.Any(any => $"{any.Item.Tag}" == $"{e.Document.Control.Tag}"))
                        return;
                    var new_button_link = new BarButtonItem() { Caption = e.Document.Caption, Tag = $"{e.Document.Control.Tag}", };
                    new_button_link.ImageOptions.SvgImage = mainAccordionGroup.Elements
                       .FirstOrDefault(first => $"{first.Tag}" == $"{e.Document.Control.Tag}")?
                       .ImageOptions.SvgImage;
                    new_button_link.ItemClick += new ItemClickEventHandler((sender, ice)
                        => HandlerBarButtonNavigationItemClick().Invoke(ice).Invoke());
                    barSubItemNavigation.AddItem(new_button_link);
                }));
        private Func<DocumentEventArgs, Action> HandlerTabbedViewDocumentClosed()
            => new Func<DocumentEventArgs, Action>((e)
                => new Action(()
                    => accordionControl.SelectedElement = tabbedView.Documents.Any()
                        ? mainAccordionGroup.Elements.FirstOrDefault(first => $"{first.Tag}" == $"{tabbedView.ActiveDocument?.Control?.Tag}")
                        : null
                ));
        private Func<DocumentCancelEventArgs, Action> HandlerTabbedViewDocumentClosing()
            => new Func<DocumentCancelEventArgs, Action>((e)
                => new Action(()
                    => barSubItemNavigation.RemoveLink(barSubItemNavigation.ItemLinks.FirstOrDefault(first => $"{first.Item.Tag}" == $"{e.Document?.Control?.Tag}"))));
    }
}