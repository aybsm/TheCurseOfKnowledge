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
        key: "relative-strength-index",
        title: "Relative Strength Index",
        index: 1,
        icon: nameof(global::TheCurseOfKnowledge.Desktop.DevEx.Properties.Resources.charttype_boxplot),
        active: true)]
    public partial class RelativeStrengthIndexView : DevExpress.XtraEditors.XtraUserControl
    {
        public RelativeStrengthIndexView()
        {
            InitializeComponent();
        }
    }
}
