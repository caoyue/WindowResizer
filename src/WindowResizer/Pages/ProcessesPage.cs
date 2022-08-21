using System.Drawing;
using System.Windows.Forms;
using WindowResizer.Configuration;
using WindowResizer.Utils;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private void ProcessesPageInit()
        {
            ProcessesGrid.AllowUserToAddRows = false;
            ProcessesGrid.RowTemplate.Height = 50;
            ProcessesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ProcessesGrid.Columns.Clear();
            ProcessesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Process",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    ForeColor = SystemColors.Highlight,
                    SelectionForeColor = SystemColors.Highlight,
                    SelectionBackColor = SystemColors.Window,
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                },
                FillWeight = 15,
                DisplayIndex = 0,
            });
            ProcessesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                DataPropertyName = "Title",
                HeaderText = "Title",
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                },
                FillWeight = 35,
                DisplayIndex = 1,
            });
            ProcessesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Top",
                DataPropertyName = "Top",
                HeaderText = "Top",
                FillWeight = 8,
                DisplayIndex = 2,
            });
            ProcessesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Left",
                DataPropertyName = "Left",
                HeaderText = "Left",
                FillWeight = 8,
                DisplayIndex = 3,
            });
            ProcessesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Right",
                DataPropertyName = "Right",
                HeaderText = "Right",
                FillWeight = 8,
                DisplayIndex = 4,
            });
            ProcessesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Bottom",
                DataPropertyName = "Bottom",
                HeaderText = "Bottom",
                FillWeight = 8,
                DisplayIndex = 5,
            });
            ProcessesGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Rect", DataPropertyName = "Rect", Visible = false,
            });

            ProcessesGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "AutoResize",
                DataPropertyName = "AutoResize",
                HeaderText = "Auto",
                FillWeight = 8,
                DisplayIndex = 6,
                FlatStyle = FlatStyle.Standard,
                DefaultCellStyle =
                {
                    ForeColor = SystemColors.Highlight, SelectionBackColor = SystemColors.Window,
                },
            });

            ProcessesGrid.Columns.Add(new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Remove",
                Name = "Remove",
                HeaderText = "",
                FlatStyle = FlatStyle.System,
                DefaultCellStyle =
                {
                    SelectionBackColor = SystemColors.Window, Padding = new Padding(5)
                },
                FillWeight = 10,
                DisplayIndex = 7,
            });

            foreach (DataGridViewColumn col in ProcessesGrid.Columns)
            {
                if (!col.Name.Equals("Name") && !col.Name.Equals("Title"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = Helper.ChangeFontSize(ProcessesGrid.Font, 9F, FontStyle.Bold);
            }

            ProcessesGrid.AutoGenerateColumns = false;
            ProcessesGrid.DataSource = ConfigLoader.Config.WindowSizes;

            ProcessesGrid.ShowCellToolTips = true;
            ProcessesGrid.CellFormatting += PrecessesGrid_CellFormatting;
            ProcessesGrid.CellClick += PrecessesGrid_CellClick;
            ProcessesGrid.CellContentClick += PrecessesGrid_CellContentClick;
            ProcessesGrid.CellValueChanged += PrecessesGrid_CellValueChanged;
            ProcessesGrid.CellMouseEnter += PrecessesGrid_CellMouseEnter;
        }

        private void PrecessesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ProcessesGrid.Columns["AutoResize"]?.Index && e.RowIndex >= 0)
            {
                ProcessesGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void PrecessesGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ConfigLoader.Save();
        }

        private void PrecessesGrid_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == ProcessesGrid.Columns["Name"]?.Index ||
                    e.ColumnIndex == ProcessesGrid.Columns["Title"]?.Index) && e.Value != null && e.Value.ToString().Length > 20)
            {
                DataGridViewCell cell = ProcessesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = cell.Value.ToString();
            }
        }

        private void PrecessesGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            string name = ProcessesGrid.Columns[e.ColumnIndex].Name;
            switch (name)
            {
                case "Name":
                    ProcessesGrid.Cursor = Cursors.Default;
                    break;

                case "AutoResize":
                case "Remove":
                    ProcessesGrid.Cursor = Cursors.Hand;
                    break;

                default:
                    ProcessesGrid.Cursor = Cursors.IBeam;
                    break;
            }
        }

        private void PrecessesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ProcessesGrid.Columns["Remove"]?.Index &&
                e.RowIndex >= 0 &&
                e.RowIndex < ConfigLoader.Config.WindowSizes.Count)
            {
                ConfigLoader.Config.WindowSizes.RemoveAt(e.RowIndex);
                ConfigLoader.Save();
            }
        }
    }
}
