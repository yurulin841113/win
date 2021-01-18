using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string str = "data source = DESK210105PC\\SQLEXPRESS; initial catalog = student; integrated security = True;";
        DataSet ds = new DataSet();
        DataTable myTable = new DataTable();
        public MainWindow()
        {
            InitializeComponent();
            


            using (SqlConnection conn = new SqlConnection(str)) {

                SqlCommand cmd = new SqlCommand("select_table1", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                myTable = ds.Tables[0];
                grid1.ItemsSource = myTable.DefaultView;


            }
            
        }

      

        private void grid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            textbox1.Text = "";

            grid2.IsEnabled = false;

            foreach (DataRowView row in grid1.SelectedItems)
            {
                System.Data.DataRow MyRow = row.Row;
                textbox1.Text = MyRow["代碼"].ToString();
                //MessageBox.Show(value); <- To check if it works...
            }


            using (SqlConnection conn = new SqlConnection(str))
            {

                SqlCommand cmd = new SqlCommand("select_table1", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@代碼", textbox1.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                myTable = ds.Tables[1];
                grid2.ItemsSource = myTable.DefaultView;

               

              
            }


           

        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            grid2.IsEnabled = true;
         
            grid2.CanUserAddRows = false;

            if (btn1.Content as string == "修改")
            {

              

                btn1.Content = "儲存";
                




            }
            else
            {  
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();

                    for (int i = 0;i < myTable.Rows.Count;i++)
                    { 
                        SqlCommand cmd = new SqlCommand("update_table1", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@代碼", textbox1.Text);
                        cmd.Parameters.AddWithValue("@板厚", Convert.ToInt32(myTable.Rows[i]["板厚"].ToString()));
                        cmd.Parameters.AddWithValue("@實際尺寸", Convert.ToDecimal(myTable.Rows[i]["實際尺寸"].ToString()));
                        cmd.Parameters.AddWithValue("@拆料尺寸", Convert.ToDecimal(myTable.Rows[i]["拆料尺寸"].ToString()));
                        cmd.ExecuteNonQuery();
                       
                    }

                    
                    int s1 = Convert.ToInt32(myTable.Rows[0]["板厚"].ToString()); 
                    int s2 = Convert.ToInt32(myTable.Rows[1]["板厚"].ToString());
                    int s3 = Convert.ToInt32(myTable.Rows[2]["板厚"].ToString());
                    Console.WriteLine(s1 + "\t" + s2 + "\t" + s3);


                }
                btn1.Content = "修改";
                grid2.IsEnabled = false;
            }
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {


            int s1 = 0;
            int s2 = 0;
            int s3 = 0;

            for (int i = 0; i < myTable.Rows.Count; i++)
            {
                

                s1 = Convert.ToInt32(Convert.ToInt32(myTable.Rows[i]["板厚"].ToString()));
                s2 = Convert.ToInt32(Convert.ToInt32(myTable.Rows[i]["實際尺寸"].ToString()));
                s3 = Convert.ToInt32(Convert.ToInt32(myTable.Rows[i]["拆料尺寸"].ToString()));
                
                Console.WriteLine(s1 +"\t" + s2 +"\t"+s3);
            }

        



            //string s2 = (grid2.Columns[0].GetCellContent(grid2.Items[1]) as TextBlock).Text;
            //string 實際尺寸2 = (grid2.Columns[1].GetCellContent(grid2.Items[1]) as TextBlock).Text;
            //string 拆料尺寸2 = (grid2.Columns[2].GetCellContent(grid2.Items[1]) as TextBlock).Text;

            //string s3 = (grid2.Columns[0].GetCellContent(grid2.Items[2]) as TextBlock).Text;
            // string 實際尺寸3 = (grid2.Columns[1].GetCellContent(grid2.Items[2]) as TextBlock).Text;
            // string 拆料尺寸3 = (grid2.Columns[2].GetCellContent(grid2.Items[2]) as TextBlock).Text;


            // Console.WriteLine(s +"\t"+實際尺寸+"\t"+拆料尺寸);
            // Console.WriteLine(s2 + "\t" + 實際尺寸2 + "\t" + 拆料尺寸2);
            // Console.WriteLine(s3 + "\t" + 實際尺寸3 + "\t" + 拆料尺寸3);
        }

      
    }
}
