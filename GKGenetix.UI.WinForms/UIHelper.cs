﻿/*
 *  "GKGenetix", the simple DNA analysis kit.
 *  Copyright (C) 2022-2025 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GKGenetix".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
