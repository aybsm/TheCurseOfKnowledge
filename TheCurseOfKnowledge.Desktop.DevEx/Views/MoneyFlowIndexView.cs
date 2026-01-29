using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheCurseOfKnowledge.Desktop.DevEx.Attributes;

namespace TheCurseOfKnowledge.Desktop.DevEx.Views
{
    [ViewControl(
        key: "money-flow-index",
        title: "Money Flow Index",
        index: 3,
        icon: nameof(global::TheCurseOfKnowledge.Desktop.DevEx.Properties.Resources.charttype_swiftplot),
        active: true)]
    public partial class MoneyFlowIndexView : DevExpress.XtraEditors.XtraUserControl
    {
        public MoneyFlowIndexView()
        {
            InitializeComponent();
        }
    }
}
