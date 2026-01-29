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
        key: "moving-average", 
        title: "Moving Average", 
        index: 0, 
        icon: nameof(global::TheCurseOfKnowledge.Desktop.DevEx.Properties.Resources.charttype_line), 
        active: true)]
    public partial class MovingAverageView : DevExpress.XtraEditors.XtraUserControl
    {
        public MovingAverageView()
        {
            InitializeComponent();
        }
    }
}
