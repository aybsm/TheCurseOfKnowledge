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
        key: "bollinger-bands",
        title: "Bollinger Bands",
        index: 4,
        icon: nameof(global::TheCurseOfKnowledge.Desktop.DevEx.Properties.Resources.charttype_splineareastacked),
        active: true)]
    public partial class BollingerBandsView : DevExpress.XtraEditors.XtraUserControl
    {
        public BollingerBandsView()
        {
            InitializeComponent();
        }
    }
}
