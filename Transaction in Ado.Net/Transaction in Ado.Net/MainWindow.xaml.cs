using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Transaction_in_Ado.Net
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection;
        string connectionString = string.Empty;
        DataSet dataset;
        SqlDataAdapter sqlDataAdapter;
        DataViewManager dataviewManager;
        DataView dataView;
        SqlTransaction sqlTransaction1 = null;
        SqlTransaction sqlTransaction2 = null;
        string TeacherId_tc = " ";
        string StudentId_tc = " ";
        string ttcounttext = " ";
        int tcount = 300;
        string sscounttext = " ";
        int scount = 300;

        public MainWindow()
        {
            InitializeComponent();

            connection = new SqlConnection();
            connectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;


            List<string> position = new List<string>();

            position.Add("Teachers");
            position.Add("Students");


            List<string> libs = new List<string>();

            libs.Add("Sergey Maksimenko");
            libs.Add("Dmitry Chebotarev");


            selectCombokBox.ItemsSource = position;

            libscombobox.ItemsSource = libs;

        }

        public void RefreshTable()
        {

            using (connection = new SqlConnection())
            {
                cardsDatagrid.Visibility = Visibility.Visible;
                cardsDatagrid.ItemsSource = null;
                SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlDataAdapter.Fill(dataset);

                cardsDatagrid.ItemsSource = dataset.Tables[0].DefaultView;

                sqlCommandBuilder.GetUpdateCommand();
            }
        }


        public void TeacherAutoincrementFileStreamWrite()
        {
            try
            {
            using (FileStream fs = new FileStream("TeacherAutoincrement.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {

                ttcounttext = tcount.ToString();
                byte[] bytes = Encoding.Default.GetBytes(ttcounttext);
                fs.Write(bytes, 0, bytes.Length);

            }

            }
            catch (Exception)
            {

            }
        }

        public int TeacherAutoincrementFileStreamRead()
        {
            try
            {
            using (FileStream fs = new FileStream("TeacherAutoincrement.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                ttcounttext = Encoding.Default.GetString(bytes);
                tcount = int.Parse(ttcounttext);
            }

            }
            catch (Exception)
            {

            }

            return tcount;
        }



        public void StudentAutoincrementFileStreamWrite()
        {

            try
            {
            using (FileStream fs = new FileStream("StudentAutoincrement.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {

                sscounttext = scount.ToString();
                byte[] bytes = Encoding.Default.GetBytes(sscounttext);
                fs.Write(bytes, 0, bytes.Length);

            }

            }
            catch (Exception)
            {

            }
        }

        public int StudentAutoincrementFileStreamRead()
        {
            try
            {
            using (FileStream fs = new FileStream("StudentAutoincrement.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                sscounttext = Encoding.Default.GetString(bytes);
                scount = int.Parse(sscounttext);
            }

            }
            catch (Exception)
            {

          
            }

            return scount;
        }


        private async void singInButton_Click(object sender, RoutedEventArgs e)
        {

            if (selectCombokBox.SelectedIndex == 0)
            {
                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    await connection.OpenAsync();
                    sqlTransaction1 = connection.BeginTransaction();

                    try
                    {



                        SqlCommand command1 = new SqlCommand("sp_FindTeachersId", connection);
                        command1.CommandType = CommandType.StoredProcedure;

                        SqlParameter sp_parameter1 = new SqlParameter();
                        sp_parameter1.ParameterName = "@Id";
                        sp_parameter1.SqlDbType = SqlDbType.Int;
                        sp_parameter1.Direction = System.Data.ParameterDirection.Output;


                        command1.Parameters.Add(sp_parameter1);

                        SqlParameter sp_parameter2 = new SqlParameter();
                        sp_parameter2.ParameterName = "@TeacherFirstName";
                        sp_parameter2.SqlDbType = SqlDbType.NVarChar;
                        sp_parameter2.Value = firstNametxtbox.Text;

                        command1.Parameters.Add(sp_parameter2);

                        SqlParameter sp_parameter3 = new SqlParameter();
                        sp_parameter3.ParameterName = "@TeacherPassword";
                        sp_parameter3.SqlDbType = SqlDbType.NVarChar;
                        sp_parameter3.Value = passwordtxtbox.Text;

                        command1.Parameters.Add(sp_parameter3);


                        command1.Transaction = sqlTransaction1;


                        try
                        {
                            await command1.ExecuteNonQueryAsync();
                            sqlTransaction1.Commit();
                            TeacherId_tc = sp_parameter1.Value.ToString();

                            MessageBox.Show($"{TeacherId_tc} {firstNametxtbox.Text} {passwordtxtbox.Text}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message} \n Can not Sing In");

                            try
                            {
                                sqlTransaction1.Rollback();

                            }
                            catch (Exception ex2)
                            {

                                MessageBox.Show(ex2.Message);
                            }

                        }
                        finally
                        {
                            connection.Close();
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }


                }
            }


            if (selectCombokBox.SelectedIndex == 1)
            {
                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    await connection.OpenAsync();
                    sqlTransaction1 = connection.BeginTransaction();

                    try
                    {



                        SqlCommand command1 = new SqlCommand("sp_FindStudentsId", connection);
                        command1.CommandType = CommandType.StoredProcedure;

                        SqlParameter sp_parameter1 = new SqlParameter();
                        sp_parameter1.ParameterName = "@Id";
                        sp_parameter1.SqlDbType = SqlDbType.Int;
                        sp_parameter1.Direction = System.Data.ParameterDirection.Output;


                        command1.Parameters.Add(sp_parameter1);

                        SqlParameter sp_parameter2 = new SqlParameter();
                        sp_parameter2.ParameterName = "@StudentFirstName";
                        sp_parameter2.SqlDbType = SqlDbType.NVarChar;
                        sp_parameter2.Value = firstNametxtbox.Text;

                        command1.Parameters.Add(sp_parameter2);

                        SqlParameter sp_parameter3 = new SqlParameter();
                        sp_parameter3.ParameterName = "@StudentPassword";
                        sp_parameter3.SqlDbType = SqlDbType.NVarChar;
                        sp_parameter3.Value = passwordtxtbox.Text;

                        command1.Parameters.Add(sp_parameter3);


                        command1.Transaction = sqlTransaction1;


                        try
                        {
                            await command1.ExecuteNonQueryAsync();
                            sqlTransaction1.Commit();
                            StudentId_tc = sp_parameter1.Value.ToString();

                            MessageBox.Show($"{StudentId_tc} {firstNametxtbox.Text} {passwordtxtbox.Text}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message} \n Can not Sing In");

                            try
                            {
                                sqlTransaction1.Rollback();

                            }
                            catch (Exception ex2)
                            {

                                MessageBox.Show(ex2.Message);
                            }

                        }
                        finally
                        {
                            connection.Close();
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }


                }
            }


        }

        private async void takeButton_Click(object sender, RoutedEventArgs e)
        {
          

            if (selectCombokBox.SelectedIndex == 0)
            {
                tcount = TeacherAutoincrementFileStreamRead();
                tcount++;
                TeacherAutoincrementFileStreamWrite();
                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    await connection.OpenAsync();

                    sqlTransaction2 = connection.BeginTransaction();

                    try
                    {
                        coverImage.Visibility = Visibility.Collapsed;


                        SqlCommand command2 = new SqlCommand("sp_SingInTeachers", connection);
                        command2.CommandType = CommandType.StoredProcedure;


                        SqlParameter sp2_parameter1 = new SqlParameter();
                        sp2_parameter1.ParameterName = "@TeacherFirstName";
                        sp2_parameter1.SqlDbType = SqlDbType.NVarChar;
                        sp2_parameter1.Value = firstNametxtbox.Text;

                        command2.Parameters.Add(sp2_parameter1);

                        SqlParameter sp2_parameter2 = new SqlParameter();
                        sp2_parameter2.ParameterName = "@TeacherPassword";
                        sp2_parameter2.SqlDbType = SqlDbType.NVarChar;
                        sp2_parameter2.Value = passwordtxtbox.Text;

                        command2.Parameters.Add(sp2_parameter2);

                        SqlParameter sp2_parameter3 = new SqlParameter();
                        sp2_parameter3.ParameterName = "@T_CardsId";
                        sp2_parameter3.SqlDbType = SqlDbType.Int;
                        sp2_parameter3.Value = tcount;

                        command2.Parameters.Add(sp2_parameter3);

                        SqlParameter sp2_parameter4 = new SqlParameter();
                        sp2_parameter4.ParameterName = "@TeacherId_tc";
                        sp2_parameter4.SqlDbType = SqlDbType.Int;
                        sp2_parameter4.Value = Convert.ToInt32(TeacherId_tc);

                        command2.Parameters.Add(sp2_parameter4);

                        SqlParameter sp2_parameter5 = new SqlParameter();
                        sp2_parameter5.ParameterName = "@BookId";
                        sp2_parameter5.SqlDbType = SqlDbType.Int;
                        sp2_parameter5.Value = bookIdtxtbox.Text;

                        command2.Parameters.Add(sp2_parameter5);

                        SqlParameter sp2_parameter6 = new SqlParameter();
                        sp2_parameter6.ParameterName = "@DateOut";
                        sp2_parameter6.SqlDbType = SqlDbType.DateTime;
                        sp2_parameter6.Value = dateouttxtbox.Text;

                        command2.Parameters.Add(sp2_parameter6);

                        SqlParameter sp2_parameter7 = new SqlParameter();
                        sp2_parameter7.ParameterName = "@DateIn";
                        sp2_parameter7.SqlDbType = SqlDbType.DateTime;
                        sp2_parameter7.Value = dateintxtbox.Text;

                        command2.Parameters.Add(sp2_parameter7);

                        if (libscombobox.SelectedIndex == 0)
                        {
                            SqlParameter sp2_parameter8 = new SqlParameter();
                            sp2_parameter8.ParameterName = "@LibId";
                            sp2_parameter8.SqlDbType = SqlDbType.Int;
                            sp2_parameter8.Value = 1;

                            command2.Parameters.Add(sp2_parameter8);
                        }

                        if (libscombobox.SelectedIndex == 1)
                        {
                            SqlParameter sp2_parameter8 = new SqlParameter();
                            sp2_parameter8.ParameterName = "@LibId";
                            sp2_parameter8.SqlDbType = SqlDbType.Int;
                            sp2_parameter8.Value = 2;

                            command2.Parameters.Add(sp2_parameter8);
                        }


                        command2.Transaction = sqlTransaction2;
                        //command2.ExecuteNonQuery();

                        try
                        {

                            await command2.ExecuteNonQueryAsync();
                            sqlTransaction2.Commit();

                            MessageBox.Show($"{TeacherId_tc} {firstNametxtbox.Text} {passwordtxtbox.Text}");

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter($"select * from T_Cards", connection);

                            RefreshTable();

                            TeacherAutoincrementFileStreamWrite();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message} \n Cannot Find Id {bookIdtxtbox.Text} in Books ");

                            try
                            {
                                sqlTransaction2.Rollback();



                            }
                            catch (Exception ex2)
                            {

                                MessageBox.Show(ex2.Message);
                            }


                        }
                        finally
                        {
                            connection.Close();
                        }




                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }
                }

            }



            if (selectCombokBox.SelectedIndex == 1)
            {
                scount = StudentAutoincrementFileStreamRead();
                scount++;
                StudentAutoincrementFileStreamWrite();
                using (connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    await connection.OpenAsync();

                    sqlTransaction2 = connection.BeginTransaction();

                    try
                    {
                        coverImage.Visibility = Visibility.Collapsed;


                        SqlCommand command2 = new SqlCommand("sp_SingInStudents", connection);
                        command2.CommandType = CommandType.StoredProcedure;


                        SqlParameter sp2_parameter1 = new SqlParameter();
                        sp2_parameter1.ParameterName = "@StudentFirstName";
                        sp2_parameter1.SqlDbType = SqlDbType.NVarChar;
                        sp2_parameter1.Value = firstNametxtbox.Text;

                        command2.Parameters.Add(sp2_parameter1);

                        SqlParameter sp2_parameter2 = new SqlParameter();
                        sp2_parameter2.ParameterName = "@StudentPassword";
                        sp2_parameter2.SqlDbType = SqlDbType.NVarChar;
                        sp2_parameter2.Value = passwordtxtbox.Text;

                        command2.Parameters.Add(sp2_parameter2);

                        SqlParameter sp2_parameter3 = new SqlParameter();
                        sp2_parameter3.ParameterName = "@S_CardsId";
                        sp2_parameter3.SqlDbType = SqlDbType.Int;
                        sp2_parameter3.Value = scount;

                        command2.Parameters.Add(sp2_parameter3);

                        SqlParameter sp2_parameter4 = new SqlParameter();
                        sp2_parameter4.ParameterName = "@StudentId_sc";
                        sp2_parameter4.SqlDbType = SqlDbType.Int;
                        sp2_parameter4.Value = Convert.ToInt32(StudentId_tc);

                        command2.Parameters.Add(sp2_parameter4);

                        SqlParameter sp2_parameter5 = new SqlParameter();
                        sp2_parameter5.ParameterName = "@BookId";
                        sp2_parameter5.SqlDbType = SqlDbType.Int;
                        sp2_parameter5.Value = bookIdtxtbox.Text;

                        command2.Parameters.Add(sp2_parameter5);

                        SqlParameter sp2_parameter6 = new SqlParameter();
                        sp2_parameter6.ParameterName = "@DateOut";
                        sp2_parameter6.SqlDbType = SqlDbType.DateTime;
                        sp2_parameter6.Value = dateouttxtbox.Text;

                        command2.Parameters.Add(sp2_parameter6);

                        SqlParameter sp2_parameter7 = new SqlParameter();
                        sp2_parameter7.ParameterName = "@DateIn";
                        sp2_parameter7.SqlDbType = SqlDbType.DateTime;
                        sp2_parameter7.Value = dateintxtbox.Text;

                        command2.Parameters.Add(sp2_parameter7);

                        if (libscombobox.SelectedIndex == 0)
                        {
                            SqlParameter sp2_parameter8 = new SqlParameter();
                            sp2_parameter8.ParameterName = "@LibId";
                            sp2_parameter8.SqlDbType = SqlDbType.Int;
                            sp2_parameter8.Value = 1;

                            command2.Parameters.Add(sp2_parameter8);
                        }

                        if (libscombobox.SelectedIndex == 1)
                        {
                            SqlParameter sp2_parameter8 = new SqlParameter();
                            sp2_parameter8.ParameterName = "@LibId";
                            sp2_parameter8.SqlDbType = SqlDbType.Int;
                            sp2_parameter8.Value = 2;

                            command2.Parameters.Add(sp2_parameter8);
                        }


                        command2.Transaction = sqlTransaction2;
                        //command2.ExecuteNonQuery();

                        try
                        {

                            await command2.ExecuteNonQueryAsync();
                            sqlTransaction2.Commit();

                            MessageBox.Show($"{StudentId_tc} {firstNametxtbox.Text} {passwordtxtbox.Text}");

                            dataset = new DataSet();
                            sqlDataAdapter = new SqlDataAdapter($"select * from S_Cards", connection);

                            StudentAutoincrementFileStreamWrite();
                            RefreshTable();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.Message} \n Cannot Find Id {bookIdtxtbox.Text} in Books ");

                            try
                            {
                                sqlTransaction2.Rollback();



                            }
                            catch (Exception ex2)
                            {

                                MessageBox.Show(ex2.Message);
                            }


                        }
                        finally
                        {
                            connection.Close();
                        }




                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}");
                    }
                }

            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



        }

        private void bookIdtxtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(bookIdtxtbox.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                bookIdtxtbox.Text = bookIdtxtbox.Text.Remove(bookIdtxtbox.Text.Length - 1);
            }
        }

        private void dateouttxtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");


            bool isValid = regex.IsMatch(dateouttxtbox.Text);


            DateTime dt;
            isValid = DateTime.TryParseExact(dateouttxtbox.Text, "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
            if (isValid)
            {
                MessageBox.Show( "Datetime is correct.");

            }

        }

        private void dateintxtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");


            bool isValid = regex.IsMatch(dateintxtbox.Text);


            DateTime dt;
            isValid = DateTime.TryParseExact(dateintxtbox.Text, "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
            if (isValid)
            {
                MessageBox.Show("Datetime is correct.");

            }
        }
    }
}
