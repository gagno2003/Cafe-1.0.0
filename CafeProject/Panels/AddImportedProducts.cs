using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using Bunifu.Framework.UI;

namespace CafeProject
{
    public partial class AddImportedProducts : UserControl
    {
        public AddImportedProducts()
        {
            InitializeComponent();
        }
        CMessageBox cMessageBox = new CMessageBox();

        static public BindingList<ProductsItem> ImportedProducts = new BindingList<ProductsItem>();

       
        private void AddImportedProducts_Load(object sender, EventArgs e)
        {
            
            
            ProductsGridView.DataSource = ImportedProducts;
            KgLBox.AddItem("კგ");
            KgLBox.AddItem("ლ");
        }

        private void AddBTN_Click(object sender, EventArgs e)
        {
            string name = NameBox?.Text;
            double price = new double();
            
            double weight = new double();
            int count = new int();

            try
            {
                price = double.Parse(PriceBox.Text);
                weight = double.Parse(WeightBox.Text);
                count = int.Parse(CountBox.Text);
            }
            catch (Exception)
            {
                cMessageBox.Show("პროდუქტი ვერ დაემატა!");
            }
           


            if (CountBox.Text != "")
            {
                if (name != null && price != 0 && weight != 0 && count != 0)
                {
                    ImportedProducts.Add(new ProductsItem(name, weight, price, count));
                    cMessageBox.Show("პროდუქტი დაემატა!");
                    NameBox.Text = "";
                    WeightBox.Text = "";
                    PriceBox.Text = "";
                    CountBox.Text = "";
                    ProductsGridView.Refresh();
                }
            }
            else
            {
                if (name != null && price != 0 && weight != 0)
                {
                    ImportedProducts.Add(new ProductsItem(name, weight, price));
                    cMessageBox.Show("პროდუქტი დაემატა!");
                    NameBox.Text = "";
                    WeightBox.Text = "";
                    PriceBox.Text = "";
                    CountBox.Text = "";
                    ProductsGridView.Refresh();
                }
                
            }

            

            ProductsGridView.Refresh();
        }

        static public int plus = 0;
        private void SaveBTN_Click(object sender, EventArgs e)
        {
            string savetext = "";
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path" })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    savetext = fbd.SelectedPath;
                }
            }

            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = false;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "ImportedProducts";

            for (int i = 1; i < ProductsGridView.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = ProductsGridView.Columns[i - 1].HeaderText;
            }

            for (int i = 0; i < ProductsGridView.Rows.Count; i++)
            {
                for (int j = 0; j < ProductsGridView.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = ProductsGridView.Rows[i].Cells[j].Value.ToString();
                }
            }

            plus++;

            FileStream sb = new FileStream("ImportedPlusNumber.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(sb);
            sw.Write(plus);
            sw.Close();

            using (StreamReader st = new StreamReader("ImportedPlusNumber.txt"))
            {
                string text = st.ReadToEnd();
                var result = int.Parse(text);
                plus = result;
            }

            string excelname = (savetext + $"/ImportedProducts{plus}.xlsx");
            try
            {
                workbook.SaveAs(excelname, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception)
            {
                cMessageBox.Show("ეს ფაილი უკვე არსებობს!");
            }

            workbook.Close();
        }

        static public int clicknumb = 0;

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            clicknumb++;
            if (clicknumb % 2 != 0)
            {
                MainWindow.form.Width = 463;
                MainWindow.form.Height = 689;
            }
            else
            {
                MainWindow.form.Width = 463;
                MainWindow.form.Height = 480;
            }
            
        }

       

        private void RemoveClick_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell item in ProductsGridView.SelectedCells)
            {
                try
                {
                    ImportedProducts.RemoveAt(item.RowIndex);
                }
                catch (Exception)
                {

                }
            }
        }

        double sum = 0;
        private void ClearBTN_Click(object sender, EventArgs e)
        {
            foreach (var item in ImportedProducts)
            {
                sum += item.Price;
            }

            cMessageBox.Show($"ანგარიში: {sum}₾");
            sum = 0;
        }

        private void BackBTN_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormsList.forms["ManagerWindow"].Show();
            MainWindow.form.Width = 282;
            MainWindow.form.Height = 459;
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddBTN_Click(null, null);
            }
        }

        private void PriceBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddBTN_Click(null, null);
            }
        }

        private void CountBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddBTN_Click(null, null);
            }
        }

        private void WeightBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddBTN_Click(null, null);
            }
        }
    }
}
