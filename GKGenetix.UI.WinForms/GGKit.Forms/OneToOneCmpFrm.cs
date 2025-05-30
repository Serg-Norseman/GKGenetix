/*
 * Genetic Genealogy Kit (GGK), v1.2
 * Copyright © 2014 by Felix Chandrakumar
 * License: MIT License (http://opensource.org/licenses/MIT)
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using GGKit.Core;
using GKGenetix.Core.Model;
using GKGenetix.UI;

namespace GGKit.Forms
{
    public partial class OneToOneCmpFrm : GKWidget
    {
        private readonly string kit1 = null;
        private readonly string kit2 = null;
        private IList<CmpSegment> segmentsRes;


        public static bool CanBeUsed(IList<KitDTO> selectedKits)
        {
            return (selectedKits != null && selectedKits.Count == 2 && !selectedKits[0].Disabled && !selectedKits[1].Disabled);
        }


        public OneToOneCmpFrm(IKitHost host, IList<KitDTO> selectedKits) : this(host, selectedKits[0].KitNo, selectedKits[1].KitNo)
        {
        }

        public OneToOneCmpFrm(IKitHost host, string kit1, string kit2) : base(host)
        {
            InitializeComponent();

            UIHelper.FixGridView(dgvSegmentIdx);
            dgvSegmentIdx.AddColumn("Chromosome", "Chromosome");
            dgvSegmentIdx.AddColumn("StartPosition", "Start Position");
            dgvSegmentIdx.AddColumn("EndPosition", "End Position");
            dgvSegmentIdx.AddColumn("SegmentLength_cm", "Segment Length (cM)", "#0.00");
            dgvSegmentIdx.AddColumn("SNPCount", "SNP Count");

            this.kit1 = kit1;
            this.kit2 = kit2;
            _host.SetStatus($"Comparing kits {kit1} and {kit2} ...");

            Task.Factory.StartNew(() => {
                segmentsRes = GKGenFuncs.CompareOneToOne(kit1, kit2, null, false, false);

                this.Invoke(new MethodInvoker(delegate {
                    UpdateView();
                }));
            });
        }

        private void UpdateView()
        {
            dgvSegmentIdx.DataSource = segmentsRes;

            var segmentStats = SegmentStats.CalculateSegmentStats(segmentsRes);
            lblTotalSegments.Text = $"{segmentStats.Total:#0.00} cM";
            lblTotalXSegments.Text = $"{segmentStats.XTotal:#0.00} cM";
            lblLongestSegment.Text = $"{segmentStats.Longest:#0.00} cM";
            lblLongestXSegment.Text = $"{segmentStats.XLongest:#0.00} cM";
            lblMRCA.Text = segmentStats.GetMRCAText(false);

            _host.SetStatus("Done.");
        }

        private void dgvSegmentIdx_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var selRow = dgvSegmentIdx.GetSelectedObj<CmpSegment>();
            var phased = GKSqlFuncs.IsPhased(kit1) || GKSqlFuncs.IsPhased(kit2);
            if (phased && selRow != null) {
                _host.ShowPhasedSegmentVisualizer(kit1, kit2, selRow.Chromosome, selRow.StartPosition, selRow.EndPosition);
            }
        }
    }
}
