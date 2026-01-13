/*
 *  GKGenetix, the simple DNA analysis kit.
 *  Copyright (C) 2022-2026 by Sergey V. Zhdanovskih.
 *
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;

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

        public static void AddColumn(this GridView dataGridView, string propertyName, string headerText, string format = "", bool visible = true, bool readOnly = true)
        {
            var cell = new TextBoxCell(propertyName);

            if (!string.IsNullOrEmpty(format))
                cell.Binding = Binding.Property<double>(propertyName).Convert(d => d.ToString(format));

            var column = new GridColumn() { HeaderText = headerText };
            column.DataCell = cell;
            column.Visible = visible;
            column.Editable = !readOnly;
            column.AutoSize = true;
            dataGridView.Columns.Add(column);
        }

        public static void AddCheckedColumn(this GridView dataGridView, string propertyName, string headerText, bool readOnly = true)
        {
            var cell = new CheckBoxCell(propertyName);

            var column = new GridColumn() { HeaderText = headerText };
            column.DataCell = cell;
            column.Editable = !readOnly;
            column.AutoSize = true;
            dataGridView.Columns.Add(column);
        }

        public static void AddComboColumn(this GridView dataGridView, string propertyName, string headerText, object[] items)
        {
            var cell = new ComboBoxCell(propertyName);
            cell.DataStore = items;

            var column = new GridColumn() { HeaderText = headerText };
            column.DataCell = cell;
            column.AutoSize = true;
            dataGridView.Columns.Add(column);
        }

        public static void AddButtonColumn(this GridView dataGridView, string propertyName, string headerText)
        {
            //var cell = new ButtonCell(propertyName);
            var cell = new TextBoxCell(propertyName);

            var column = new GridColumn() { HeaderText = headerText };
            column.DataCell = cell;
            column.AutoSize = true;
            dataGridView.Columns.Add(column);
        }

        public static void FixGridView(GridView dataGridView)
        {
            dataGridView.GridLines = GridLines.Both;
            dataGridView.AllowMultipleSelection = false;
        }

        /*public static DataGridViewRow[] GetArray(this DataGridViewRowCollection collection)
        {
            var result = new DataGridViewRow[collection.Count];
            collection.CopyTo(result, 0);
            return result;
        }*/

        public static T GetSelectedObj<T>(this GridView dataGridView) where T : class
        {
            var selItems = dataGridView.SelectedItems.ToList();
            return (selItems.Count > 0) ? selItems[0] as T : null;
        }

        public static TreeNode FindByTag(this TreeView treeView, TreeNode rootNode, object tag)
        {
            foreach (TreeNode node in rootNode.Children) {
                if (node.Tag.Equals(tag)) return node;
                TreeNode next = FindByTag(treeView, node, tag);
                if (next != null) return next;
            }
            return null;
        }

        public static void InitCommonStyles()
        {
            Eto.Style.Add<TableLayout>("paddedTable", table => {
                table.Padding = new Padding(8);
                table.Spacing = new Size(4, 4);
            });

            Eto.Style.Add<TableLayout>("paddedTable8", table => {
                table.Padding = new Padding(8);
                table.Spacing = new Size(8, 8);
            });

            Eto.Style.Add<StackLayout>("vertListStack", stack => {
                stack.Orientation = Orientation.Vertical;
                stack.Padding = new Padding(8);
                stack.Spacing = 4;
            });

            Eto.Style.Add<StackLayout>("horzListStack", stack => {
                stack.Orientation = Orientation.Horizontal;
                stack.Padding = new Padding(8);
                stack.Spacing = 4;
            });

            Eto.Style.Add<StackLayout>("dlgFooter", stack => {
                stack.Orientation = Orientation.Horizontal;
                stack.Padding = new Padding(0);
                stack.Spacing = 8;
            });

            Eto.Style.Add<Button>("dlgBtn", button => {
                button.ImagePosition = ButtonImagePosition.Left;
                button.Size = new Size(120, 26);
            });
        }
    }
}
