using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace caso2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Deshabilitar el botón mientras se realiza la consulta
            button1.Enabled = false;

            // Crear un hilo para ejecutar la consulta
            Thread Thread = new Thread(Consulta);
            Thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Deshabilitar el botón mientras se realiza la consulta
            button2.Enabled = false;

            // Crear un hilo para ejecutar la consulta
            Thread Thread2 = new Thread(Diferencia);
            Thread2.Start();
        }

        private void Diferencia()
        {
            string connection = $"Data Source=DESKTOP-F8D58TD\\SQLEXPRESS; Initial Catalog=AdventureWorks2012; Integrated security=true;";
            SqlConnection conexion = new SqlConnection(connection);
            try
            {
                conexion.Open();
                string consulta = "SELECT BusinessEntityID, SUM(LastReceiptCost - StandardPrice)" +
                    "AS DiferenciaTotal FROM Purchasing.ProductVendor GROUP BY BusinessEntityID;";
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                DataTable dataTab = new DataTable();

                // Agregar una columna de contador para saber el numero de renglones
                dataTab.Columns.Add("No.Renglon", typeof(int));
                adap.Fill(dataTab);
                for (int i = 0; i < dataTab.Rows.Count; i++)
                {
                    dataTab.Rows[i]["No.Renglon"] = i + 1;
                }

                // Reorganizar las columnas para colocar "No.Renglon" al principio
                dataTab.Columns["No.Renglon"].SetOrdinal(0);

                // Asignar los resultados al DataGridView
                dataGridView2.Invoke(new Action(() => dataGridView2.DataSource = dataTab));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Habilitar el botón después de completar la consulta
                button2.Invoke(new Action(() => button2.Enabled = true));

                conexion.Close();
            }
        }
        private void Consulta()
        {
            string connection = 
                $"Data Source=DESKTOP-F8D58TD\\SQLEXPRESS; Initial Catalog=AdventureWorks2012; Integrated security=true;";
            SqlConnection conexion = 
                new SqlConnection(connection);
            try
            {
               
                    conexion.Open();
                    string select = "SELECT v.BusinessEntityID, v.Name AS NombreVendor, " +
                                     "p.Name AS NombreProducto, p.Color, pv.StandardPrice, pv.LastReceiptCost, " +
                                     "um.Name AS NombreUnitMeasure " +
                                     "FROM Purchasing.Vendor AS v INNER JOIN " +
                                     "Purchasing.ProductVendor AS pv ON " +
                                     "v.BusinessEntityID = pv.BusinessEntityID INNER JOIN " +
                                     "Production.Product AS p ON " +
                                     "pv.ProductID = p.ProductID INNER JOIN " +
                                     "Production.UnitMeasure AS um ON " +
                                     "pv.UnitMeasureCode = um.UnitMeasureCode " +
                                     "WHERE v.BusinessEntityID BETWEEN 1610 AND 1624;";
                    SqlCommand cmd = new SqlCommand(select, conexion);
                    SqlDataAdapter adap = new SqlDataAdapter(cmd);
                    DataTable dataTab = new DataTable();
                    adap.Fill(dataTab);
                    dataTab.Columns.Add("No.Renglon", typeof(int));
                    for (int i = 0; i < dataTab.Rows.Count; i++)
                    {
                        dataTab.Rows[i]["No.Renglon"] = i + 1;
                    }
                    dataTab.Columns["No.Renglon"].SetOrdinal(0);


                    dataGridView1.Invoke(new Action(() => dataGridView1.DataSource = dataTab));

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    
                    button1.Invoke(new Action(() => button1.Enabled = true));

                    conexion.Close();
                }


            }
        
        


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}




