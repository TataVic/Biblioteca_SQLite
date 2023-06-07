using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

/////////////////////////////////////////////////
// ATIVIDADE - BIBLIOTECA C# & SQLite //////////
// Autores: Ryhan Clayver e Tauani Vitória ////
///Data: 27/05/2023 //////////////////////////
/////////////////////////////////////////////

///Características: implementar o CRUD e Pesquisa pelos indíces com o DataGridView em SQLite
namespace Biblioteca
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        private void btnConectar_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\db\database.db";
            string strConnection = @"Data Source =" + baseDados + "; Version = 3";

            if (!File.Exists(baseDados))
            {
                SQLiteConnection.CreateFile(baseDados);
            }

            SQLiteConnection conexao = new SQLiteConnection(strConnection);

            try
            {
                conexao.Open();
                lblMural.Text = "Online!";
            }
            catch (Exception )
            {
                MessageBox.Show("Offline!", "Sem Conexão", MessageBoxButtons.OK);
            }
            finally
            {
                conexao.Close();
            }
        }
        private void btnCriar_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\db\database.db";
            string strConnection = @"Data Source =" + baseDados + "; Version = 3";

            SQLiteConnection conexao = new SQLiteConnection(strConnection);

            try
            {
                conexao.Open();
                SQLiteCommand comando = new SQLiteCommand();
                comando.Connection = conexao;
                comando.CommandText = "CREATE TABLE livros(id INTEGER" +
                    " NOT NULL PRIMARY KEY AUTOINCREMENT, Titulo VARCHAR(50)," +
                    "Autor VARCHAR(50)," +
                    "Ano VARCHAR (4))";
                comando.ExecuteNonQuery();
                lblMural.Text = "Estantes abertas!";
                comando.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("A tabela já existe!", "Atenção", MessageBoxButtons.OK);
            }
            finally
            {
                conexao.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\db\database.db";
            string strConnection = @"Data Source =" + baseDados + "; Version = 3";

            SQLiteConnection conexao = new SQLiteConnection(strConnection);
            try
            {
                conexao.Open();
                SQLiteCommand comando = new SQLiteCommand();
                comando.Connection = conexao;
                string titulo = txtTitulo.Text;
                string autor = txtAutor.Text;
                string ano = txtAno.Text;
                comando.CommandText = "INSERT INTO livros(Titulo," +
                    "Autor," + "Ano) VALUES ('" + titulo + "','" + autor + "','" + ano + "')";
                comando.ExecuteNonQuery();
                lblMural.Text = "Livro adicionado!";
                comando.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível adicionar, tente criar uma tabela", "Atenção", MessageBoxButtons.OK);
            }
            finally
            {
                conexao.Close();
                txtTitulo.Clear();
                txtAutor.Clear();
                txtAno.Clear();
                txtAutor.Focus();
                btnListar_Click(sender, e); 
            }
                
        }
        private void btnListar_Click(object sender, EventArgs e)
        {
            datagrid.Rows.Clear();
            string baseDados = Application.StartupPath + @"\db\database.db";
            string strConnection = @"Data Source =" + baseDados + "; Version = 3";

            SQLiteConnection conexao = new SQLiteConnection(strConnection);

            try
            {
                String query = "SELECT * FROM livros";

                if (txtTitulo.Text != "")
                {
                    query = "SELECT * FROM livros WHERE Titulo ='" + txtTitulo.Text + "' Autor ='" + txtAutor.Text + "'";

                }

                DataTable dados = new DataTable();
                SQLiteDataAdapter adaptador = new SQLiteDataAdapter(query, strConnection);
                conexao.Open();
                adaptador.Fill(dados);

                foreach (DataRow linha in dados.Rows)
                {
                    datagrid.Rows.Add(linha.ItemArray); //lista os dados 
                }
            }
            catch (Exception)
            {
                datagrid.Rows.Clear();
                MessageBox.Show("Não foi possível exibir os livros!" , "Atenção", MessageBoxButtons.OK);
            }
            finally
            {
                conexao.Close();
            }
        }
        
        private void datagrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datagrid.SelectedRows.Count > 0)
            {
                txtTitulo.Text = datagrid.SelectedRows[0].Cells[0].Value.ToString();
                txtAutor.Text = datagrid.SelectedRows[0].Cells[1].Value.ToString();
                txtAno.Text = datagrid.SelectedRows[0].Cells[2].Value.ToString();

            }

        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\db\database.db";
            string strConnection = @"Data Source =" + baseDados + "; Version = 3";
            SQLiteConnection conexao = new SQLiteConnection(strConnection);

            try
            {
                conexao.Open();
                SQLiteCommand comando = new SQLiteCommand();
                comando.Connection = conexao;

                int id = Convert.ToInt32(datagrid.SelectedRows[0].Cells[0].Value);
                string titulo = txtTitulo.Text;
                string autor = txtAutor.Text;
                string ano = txtAno.Text;
                comando.CommandText = "UPDATE livros SET Titulo = '"+titulo+"' WHERE Id='" +id+"'";
                comando.CommandText = "UPDATE livros SET Autor = '" + autor + "' WHERE Id='" + id + "'";
                comando.CommandText = "UPDATE livros SET Ano = '" + ano + "' WHERE Id='" + id + "'";
                comando.ExecuteNonQuery();
                lblMural.Text = "Livro atualizado!";
  
                txtTitulo.Clear();
                txtAutor.Clear();
                txtAno.Clear();
                txtTitulo.Focus();
                txtAutor.Focus();
                comando.Dispose();
               
            }
            catch (Exception )
            {
                MessageBox.Show("Não foi possível alterar!" , "Atenção", MessageBoxButtons.OK);
            }
            finally
            {
                conexao.Close();
                btnListar_Click(sender, e); //atualiza automaticamente 
            }

        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            string baseDados = Application.StartupPath + @"\db\database.db";
            string strConnection = @"Data Source =" + baseDados + "; Version = 3";
            SQLiteConnection conexao = new SQLiteConnection(strConnection);

            try
            {
                conexao.Open();
                SQLiteCommand ecomando = new SQLiteCommand(strConnection);
                ecomando.Connection = conexao;

                int id = Convert.ToInt32(datagrid.SelectedRows[0].Cells[0].Value);
                
                ecomando.CommandText = "DELETE from livros WHERE id='"+id+"'";
                ecomando.ExecuteNonQuery();
                lblMural.Text = "Livro excluído!";
                ecomando.Dispose();
               
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível realizar a exclusão!", "Aviso", MessageBoxButtons.OK);
            }
            finally 
            {
                txtTitulo.Clear();
                txtAutor.Clear();
                txtAno.Clear();
                conexao.Close();
                btnListar_Click(sender, e);
            }
            }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            datagrid.Rows.Clear();
            int opcao = cmbOpcao.SelectedIndex; //opção pelos index do nome do datagrid
            // 3 opções: Título, Autor e Ano

            string baseDados = Application.StartupPath + @"\db\database.db";
            string strConnection = @"Data Source =" + baseDados + "; Version = 3";
            SQLiteConnection conexao = new SQLiteConnection(strConnection);

            switch (opcao)
            {
                case 0:
                    try //Opção para encontrar o título
                    {
                        string query = "SELECT * FROM livros WHERE Titulo = '" + txtBoxPesquisado.Text+"'";

                        DataTable dados = new DataTable();
                        SQLiteDataAdapter adapta = new SQLiteDataAdapter(query, strConnection);
                        conexao.Open();
                        adapta.Fill(dados);
                        
                        foreach(DataRow linha in dados.Rows)
                        {
                            datagrid.Rows.Add(linha.ItemArray);
                        }
                    }
                    catch (Exception)
                    {
                        datagrid.Rows.Clear();
                        MessageBox.Show("Título não encontrado!", "Aviso", MessageBoxButtons.OK);

                    }
                    finally { conexao.Close(); }
                    break;

                case 1: //Opção para encontrar o autor
                    try
                    {
                        string query = "SELECT * FROM livros WHERE Autor= '" + txtBoxPesquisado.Text + "'";

                        DataTable dados = new DataTable();
                        SQLiteDataAdapter adapta = new SQLiteDataAdapter(query, strConnection);
                        conexao.Open();
                        adapta.Fill(dados);

                        foreach (DataRow linha in dados.Rows)
                        {
                            datagrid.Rows.Add(linha.ItemArray);
                        }
                    }
                    catch (Exception)
                    {
                        datagrid.Rows.Clear();
                        MessageBox.Show("Autor não encontrado!", "Aviso", MessageBoxButtons.OK);

                    }
                    finally { conexao.Close(); }
                    break;

                case 2: //Opção para encontrar o ano 
                    try
                    {
                        string query = "SELECT * FROM livros WHERE Ano = '" + txtBoxPesquisado.Text + "'";

                        DataTable dados = new DataTable();
                        SQLiteDataAdapter adapta = new SQLiteDataAdapter(query, strConnection);
                        conexao.Open();
                        adapta.Fill(dados);

                        foreach (DataRow linha in dados.Rows)
                        {
                            datagrid.Rows.Add(linha.ItemArray);
                        }
                    }
                    catch (Exception)
                    {
                        datagrid.Rows.Clear();
                        MessageBox.Show("Livro não encontrado!", "Aviso", MessageBoxButtons.OK);

                    }
                    finally { conexao.Close(); }
                    break;

                default:
                    MessageBox.Show("Nenhum livro encontrado, tente novamente!", "Erro", MessageBoxButtons.OK);
                    break;
            }
        }
    }
}

