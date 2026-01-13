/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace GKGenetix.UI
{
    public static class UIHelper
    {
        public static Color HeatMapColor(double percent, double max)
        {
            double val = percent * 255 / max;

            int r = 255;
            int g = Convert.ToByte(val);
            int b = Convert.ToByte(val);
            return Color.FromArgb(255, r, g, b);
        }

        public static void AddColumn(this DataGridView dataGridView, string propertyName, string headerText, string format = "", bool visible = true, bool readOnly = true)
        {
            var column = new DataGridViewTextBoxColumn() { DataPropertyName = propertyName, HeaderText = headerText };
            if (!string.IsNullOrEmpty(format)) column.DefaultCellStyle.Format = format;
            column.Visible = visible;
            column.ReadOnly = readOnly;
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns.Add(column);
        }

        public static void AddCheckedColumn(this DataGridView dataGridView, string propertyName, string headerText)
        {
            var column = new DataGridViewCheckBoxColumn() { DataPropertyName = propertyName, HeaderText = headerText };
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns.Add(column);
        }

        public static void AddComboColumn(this DataGridView dataGridView, string propertyName, string headerText, object[] items)
        {
            var column = new DataGridViewComboBoxColumn() { DataPropertyName = propertyName, HeaderText = headerText };
            column.Items.AddRange(items);
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns.Add(column);
        }

        public static void AddButtonColumn(this DataGridView dataGridView, string propertyName, string headerText)
        {
            var column = new DataGridViewButtonColumn() { DataPropertyName = propertyName, HeaderText = headerText };
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.Columns.Add(column);
        }

        public static void FixGridView(DataGridView dataGridView)
        {
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView.ReadOnly = true;
            dataGridView.ShowEditingIcon = false;

            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.MultiSelect = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public static DataGridViewRow[] GetArray(this DataGridViewRowCollection collection)
        {
            var result = new DataGridViewRow[collection.Count];
            collection.CopyTo(result, 0);
            return result;
        }

        public static T GetSelectedObj<T>(this DataGridView dataGridView) where T : class
        {
            return (dataGridView.SelectedRows.Count > 0) ? dataGridView.SelectedRows[0].DataBoundItem as T : null;
        }

        public static TreeNode FindByTag(this TreeView treeView, TreeNode rootNode, object tag)
        {
            foreach (TreeNode node in rootNode.Nodes) {
                if (node.Tag.Equals(tag)) return node;
                TreeNode next = FindByTag(treeView, node, tag);
                if (next != null) return next;
            }
            return null;
        }
    }
}
