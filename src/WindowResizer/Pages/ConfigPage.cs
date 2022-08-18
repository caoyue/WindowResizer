using System.Drawing;
using System.Windows.Forms;
using WindowResizer.Configuration;

// ReSharper disable once CheckNamespace
namespace WindowResizer
{
    public partial class SettingForm
    {
        private void ConfigPageInit()
        {
            WindowsGrid.AllowUserToAddRows = false;
            WindowsGrid.RowTemplate.Height = 50;
            WindowsGrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            WindowsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            WindowsGrid.Columns.Clear();
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "ExeName",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    ForeColor = Color.Blue
                },
                FillWeight = 15,
                DisplayIndex = 0,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Title",
                DataPropertyName = "Title",
                HeaderText = "Title",
                Resizable = DataGridViewTriState.True,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft
                },
                FillWeight = 35,
                DisplayIndex = 1,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Top",
                DataPropertyName = "Top",
                HeaderText = "Top",
                FillWeight = 8,
                DisplayIndex = 2,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Left",
                DataPropertyName = "Left",
                HeaderText = "Left",
                FillWeight = 8,
                DisplayIndex = 3,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Right",
                DataPropertyName = "Right",
                HeaderText = "Right",
                FillWeight = 8,
                DisplayIndex = 4,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Bottom",
                DataPropertyName = "Bottom",
                HeaderText = "Bottom",
                FillWeight = 8,
                DisplayIndex = 5,
            });
            WindowsGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Rect", DataPropertyName = "Rect", Visible = false,
            });

            WindowsGrid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "AutoResize",
                DataPropertyName = "AutoResize",
                HeaderText = "Auto",
                FillWeight = 8,
                DisplayIndex = 6,
            });

            WindowsGrid.Columns.Add(new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Remove",
                Name = "Remove",
                HeaderText = "",
                FlatStyle = FlatStyle.Flat,
                DefaultCellStyle =
                {
                    ForeColor = Color.Blue
                },
                FillWeight = 10,
                DisplayIndex = 7,
            });

            foreach (DataGridViewColumn col in WindowsGrid.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font =
                    new Font("Microsoft YaHei UI", 16F, FontStyle.Bold, GraphicsUnit.Pixel);
            }

            WindowsGrid.AutoGenerateColumns = false;
            WindowsGrid.RowTemplate.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);
            WindowsGrid.DataSource = ConfigLoader.Config.WindowSizes;

            WindowsGrid.ShowCellToolTips = true;
            WindowsGrid.CellFormatting += WindowsGrid_CellFormatting;
            WindowsGrid.CellClick += WindowsGrid_CellClick;
            WindowsGrid.CellValueChanged += WindowsGrid_CellValueChanged;
        }


        private void WindowsGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ConfigLoader.Save();
        }

        private void WindowsGrid_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null &&
                (e.ColumnIndex == WindowsGrid.Columns["Name"]?.Index ||
                    e.ColumnIndex == WindowsGrid.Columns["Title"]?.Index))
            {
                DataGridViewCell cell =
                    WindowsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.ToolTipText = cell.Value.ToString();
            }
        }

        private void WindowsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == WindowsGrid.Columns["Remove"]?.Index &&
                e.RowIndex >= 0 &&
                e.RowIndex < ConfigLoader.Config.WindowSizes.Count)
            {
                ConfigLoader.Config.WindowSizes.RemoveAt(e.RowIndex);
                ConfigLoader.Save();
            }
        }
    }
}
