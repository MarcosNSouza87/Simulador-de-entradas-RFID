using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Prj_Simulador_B01
{
    public partial class frm_principal : Form
    {
        #region Variaveis


        string cnnDB, cda, sala = "", periodo = "", readMsg;
        int ct = 0;


        bool loadSimu = false, aEconn, aWconn, checkDB = false;
        IPEndPoint ipEnd_clienteE;
        Socket clientSock_clienteE;
        IPEndPoint ipEnd_clienteW;
        Socket clientSock_clienteW;

        static byte[] inStream = new byte[18];
        byte[] b = new byte[4];
        int ti, tf, sl = 1, count = 0;
        int[] numeros = new int[40];



        //dias da semana
        string[] diaSemana = { "Segunda", "Terça", "Quarta", "Quinta", "Sexta" };
        int ds = 0;
        byte dia_Semana;

        byte token = 0;


        // final enviar dados
        byte[] hrs = new byte[14];
        List<byte[]> userA = new List<byte[]>();
        //byte[][][] userA = new byte[6][40][4]
        List<byte[]> turma = new List<byte[]>();
        List<byte[]> terminal = new List<byte[]>();

        //quantidades
        byte[] amount_User = new byte[6]; //0 users_A,usersB,
        byte[] amount_turma = new byte[6];
        byte[] qtd_Terminais = new byte[2]; //C.A.  C.P
                                            //List<byte[]> lstTerminais = new List<byte[]>();
                                            //   byte[][] lstTerminais = new byte[50][];
                                            //simulacao
                                            //byte[,] amt_user_falta = new byte[72,4];
        
        byte[][] simuA = new byte[10][];
        byte[][] simuB = new byte[10][];
       
        byte day = 0;

        List<Terminal> lstTerminais = new List<Terminal>();
        List<User> user = new List<User>();
        User[] ufalt;
        #endregion
        //construtor
        public frm_principal()
        {

            InitializeComponent();
            flpnl_main.Size = new System.Drawing.Size(1197, 480);
            btnDataBase.BackColor = btnConnEthernet.BackColor = btnConnWifi.BackColor = btnLOAD2DB.BackColor = btnVincularDB.BackColor = btnArduino.BackColor = btnDB_conn.BackColor = Color.Red;
            date_begin.CustomFormat = "dddd dd/MM/yyyy";
            date_end.CustomFormat = "dd/MM/yyyy";


        }


        #region Menu de Icones
        private void btnTerminais_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnl_terminais);
        }

        private void btnDataBase_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnl_DB);
        }
        private void btnUsers_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnl_gearDt);
        }
        private void btnSimulation_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnl_simu);
        }

        private void btnHistoryLog_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnlHistoricoLog);
        }
        private void btnGraphics_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnl_Chart);
        }
        private void btnArduino_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnl_Arduino);
        }

        private void btnConfigSimu_Click(object sender, EventArgs e)
        {
            vpnl_in_pnl(pnl_simu_config);
        }
        #endregion
        #region Expansão de Windows
        int x = 112;
        private void btnConfig_Click(object sender, EventArgs e)
        {
        }
        int k = 330;
        private void btnHistóricoTerminalX_Click(object sender, EventArgs e)
        {
            if (k == 330)
            { pnl_terminais.Size = new System.Drawing.Size(690, 448); k = 690; }
            else
            {
                pnl_terminais.Size = new System.Drawing.Size(330, 448); k = 330;
            }
        }
        #endregion
        #region Lista de Terminais

        private void btnGravarTerminais_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvTerminais.Rows.Count; i++)
            {

            }
        }

        private void btnAddTerminais_Click(object sender, EventArgs e)
        {
            byte[] trm = new byte[13];
            trm = new byte[] { 116, 114, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            if (tbNmTpAcss.Text != "Tipo")//&& cbnTurm.Text != "Turma")
            {
                
                int x = int.Parse(tb_qtdListTerm.Text);
                string ip = String.Format("{0}.{1}.{2}.{3}", IPA_LstTerm.Text, IPB_LstTerm.Text, IPC_LstTerm.Text, IPD_LstTerm.Text);
                if (tbNmTpAcss.SelectedIndex == 1)
                {
                    tf = x;
                }

                if (tb_qtdListTerm.Text != "")
                {
                    b = strtoBt(ip);
                    // int p = int.Parse(tbPortaTerm.Text);

                    for (int i = 0; i < x; i++)
                    {
                        #region for_terminal
                        if (b[3] == 255)
                        {
                            if (b[2] == 255)
                            {
                                if (b[1] == 255)
                                {
                                    if (b[0] == 255) { }
                                    b[0]++;
                                }
                                b[1]++;
                            }
                            b[2]++;
                        }

                        if (cda != tbT.Text) sl = 1;

                        sala = String.Format(tbT.Text + "{0:D2}", sl++);

                        cda = tbT.Text;

                        if (cbPeriodo.Text != "Periodo") { periodo = cbPeriodo.Text.Substring(0,1) + " - " + diaSemana[ds++]; }
                        if (ds == 5) { ds = 0; }
                        if (!cbPeriodo.Enabled) { periodo = "-Integral-"; }

                        if (checkDB == false)
                        {
                            if (aWconn == true || aEconn == true)
                            {


                                // lstSalas.Add((dia++).ToString() + String.Format("{0}{1:D4}", cbPeriodo.SelectedIndex, count) + cbnTurm.SelectedIndex);
                            }
                        }
                        byte tp = 0;
                        // terminais.Add(tbNmTpAcss.SelectedIndex.ToString() + String.Format("{0:D4}", count++));   
                        if (tbNmTpAcss.SelectedIndex == 0)
                        {
                            tp = 65; //controle de acesso A = 65 
                            qtd_Terminais[0]++;
                        }
                        if (tbNmTpAcss.SelectedIndex == 1)
                        {
                            tp = 80; //controle de presença  P = 80
                            Console.WriteLine(cbnTurm.Text);
                            if (cbnTurm.Text == "Turma") { MessageBox.Show("Selecione uma turma para o tipo C.P");break; }
                            trm[2] = Convert.ToByte(cbnTurm.Text);
                            trm[3 + ds * 2] = Convert.ToByte(Convert.ToChar(cbPeriodo.Text.Substring(0, 1).ToLower()));
                            trm[4 + ds * 2] = Convert.ToByte(count);
                            qtd_Terminais[1]++;
                        }
                        if (tp != 0) // tratamento de erro caso o tipo ñ tenha sido escolhido
                        {
                            Terminal t = new Terminal();
                            terminal.Add(new byte[] { 109, 118, tp, Convert.ToByte(count) });
                            t.Ip = new byte[] { b[0], b[1], b[2], b[3] };
                            t.TipoTerminal = tbNmTpAcss.Text.Substring(0,6);
                            t.Num_sala = (sl - 1).ToString("D2");
                            t.Bloco = tbT.Text;
                            t.IdTerminal = Convert.ToByte(count);
                            lstTerminais.Add(t);
                            count++;
                            dgvTerminais.Rows.Add(string.Format("{0:D3}.{1:D3}.{2:D3}.{3:D3}", b[0], b[1], b[2], b[3]++), tbNmTpAcss.Text, sala, periodo, cbnTurm.Text);

                        }
                        if (ds == 4 && tp == 80)
                        {
                            turma.Add(trm);
                        }
                        #endregion
                    }

                    if (checkDB == false)
                    {
                        turma.Add(trm);
                    }

                    ti = x;
                    //IPA_LstTerm.Text = String.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
                    IPA_LstTerm.Text = String.Format("{0}", b[0]);
                    IPB_LstTerm.Text = String.Format("{0}", b[1]);
                    IPC_LstTerm.Text = String.Format("{0}", b[2]);
                    IPD_LstTerm.Text = String.Format("{0}", b[3]);
                    tbNmTpAcss.Text = "Tipo";

                }


            }
            else
            {
                MessageBox.Show("selecione um tipo");
            }

        }

        private void btnClearTerminais_Click(object sender, EventArgs e)
        {
            dgvTerminais.Rows.Clear();
            tokenTe = 0;
            terminal.Clear();
            qtd_Terminais[0] = qtd_Terminais[1] = 0;
            lstTerminais.Clear();
        }

        /// importa para o banco de dados a lista de terminais feita...
        private void btnLT_UpLoad2DB_Click(object sender, EventArgs e)
        {


            MySqlConnection conn = new MySqlConnection(cnnDB);
            try
            {
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO terminal (id_terminal,IP,Tipo,bloco_sala,num_sala,id_historico)VALUES";

                for (int i = 0; i < lstTerminais.Count; i++)
                {
                    comm.CommandText += string.Format("(@id_terminal{0},@Ip{0},@Tipo{0},@bloco{0},@num{0},@id_historico{0})", i);
                    comm.Parameters.AddWithValue("@id_terminal" + i.ToString(), i.ToString());
                    comm.Parameters.AddWithValue("@Ip" + i.ToString(), lstTerminais[i].getIPString());
                    comm.Parameters.AddWithValue("@Tipo" + i.ToString(), lstTerminais[i].TipoTerminal);
                    comm.Parameters.AddWithValue("@bloco" + i.ToString(), lstTerminais[i].Bloco);
                    comm.Parameters.AddWithValue("@num" + i.ToString(), lstTerminais[i].Num_sala);
                    comm.Parameters.AddWithValue("@id_historico" + i.ToString(), i.ToString("D2"));
                    if (i == lstTerminais.Count - 1)
                    {
                        comm.CommandText += ";";
                    }
                    else
                    {
                        comm.CommandText += ",";
                    }

                }

                comm.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error ao importar :  \nMensagem do Banco de Dados ::\n" + ex.Message,
                    "ERRO AO IMPORTAR PARA O BANCO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnHistoricoTerminal_Click(object sender, EventArgs e)
        {

        }
        private void btnShowIP_Click(object sender, EventArgs e)
        {
            //dgvTerminais.Columns["cIP"].Visible = !dgvTerminais.Columns["cIP"].Visible;
        }

        private void btnShowPorta_Click(object sender, EventArgs e)
        {
            dgvTerminais.Columns["cPort"].Visible = !dgvTerminais.Columns["cPort"].Visible;
        }

        private void btnShowStatus_Click(object sender, EventArgs e)
        {
            dgvTerminais.Columns["cTipo"].Visible = !dgvTerminais.Columns["cTipo"].Visible;
        }

        int tokenTe = 0;

        private void btnLOAD2DB_Click(object sender, EventArgs e)
        {
            if (checkDB == true)
            {
                MySqlConnection conn = new MySqlConnection(cnnDB);
                byte[] trm = new byte[] { 116, 114, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                //  byte[] trmB = new byte[] { 116, 114, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                try
                {

                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        btnDB_conn.BackColor = Color.Lime;
                    }
                    else btnDB_conn.BackColor = Color.Red;


                    string sql = String.Format("SELECT " +
                   "sala.idSala, sala.bloco_sala, sala.perido_aula, sala.hra_d_aula, sala.Diadasemana, disciplina.idTURMA ," +
                   " turma.nomeTurma " +
                   "FROM `sala`,disciplina,turma " +
                   "WHERE sala.idDisciplina = disciplina.idDISCIPLINA AND turma.idTURMA = disciplina.idTURMA");

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    int dia = 0;
                    //  int cA = 0, cB = 0;
                    tbNmTpAcss.SelectedIndex = 1;
                    string ip = String.Format("{0}.{1}.{2}.{3}", IPA_LstTerm.Text, IPB_LstTerm.Text, IPC_LstTerm.Text, IPD_LstTerm.Text);

                    b = strtoBt(ip);
                    byte tp = 0;

                    while (reader.Read())
                    {
                        string ds = reader["Diadasemana"].ToString();
                        string blc_sl = reader["bloco_sala"].ToString();
                        string num_sl = String.Format("{0:D2}", reader["idSala"]);
                        string periodo = reader["perido_aula"].ToString();
                        string turmaDt = reader["idTURMA"].ToString();
                        string hraEntrada = reader["hra_d_aula"].ToString();

                        for (int k = 0; k < x; k++)
                        {
                            if (b[3] == 255)
                            {
                                if (b[2] == 255)
                                {
                                    if (b[1] == 255)
                                    {
                                        if (b[0] == 255) { b[0] = 1; }
                                        b[0]++;
                                    }
                                    b[1]++;
                                }
                                b[2]++;
                            }
                        }


                        if (periodo == "Matutino")
                        {
                            HEM_H.Text = hraEntrada.Substring(0, 2); HEM_Min.Text = hraEntrada.Substring(3, 2);
                            HIM_H.Text = String.Format("{0:D2}", 1 + int.Parse(hraEntrada.Substring(0, 2)));
                            HIM_Min.Text = String.Format("{0:D2}", 15 + int.Parse(hraEntrada.Substring(3, 2)));
                            HSM_H.Text = String.Format("{0:D2}", 3 + int.Parse(hraEntrada.Substring(0, 2)));
                            HSM_Min.Text = String.Format("{0:D2}", int.Parse(hraEntrada.Substring(3, 2)));
                        }
                        if (periodo == "Noturno")
                        {
                            HEN_H.Text = hraEntrada.Substring(0, 2); HEN_Min.Text = hraEntrada.Substring(3, 2);
                            HIN_H.Text = String.Format("{0:D2}", 1 + int.Parse(hraEntrada.Substring(0, 2)));
                            HIN_Min.Text = String.Format("{0:D2}", 15 + int.Parse(hraEntrada.Substring(3, 2)));
                            HSN_H.Text = String.Format("{0:D2}", 3 + int.Parse(hraEntrada.Substring(0, 2)));
                            HSN_Min.Text = String.Format("{0:D2}", int.Parse(hraEntrada.Substring(3, 2)));

                        }
                        hrs[0] = 104;
                        hrs[1] = 112;
                        hrs[2] = Convert.ToByte(HEM_H.Text);
                        hrs[3] = Convert.ToByte(HEM_Min.Text);
                        hrs[4] = Convert.ToByte(HIM_H.Text);
                        hrs[5] = Convert.ToByte(HIM_Min.Text);
                        hrs[6] = Convert.ToByte(HSM_H.Text);
                        hrs[7] = Convert.ToByte(HSM_Min.Text);
                        hrs[8] = Convert.ToByte(HEN_H.Text);
                        hrs[9] = Convert.ToByte(HEN_Min.Text);
                        hrs[10] = Convert.ToByte(HIN_H.Text);
                        hrs[11] = Convert.ToByte(HIN_Min.Text);
                        hrs[12] = Convert.ToByte(HSN_H.Text);
                        hrs[13] = Convert.ToByte(HSN_Min.Text);
                        switch (ds)
                        {
                            case "segunda": dia = 0; break;
                            case "Terca": dia = 1; break;
                            case "Quarta": dia = 2; break;
                            case "Quinta": dia = 3; break;
                            case "Sexta": dia = 4; break;
                        }

                        //if (cbGD_NumTurm.Items.Contains(dgvDtSimu.Rows[i].Cells[2].Value.ToString())) // ñ faz sentido pra mim!
                        //{
                        //    int tt = cbGD_NumTurm.SelectedIndex = cbGD_NumTurm.FindString(dgvDtSimu.Rows[i].Cells[2].Value.ToString());
                        //    Console.WriteLine("numero do index da turma :: " + Convert.ToByte(97 + tt));
                        //   // userA.Add(new byte[] { 117, 115, Convert.ToByte(97 + tt), card[0], card[1], card[2], card[3] });
                        //    amount_User[tt]++;
                        //}

                        if (tokenTe == 0)
                        {
                            //Terminal t = new Terminal();
                            //t.Ip = new byte[] { b[0], b[1], b[2], b[3] };
                            //lstTerminais.Add(t);
                            //lstTerminais.Add(new byte[] { Convert.ToByte(count), b[0], b[1], b[2], b[3] });
                            string term = string.Format("{0:D3}.{1:D3}.{2:D3}.{3:D3}", b[0], b[1], b[2], b[3]);

                            dgvTerminais.Rows.Add(term, tbNmTpAcss.Text, blc_sl + num_sl, (periodo.Substring(0, 1) + " - " + ds), turmaDt);

                            int tt = cbGD_NumTurm.SelectedIndex = cbGD_NumTurm.FindString(reader["idTURMA"].ToString());

                            tp = Convert.ToByte(97 + tt); //controle de presença
                            trm[2] = tp;
                            trm[3 + dia * 2] = Convert.ToByte(Convert.ToChar(periodo.Substring(0, 1).ToLower()));
                            trm[4 + dia * 2] = Convert.ToByte(count);
                        }


                        tp = 80;
                        if (tp != 0)
                        {
                            terminal.Add(new byte[] { 109, 118, tp, Convert.ToByte(count) });
                            Terminal t = new Terminal();
                            t.Ip = new byte[] { b[0], b[1], b[2], b[3]++ };
                            t.Num_sala = num_sl;
                            t.Bloco = blc_sl;
                            t.TipoTerminal = tbNmTpAcss.Items[1].ToString().Substring(0, 6);
                            lstTerminais.Add(t);
                            qtd_Terminais[1]++;
                            count++;
                        }
                        if (dia == 4)
                        {
                            turma.Add(new byte[] { 116, 114, trm[2], trm[3], trm[4], trm[5], trm[6], trm[7], trm[8], trm[9], trm[10], trm[11], trm[12] });
                        }

                    }

                    for (int i = 0; i < lstTerminais.Count; i++)
                    {
                        for (int n = 0; n < 4; n++)
                            Console.Write(lstTerminais[i].Ip[n] + ".");
                        Console.WriteLine();
                    }

                    reader.Close();
                    conn.Close();
                    //tbIPTerm.Text = String.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
                    IPA_LstTerm.Text = String.Format("{0}", b[0]);
                    IPB_LstTerm.Text = String.Format("{0}", b[1]);
                    IPC_LstTerm.Text = String.Format("{0}", b[2]);
                    IPD_LstTerm.Text = String.Format("{0}", b[3]);
                    tbNmTpAcss.Text = "Tipo";




                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro na Conexão com o Banco de Dados!  \nMessage do DB::\n" + ex.Message, "Status:: Não Conectado ao Banco de Dados",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                tokenTe = 1;
            }else
            {
                MessageBox.Show("Conecte ao Banco Primeiro!", "Status:: Não Conectado ao Banco de Dados", MessageBoxButtons.OK,MessageBoxIcon.Information);
                dgvDtSimu.Rows.Clear();
                btnDataBase_Click(sender, e);
            }
        
        }


        private void dgvTerminais_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvHistTerm.Rows.Clear();
            if (e.RowIndex >= 0)
            {
                //Console.WriteLine(dgvTerminais.Rows[e.RowIndex].Cells[0].Value.ToString());
                string ip = dgvTerminais.Rows[e.RowIndex].Cells[0].Value.ToString();
                //012345678901234
                //192.168.015.001
                IPA_Search.Text = (int.Parse(ip.Substring(0, 3))).ToString();
                IPB_Search.Text = (int.Parse(ip.Substring(4, 3))).ToString();
                IPC_Search.Text = (int.Parse(ip.Substring(8, 3))).ToString();
                IPD_Search.Text = (int.Parse(ip.Substring(12, 3))).ToString();
                int i = e.RowIndex;
                for (int n = 0; n < lstTerminais[i].historicoTerminal.Count; n++)
                {
                    string hora = String.Format("{0:D2}:{1:D2}:{2:D2}",
                        lstTerminais[i].historicoTerminal[n].Hora[0],
                        lstTerminais[i].historicoTerminal[n].Hora[1],
                        lstTerminais[i].historicoTerminal[n].Hora[2]);
                    string card = String.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}",
                        lstTerminais[i].historicoTerminal[n].Card[0],
                        lstTerminais[i].historicoTerminal[n].Card[1],
                        lstTerminais[i].historicoTerminal[n].Card[2],
                        lstTerminais[i].historicoTerminal[n].Card[3]);
                    char c = Convert.ToChar(lstTerminais[i].historicoTerminal[n].Tipo);
                    User u = new User();
                    u = user.Find(x => x.Cards.Contains(card));
                    dgvHistTerm.Rows.Add(hora, lstTerminais[i].historicoTerminal[n].Date, c, card, u.Nome);
                }
            }
        }

        #region pesquisa no histórico dos terminais 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //string search = tbLT_Search.Text;
            //int tipo = cbLT_tipoS.SelectedIndex;
            //if (cbLT_tipoS.Text != "Tipo")
            //{
            //    for (int i = 0; i < lstTerminais.Count; i++)
            //    {
            //        if (tipo == 0)
            //        {

            //        }
            //    }
            //}

        }

        private void btnClearS_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void tb_qtdListTerm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 08)
            {
                //Atribui True no Handled para cancelar o evento
                e.Handled = true;
            }
        }

        #region textBox Ips lista terminais

        private void IPA_LstTerm_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPA_LstTerm, e);
        }

        private void IPB_LstTerm_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPB_LstTerm, e);
        }

        private void IPC_LstTerm_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPC_LstTerm, e);
        }

        private void IPD_LstTerm_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPD_LstTerm, e);
        }
        #endregion

        #endregion
        #region Gerador de Dados

        bool a = false;
        private void tQtd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("este campo aceita somente numero e virgula");
                a = false;
            }
            else
                a = true;
        }

        string[] nomeProprio =
        {
            "Mirghel",
            "Davi",
            "Arthur",
            "Pedro",
            "Gabriel",
            "Bernado",
            "Lucas",
            "Matheus",
            "Rafael",
            "Enzo",
            "Heitor"
        };
        string[] nomeSobrenome =
        {
            "Alves","Monteiro",
            "Novaes","Mendes",
            "Barros","Freitas","Barbosa",
            "Pinto","Moura",  "Dias",
            "Castro", "Campos",
            "Cardoso", "Silva", "Souza",
            "Costa", "Santos"
        };

        bool cardU = false, nameU = false;

        private void btnGerarDt_Click(object sender, EventArgs e)
        {
            int qtd;

            if (checkDB)
            {
                qtd = dgvDtSimu.RowCount - 1; // muda aqui
                userA.Clear();
                Array.Clear(amount_User, 0, amount_User.Length);
            }
            else
            {
                if (cbGD_QtdUsers.Text == "Qtd Users") { cbGD_QtdUsers.Text = "1"; }
                qtd = int.Parse(cbGD_QtdUsers.Text);
            }
            if (cbGD_NumTurm.Text != "Numero Turma" || checkDB == true)
            {

                Random rnd = new Random();

                for (int i = 0; i < qtd; i++)
                {

                    string nm = dgvDtSimu.Rows[i].Cells[0].Value.ToString();
                    if (nameU == true)
                    {
                        nm = "";
                        int np = rnd.Next((nomeProprio.Length - 1));  //np = nome proprio
                        int ca = rnd.Next((nomeProprio.Length - 1));  //ca = nome proprio (composto A)
                        int ns = rnd.Next((nomeSobrenome.Length - 1));//ns = nome sobrenome
                        int cb = rnd.Next((nomeSobrenome.Length - 1));//cb = nome sobrenome (composto B)
                                                                      // int turma = rnd.Next(int.Parse(qb.ToString()));
                        int x = rnd.Next(0, 4);
                        if (np == ca || ns == cb) { ca++; cb++; }

                        switch (x)
                        {
                            case 0: //
                                nm = nomeProprio[np] + " " + nomeSobrenome[ns];
                                break;
                            case 1:
                            case 4:
                                nm = nomeProprio[np] + " " + nomeSobrenome[ns]
                                    + " de " + nomeSobrenome[cb];
                                break;
                            case 2:

                                nm =
                                    nomeProprio[np] + " " + nomeProprio[ca] + " " +
                                    nomeSobrenome[ns];
                                break;
                            case 3:

                                nm = nomeProprio[np] + " " + nomeProprio[ca] + " " +
                                       nomeSobrenome[ns] + " de " + nomeSobrenome[cb];
                                break;
                        }
                    }
                    User u = new User();
                    string cardStr = dgvDtSimu.Rows[i].Cells["cCard"].Value.ToString();
                    byte[] card = new byte[4];
                    if (cardU == true)
                    {                     
                        rnd.NextBytes(card);
                        cardStr = BitConverter.ToString(card);
                    }
                    else
                    {

                        card[0] = Convert.ToByte(dgvDtSimu.Rows[i].Cells[1].Value.ToString().Substring(0, 2), 16);
                        card[1] = Convert.ToByte(dgvDtSimu.Rows[i].Cells[1].Value.ToString().Substring(3, 2), 16);
                        card[2] = Convert.ToByte(dgvDtSimu.Rows[i].Cells[1].Value.ToString().Substring(6, 2), 16);
                        card[3] = Convert.ToByte(dgvDtSimu.Rows[i].Cells[1].Value.ToString().Substring(9, 2), 16);
                        cardStr = dgvDtSimu.Rows[i].Cells[1].Value.ToString();
                    }

                    if (checkDB) // muda aqui
                    {

                        dgvDtSimu.Rows[i].Cells[0].Value = nm;
                        dgvDtSimu.Rows[i].Cells[1].Value = cardStr;
                        if (cbGD_NumTurm.Items.Contains(dgvDtSimu.Rows[i].Cells[2].Value.ToString())) // ñ faz sentido pra mim!
                        {
                            int tt = cbGD_NumTurm.SelectedIndex = cbGD_NumTurm.FindString(dgvDtSimu.Rows[i].Cells[2].Value.ToString());
                            Console.WriteLine("numero do index da turma :: " + Convert.ToByte(97 + tt));
                            userA.Add(new byte[] { 117, 115, Convert.ToByte(97 + tt), card[0], card[1], card[2], card[3] });

                            u.Nome = nm;
                            u.Ids_turma = String.Format("{0}", Convert.ToByte(97 + tt));
                            u.Cardb = card;
                            u.Cards = cardStr;
                            u.IdUser = dgvDtSimu.Rows[i].Cells["Ciduser"].Value.ToString();
                            user.Add(u);
                            amount_User[tt]++;
                        }

                    }
                    else
                    {
                        if (aWconn == true || aEconn == true)
                        {
                            userA.Add(new byte[] { 117, 115, Convert.ToByte(Convert.ToChar(cbGD_NumTurm.Text)), card[0], card[1], card[2], card[3] });

                            u.Nome = nm;
                            // u.Ids_turma = String.Format("{0}", Convert.ToByte(97 + tt));
                            u.Cardb = card;
                            u.Cards = cardStr;
                            user.Add(u);
                            amount_User[cbGD_NumTurm.SelectedIndex]++;
                            dgvDtSimu.Rows.Add(nm, cardStr, cbGD_NumTurm.Text);
                        }
                    }
                }
                if (token == 0)
                {
                    cbGD_NumTurm.Enabled = cbGD_QtdUsers.Enabled = true;
                    token++;
                }




                //Console.WriteLine("Quantidade turma " + cbGD_NumTurm.Items[0].ToString() + "::" + amount_User[0]);
                //Console.WriteLine("Quantidade turma " + cbGD_NumTurm.Items[1].ToString() + ":" + amount_User[1]);
                cbGD_QtdUsers.Text = "Qtd Users";
                cbGD_NumTurm.Text = "Turma";

            }
            else
            {
                MessageBox.Show("Selecione uma turma para adicionar os usuarios");
            }

        }

        /*
         * 


         *    
            int qtd = int.Parse( tbQtUser.Text);

            dgvList.Rows.Clear();
            Random rnd = new Random();
            List<int> lista = new List<int>();
            for (int i = 0; i < qtd; i++)
            {
                string nm = "";
                int np = rnd.Next((nomeProprio.Length - 1));  //np = nome proprio
                int ca = rnd.Next((nomeProprio.Length - 1));  //ca = nome proprio (composto A)
                int ns = rnd.Next((nomeSobrenome.Length - 1));//ns = nome sobrenome
                int cb = rnd.Next((nomeSobrenome.Length - 1));//cb = nome sobrenome (composto B)
                int x = rnd.Next(0, 4);
                if (np == ca || ns == cb) { ca++; cb++; }
                
                switch (x)
                {
                    case 0: //
                        nm = nomeProprio[np] + " " + nomeSobrenome[ns];
                        break;
                    case 1:
                    case 4:
                        nm = nomeProprio[np] + " " + nomeSobrenome[ns]
                            + " de " + nomeSobrenome[cb];
                        break;
                    case 2:
                        
                        nm =
                            nomeProprio[np] + " " + nomeProprio[ca] + " " +
                            nomeSobrenome[ns];
                        break;
                    case 3:
                        
                        nm = nomeProprio[np] + " " + nomeProprio[ca] + " " +
                               nomeSobrenome[ns] + " de " + nomeSobrenome[cb];
                        break;
                }
                //int dia = rnd.Next(1, 27);
                //int mes = rnd.Next(1, 12);
                //int ano = rnd.Next(1995, 2001);
                //string aniversario = dia + "/" + mes + "/" + ano;
                byte[] card = new byte[4];

                rnd.NextBytes(card);
                string cardStr = BitConverter.ToString(card);

                dgvList.Rows.Add(nm, 10, cardStr);

            }
        }
         */
        private void btnDelDt_Click(object sender, EventArgs e)
        {
            dgvDtSimu.Rows.Clear();
            userA.Clear();
        }

        private void cbColunasParaSimulacao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void cbDB_colunas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void cbDB_Tabelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDB_colunas.Items.Clear();
            try
            {
                MySqlConnection conn = new MySqlConnection(cnnDB);
                conn.Open();
                string sql = String.Format("SELECT * FROM information_schema.COLUMNS where table_name = '{0}'", cbDB_Tabelas.Text);

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbDB_colunas.Items.Add(reader["COLUMN_NAME"].ToString());
                }
                cbDB_colunas.Text = "";
                /*
                DataSet ds = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds, cbTables.Text);

                dgvTable.DataSource = ds.Tables[cbTables.Text];
                */
                conn.Close();


            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro :: " + ex);
            }
        }
        #endregion
        #region configurar Arduino para Conectar

        bool s = false;
        private void btnConnEthernet_Click(object sender, EventArgs ex)
        {
            string tbEnderecoIP = IPAE.Text + "." + IPBE.Text + "." + IPCE.Text + "." + IPDE.Text;
            try
            {

                ipEnd_clienteE = new IPEndPoint(IPAddress.Parse(tbEnderecoIP), int.Parse(PortE.Text));
                clientSock_clienteE = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                if (int.Parse(PortE.Text) != int.Parse(PortW.Text) && s == false)
                {


                    IAsyncResult result = clientSock_clienteE.BeginConnect(ipEnd_clienteE, null, null);

                    s = result.AsyncWaitHandle.WaitOne(1000, true);
                    if (!s)
                    {
                        clientSock_clienteE.Close();
                        MessageBox.Show("Erro ao conectar!  \nVerifique o IP do Arduino Uno e a porta!", "Arduino Uno Ethernet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnConnEthernet.BackColor = Color.Red;
                        aEconn = false;
                    }
                    else
                    {
                        btnArduino.BackColor = btnConnEthernet.BackColor = Color.Lime;
                        aEconn = true;
                        if(checkDB == false)
                        { 
                        for (byte i = 0; i < 2; i++)
                        {
                            cbGD_NumTurm.Items.Add(Convert.ToChar(97 + i));
                            cbnTurm.Items.Add(Convert.ToChar(97 + i));
                        }
                        }
                        cbGD_QtdUsers.Enabled = cbGD_NumTurm.Enabled = true;

                        //if (!thread_ArduinoUno.IsBusy)
                        //    thread_ArduinoUno.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show("Não é possivel se conectar no mesmo arduino 2 vezes");
                }

            }
            catch (SocketException se)
            { btnConnEthernet.BackColor = Color.Red;
                MessageBox.Show(se.Message);
            }
        }

        private void btnConnWifi_Click(object sender, EventArgs e)
        {
            string tbEnderecoIP = IPAW.Text + "." + IPBW.Text + "." + IPCW.Text + "." + IPDW.Text;
            try
            {
                if (int.Parse(PortE.Text) != int.Parse(PortW.Text))
                {
                    ipEnd_clienteW = new IPEndPoint(IPAddress.Parse(tbEnderecoIP), int.Parse(PortW.Text));
                    clientSock_clienteW = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    IAsyncResult result = clientSock_clienteW.BeginConnect(ipEnd_clienteW, null, null);

                    bool sucesso = result.AsyncWaitHandle.WaitOne(1000, true);
                    if (!sucesso)
                    {
                        clientSock_clienteW.Close();
                        MessageBox.Show("Erro ao conectar!  \nVerifique o IP do Arduino Wemos e a porta!", "Arduino Wemos D1 - Wifi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnConnWifi.BackColor = Color.Red;
                        aWconn = false;
                    }
                    else
                    {
                        btnArduino.BackColor = btnConnWifi.BackColor = Color.Lime;
                        aWconn = true;
                        Console.WriteLine("conectado ao wemos" + aWconn);
                        //thread_ArduinoWemos.RunWorkerAsync();
                       // cbGD_NumTurm.Items.Clear();
                        int i;
                        if (checkDB == false)
                        {
                            if (aEconn == true) i = 2; else i = 0;
                            for (; i < 6; i++)
                            {
                                cbGD_NumTurm.Items.Add(Convert.ToChar(97 + i));
                                cbnTurm.Items.Add(Convert.ToChar(97 + i));
                            }
                            cbGD_NumTurm.Enabled = cbGD_QtdUsers.Enabled = true;

                        }


                    }



                }
                else
                {
                    MessageBox.Show("Não é possivel se conectar no mesmo arduino 2 vezes");
                }

            }
            catch (SocketException se)
            { btnConnWifi.BackColor = Color.Red;
                MessageBox.Show(se.Message);
            }
            
        }

        #region ARDUINO textbox - ip uno e wemos 
        private void IPAE_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPAE, e);
        }

        private void IPBE_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPBE, e);
        }

        private void IPCE_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPCE, e);
        }

        private void IPDE_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPDE, e);
        }

        private void IPAW_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPAW, e);
        }

        private void IPBW_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPBW, e);
        }

        private void IPCW_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPCW, e);
        }

        private void IPDW_TextChanged(object sender, EventArgs e)
        {
            CheckIPs(IPDW, e);
        }
        #endregion

        #endregion
        #region Simulação e configurações

        private void btnSave_Click(object sender, EventArgs e)
        {
            //29/04/2017 6 - 29/11/2017 - 06:00 - 23:30 -1- 08:00 - 09:15 - 11:00 -2- 00:00 00:00 00:00 -3- 19:00 - 20:30 - 22:30
            if (date_begin.Value.Date <= date_end.Value.Date)
            {
                dt_b = date_begin.Value.ToString("dd/MM/yyyy");
                dateVar.Value = new DateTime(int.Parse(dt_b.Substring(6, 4)), int.Parse(dt_b.Substring(3, 2)), int.Parse(dt_b.Substring(0, 2)));
                date = Convert.ToDateTime(date_end.Value.ToString("dd/MM/yyyy")) - Convert.ToDateTime(date_begin.Value.ToString("dd/MM/yyyy"));


                dia_Semana = (Convert.ToByte(date_begin.Value.DayOfWeek));
                if (dia_Semana == 0) dia_Semana = 1; else if (dia_Semana > 4) dia_Semana = 4;
                --dia_Semana;
                dateTime.Text = date_begin.Text;
                hrs[0] = 104;
                hrs[1] = 112;
                hrs[2] = Convert.ToByte(HEM_H.Text);
                hrs[3] = Convert.ToByte(HEM_Min.Text);
                hrs[4] = Convert.ToByte(HIM_H.Text);
                hrs[5] = Convert.ToByte(HIM_Min.Text);
                hrs[6] = Convert.ToByte(HSM_H.Text);
                hrs[7] = Convert.ToByte(HSM_Min.Text);
                hrs[8] = Convert.ToByte(HEN_H.Text);
                hrs[9] = Convert.ToByte(HEN_Min.Text);
                hrs[10] = Convert.ToByte(HIN_H.Text);
                hrs[11] = Convert.ToByte(HIN_Min.Text);
                hrs[12] = Convert.ToByte(HSN_H.Text);
                hrs[13] = Convert.ToByte(HSN_Min.Text);
            }
            else
                MessageBox.Show("Datas iguais!");




        }

        private void date_end_ValueChanged(object sender, EventArgs e)
        {
            if (date_end.Value < date_begin.Value)
            {
                MessageBox.Show("A data final tem que ser maior que a data inicial", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        #endregion
        #region Banco de Dados
        List<string> iduser = new List<string>();
        private void btnStartDB_Click(object sender, EventArgs e)
        {

            //if (tbDB_db.Text != "" && tbDB_Server.Text != "" && tbDB_User.Text != "")           
                cnnDB = String.Format("Server={0};Port=3306;Database={1};Uid={2};password={3};",  tbDB_Server.Text, tbDB_db.Text, tbDB_User.Text, tbDB_Pass.Text);
            //else
            //{
                
            //    MessageBox.Show("Preencha os campos para conectar ao banco de dados!"
            //        , "ERRO CAMPOS EM BRANCO!",
            //        MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

            MySqlConnection conn = new MySqlConnection(cnnDB);

            try
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    btnDataBase.BackColor = btnLOAD2DB.BackColor = btnVincularDB.BackColor = btnDB_conn.BackColor = Color.Lime;

                    checkDB = true;
                }
                else btnDataBase.BackColor = btnDB_conn.BackColor = Color.Red;
                cbGD_NumTurm.Items.Clear();
                //   cbGD_NumTurm.Items.Clear();
                // string sql = String.Format("SELECT * FROM information_schema.tables WHERE table_schema ='{0}'", tbDB_db.Text);
                string sql = "SELECT disciplina.idTURMA FROM sala,disciplina WHERE sala.idDisciplina = disciplina.idDISCIPLINA";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!cbGD_NumTurm.Items.Contains(reader["idTURMA"].ToString()))
                    {
                        cbGD_NumTurm.Items.Add(reader["idTURMA"].ToString());
                        cbnTurm.Items.Add(reader["idTURMA"].ToString());
                        //tk = 2;
                    }

                }
                reader.Close();
                conn.Close();
                checkDB = true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erro ao conectar!  \nMensagem do Banco de Dados ::\n" + ex.Message, "ERRO NA CONEXAO COM O BANCO DE DADOS!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                btnDataBase.BackColor = btnLOAD2DB.BackColor = btnVincularDB.BackColor = btnDB_conn.BackColor = Color.Red;
                
            }

        }

        private void ckb_vinaluno_CheckedChanged(object sender, EventArgs e)
        { }

        private void btnVincularDB_Click(object sender, EventArgs e)
        {
            if (checkDB == true)
            {           
                MySqlConnection conn = new MySqlConnection(cnnDB);

                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        btnDB_conn.BackColor = Color.Lime;
                    }
                    else btnDB_conn.BackColor = Color.Red;


                    string sql = String.Format("SELECT aluno.idALUNO,aluno.matricula,aluno.nome,aluno.cardUser, turma.idTURMA FROM `aluno`,turma WHERE aluno.turma = turma.nomeTurma");
                  //  int tk = 0;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (cbGD_NumTurm.Items.Contains(reader["idTURMA"].ToString()))
                        {
                            dgvDtSimu.Rows.Add(reader["nome"].ToString(), reader["cardUser"].ToString(), reader["idTURMA"].ToString(), reader["idALUNO"]);
                        }                  
                    }
                    reader.Close();
                    conn.Close();
                    
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Erro na Conexão com o Banco de Dados!  \nMessage do DB::\n" + ex.Message, "Status:: Não Conectado ao Banco de Dados",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                dgvDtSimu.Rows.Clear();
                btnDataBase_Click(sender, e);

            }


        }





        #endregion
        #region Histórico de Atividades
        #endregion
        #region Simulacao

        byte hrInicial, minInicial, hrFinal, minFinal;
        int limitD = 0;

        ///buttons design
        private void btnSubmitDt_Click(object sender, EventArgs e) //envia os dados pros arduinos
        {
            if (aEconn == true)
            {
                if (!thread_ArduinoUno.IsBusy)
                    thread_ArduinoUno.RunWorkerAsync();
            }
            if (aWconn == true)
            {
                if (!thread_ArduinoWemos.IsBusy)
                    thread_ArduinoWemos.RunWorkerAsync();
            }


            //wemos abaixo
            dt_b = date_begin.Value.ToString("dd/MM/yyyy");
            dayW = Convert.ToByte(date_begin.Value.DayOfWeek.ToString("d"));
            dateVar.Value = new DateTime(int.Parse(dt_b.Substring(6, 4)), int.Parse(dt_b.Substring(3, 2)), int.Parse(dt_b.Substring(0, 2)));
            date = Convert.ToDateTime(date_end.Value.ToString("dd/MM/yyyy")) - Convert.ToDateTime(date_begin.Value.ToString("dd/MM/yyyy"));
            dtSimu = "loadSimu";

            log_gif.Visible = logTxt.Visible = true;

        }

        private void btnRealTimeSimu_Click(object sender, EventArgs e)
        {
            if (dtSimu == "loadSimu")
            {
                //hrInicial = hora = Convert.ToByte(HI_Hr.Text);
                //minInicial = minuto = Convert.ToByte(HI_Min.Text);
                //hrFinal = Convert.ToByte(HF_H.Text);
                //minFinal = Convert.ToByte(HF_M.Text);
                //segundo = 0;
                //tk = 1;

                if (!bw_CPU_PCSIMULADOR.IsBusy)
                    bw_CPU_PCSIMULADOR.RunWorkerAsync();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
        }

        private void btnFastSimu_Click(object sender, EventArgs e)
        {
            if(cbPrc_falhasSistema.Text == "% de erro")
            {
                cbPrc_falhasSistema.SelectedIndex = 0;
            }
            if(cbPrc_faltas.Text == "% de faltas")
            {
                cbPrc_faltas.SelectedIndex = 0;
            }
            if (dtSimu == "waitStart")
            {
                dtSimu = "fastSimu";
                dateTime.Text =  date_begin.Value.ToString("dddd ,dd/MM/yyyy");
            }
            
        }

        private void cbPrc_falhasSistema_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void cbMin_simuRT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_timerSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        /// end desing
        /// 
        /// 
        public void uploadDate(int diaCount) 
        {           
            if (aEconn == true && aWconn == true) limitD = 2; else limitD = 1;

            if (d == limitD)
            {   //            do seg ter qua qui sex sab
                // dom = 0 ,  0   1   2   3   4   5   6
                dateVar.Value = dateVar.Value.AddDays(1);

                if (dateVar.Value.DayOfWeek.ToString("d") == "6")
                {
                    dateVar.Value = dateVar.Value.AddDays(2);
                    day = 0; dayW = 0;
                }
                else if (dateVar.Value.DayOfWeek.ToString("d") == "0")
                {
                    dateVar.Value = dateVar.Value.AddDays(1);
                    day = 0; dayW = 0;
                }
                
                //Console.WriteLine(dateVar.Value.ToString("dd/MM/yyyy"));
                
                if (dateTime.InvokeRequired)
                {
                    dateTime.BeginInvoke((MethodInvoker)delegate
                    { dateTime.Text = dateVar.Value.ToString("dddd , dd MMMM yyyy"); });
                }

                d = 0;
            }
        }
        int multRT;

        //thread_ timer
        int tu = 0, tw = 0;
        string dtSimu = "nda";
        List<byte[]> SD_listE = new List<byte[]>();
        List<byte[]> SD_listW = new List<byte[]>();
        string startSimu = "snda";
        string rmsgSimu = "";
        int xE = 0, xW = 0, d;
        TimeSpan date;
        DateTimePicker dateVar = new DateTimePicker();
        int tk = 0;
        string dt_b;

        byte[] simuDados = new byte[] { 0, 0, 0, 0 };
        byte periodE = 0, periodW = 0;
        byte timeE = 0, timeW = 0;
        byte turmE = 0, turmW = 0;
        byte dayW = 0, dayE = 0;
        /// ARDUINO UNO ---
        private void thread_ArduinoUno_DoWork(object sender, DoWorkEventArgs e)
        {
            //1 - LOAD
            //2 - GET DTS
            //3 - APP MODIFYERS  
            while (dtSimu == "nda")
            {
                Thread.Sleep(1000);
            }
            #region load to arduino UNO
            if (dtSimu == "loadSimu")
            {
                if (aEconn == true)
                {
                    #region a
                    if (clientSock_clienteE.Connected)
                    {
                        thread_ArduinoUno.ReportProgress(1, "Compilando Arquivos (1/2)...");
                        //envia os horarios
                        clientSock_clienteE.Send(hrs);
                        Thread.Sleep(100);

                        //envia os card alunos
                        if (aWconn) /// se for igual a verdadeiro quer dizer q existe uma conexão com o wemos
                        {
                            for (int j = 0; j < userA.Count; j++)
                            {
                                if (userA[numeros[j]][2] == 97 || userA[numeros[j]][2] == 98)
                                {
                                    clientSock_clienteE.Send(userA[numeros[j]]);
                                    Thread.Sleep(200);
                                }

                            }
                        }
                        else
                        {  /// senão envia os usuarios das turmas a b
                            for (int i = 0; i < userA.Count; i++)
                            {
                                clientSock_clienteE.Send(userA[i]);
                                Thread.Sleep(200);
                            }
                        }
                        // bgAUnoLoad
                        thread_ArduinoUno.ReportProgress(1, "Sucesso! (1/2)");
                        Thread.Sleep(100);
                        //envia as turmas
                        for (int i = 0; i < turma.Count; i++)
                        {
                            if (i < 2)
                            {
                                clientSock_clienteE.Send(turma[i]);
                                Thread.Sleep(200);
                            }
                        }
                        thread_ArduinoUno.ReportProgress(1, "Compilando Arquivos (2/2)...");
                        //envia os terminais
                        for (int i = 0; i < terminal.Count; i++)
                        {
                            clientSock_clienteE.Send(terminal[i]);
                            //        Console.WriteLine(terminal[i][0] + " - " + terminal[i][1] + " - " + terminal[i][2] + " - " + terminal[i][3]);
                            Thread.Sleep(100);
                        }
                        thread_ArduinoUno.ReportProgress(1, "sucesso! (2/2)...");
                        byte[] qtds = new byte[] { 113, 116, qtd_Terminais[0], qtd_Terminais[1], amount_User[0], amount_User[1] };
                        clientSock_clienteE.Send(qtds);
                        Thread.Sleep(100);
                        dtSimu = "waitStart";


                    }
                    #endregion
                }
            }
            #endregion

            if (dtSimu == "waitStart")
            {
                if (aEconn == true)
                {
                    btnSubmitDt.BeginInvoke((MethodInvoker)delegate { btnSubmitDt.Enabled = false; });
                    btnRealTimeSimu.BeginInvoke((MethodInvoker)delegate { btnRealTimeSimu.Enabled = true; });
                    btnFastSimu.BeginInvoke((MethodInvoker)delegate { btnFastSimu.Enabled = true; });
                    log_gif.BeginInvoke((MethodInvoker)delegate { log_gif.Visible = false; });
                }
            }
            while (dtSimu == "waitStart") //aguarda até mudar pra realtimer ou fastsimu
            {       Thread.Sleep(1000);     }
            if (aEconn == true)
            {
                #region realtimer_fastSimu
                //2/3 - get dt and app modifyers

                //if (dtSimu == "realTimer")
                //{
                    //while (dtSimu == "realTimer")
                    //{
                    //    int n = 0;
                    //    simuA[0] = new byte[] { 83, 115, day, 60, periodE, 0, turmE }; //entrada incial
                    //    simuA[1] = new byte[] { 83, 115, day, 61, periodE, 0, turmE }; // c.p
                    //    simuA[2] = new byte[] { 83, 115, day, 62, periodE, 1, turmE }; // saida intervalo
                    //    simuA[3] = new byte[] { 83, 115, day, 63, periodE, 1, turmE }; // volta intervalo
                    //    simuA[4] = new byte[] { 83, 115, day, 64, periodE, 2, turmE }; // c.p
                    //    simuA[5] = new byte[] { 83, 115, day, 62, periodE, 2, turmE }; // saida final

                    //    SimuStartLD(timeE++, clientSock_clienteE, 0);
                    //    int i = n;
                    //    while (rmsgSimu == "pause")
                    //    {
                    //        Thread.Sleep(1000);
                    //    }
                    //    //startSimu = "realTimer";


                    //}
                    //}
                    //else 
                 if (dtSimu == "fastSimu")
                {

                    //gerador de dados fastSimu
                    int n = 0;

                    if (date_begin.Value.DayOfWeek.ToString("d") == "6" || date_begin.Value.DayOfWeek.ToString("d") == "0")
                        day = 0;
                    else
                    {
                        day = Convert.ToByte(date_begin.Value.DayOfWeek.ToString("d"));
                        day--;
                    }
                    simuA[0] = new byte[] { 83, 115, day, 60, periodE, 0, turmE }; //entrada incial
                    simuA[1] = new byte[] { 83, 115, day, 61, periodE, 0, turmE }; // c.p
                    simuA[2] = new byte[] { 83, 115, day, 62, periodE, 1, turmE }; // saida intervalo
                    simuA[3] = new byte[] { 83, 115, day, 63, periodE, 1, turmE }; // volta intervalo
                    simuA[4] = new byte[] { 83, 115, day, 64, periodE, 2, turmE }; // c.p
                    simuA[5] = new byte[] { 83, 115, day, 62, periodE, 2, turmE }; // saida final
                    // % falta
                    calcu_Faltas(turmE);

                    while (dtSimu == "fastSimu")
                    {
                        // % erro
                        erroSistema(periodE);

                        // guarda dados
                        SimuStartLD(timeE++, clientSock_clienteE, 0);

                        for (int i = n; i < SD_listE.Count; i++)
                        {
                            string card = String.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}", SD_listE[i][5], SD_listE[i][6], SD_listE[i][7], SD_listE[i][8]);

                            string tm = String.Format("{0:D2}:{1:D2}:{2:D2}", SD_listE[i][0], SD_listE[i][1], SD_listE[i][2]);

                            int nt = int.Parse(String.Format("{0}", SD_listE[i][4]));
                            char c = Convert.ToChar(SD_listE[i][3]);

                            string term = String.Format("{0}.{1}.{2}.{3}", lstTerminais[nt].Ip[0], lstTerminais[nt].Ip[1], lstTerminais[nt].Ip[2], lstTerminais[nt].Ip[3]);
                            HistoricoTerminal ht = new HistoricoTerminal();
                            ht.Hora = new byte[] { SD_listE[i][0], SD_listE[i][1], SD_listE[i][2] };
                            ht.Card = new byte[] { SD_listE[i][5], SD_listE[i][6], SD_listE[i][7], SD_listE[i][8] };
                            ht.Tipo = SD_listE[i][3];
                            ht.Date = dateVar.Value.ToString("dd/MM/yyyy");
                            lstTerminais[nt].historicoTerminal.Add(ht);

                            if (dgv_HistoricoLog.InvokeRequired)
                            {
                                dgv_HistoricoLog.BeginInvoke((MethodInvoker)delegate
                                {
                                    dgv_HistoricoLog.Rows.Add(tm, c, term, card, dateVar.Value.ToString("dd/MM/yyyy"), nt.ToString());
                                });
                            }
                        }

                        n = SD_listE.Count;
                        upToDB();

                        if (timeE > 5)
                        {
                            timeE = 0;
                            Console.WriteLine(timeE);
                            periodE++; turmE++;
                            if (periodE == cbGD_NumTurm.Items.Count)
                            {
                                day++;
                                periodE = turmE = 0;
                                uploadDate(++d);
                                d = 0;
                                calcu_Faltas(turmE);
                            }
                            if (dateVar.Value.ToString("dd/MM/yyyy") == date_end.Value.ToString("dd/MM/yyyy"))
                            {
                                dtSimu = "stop";
                            }
                            else
                            {
                                dateTime.BeginInvoke((MethodInvoker)delegate { dateTime.Text = dateVar.Value.ToString("dddd , dd/MM/yyyy"); });
                            }

                            simuA[0] = new byte[] { 83, 115, day, 60, periodE, 0, turmE }; //entrada incial
                            simuA[1] = new byte[] { 83, 115, day, 61, periodE, 0, turmE }; // c.p
                            simuA[2] = new byte[] { 83, 115, day, 62, periodE, 1, turmE }; // saida intervalo
                            simuA[3] = new byte[] { 83, 115, day, 63, periodE, 1, turmE }; // volta intervalo
                            simuA[4] = new byte[] { 83, 115, day, 64, periodE, 2, turmE }; // c.p
                            simuA[5] = new byte[] { 83, 115, day, 62, periodE, 2, turmE }; // saida final
                        }
                        else
                        {
                            dateTime.BeginInvoke((MethodInvoker)delegate { dateTime.Text = dateVar.Value.ToString("dddd , dd/MM/yyyy"); });
                        }

                    }
                    #endregion

                }
            }
        }

        private void thread_ArduinoUno_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.ProgressPercentage == 1)
            {
                logTxt.BeginInvoke((MethodInvoker)delegate
                {
                    logTxt.Text = e.UserState.ToString();
                });
            }
        }
        private void thread_ArduinoUno_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
                logTxt.BeginInvoke((MethodInvoker)delegate
                {
                    logTxt.Text = "Aguardando inicio de simulação.";
                });
            
        }

        ///ARDUINO WEMOS
        ///  
        
        private void thread_ArduinoWemos_DoWork(object sender, DoWorkEventArgs e)
        {
            //1 - LOAD
            //2 - GET DTS
            //3 - APP MODIFYERS  
            while (dtSimu == "nda")
            {
                Thread.Sleep(1000);
            }
            #region load to arduino WEMOS
            if (dtSimu == "loadSimu")
            {
                if (clientSock_clienteW.Connected)
                {
                    thread_ArduinoWemos.ReportProgress(1, "Compilando Arquivos (1/2)...");
                    //envia os horarios
                    clientSock_clienteW.Send(hrs);
                    Thread.Sleep(100);

                    //envia os card alunos
                    if (aEconn) /// se for igual a verdadeiro quer dizer q existe uma conexão com o wemos
                    {
                        for (int j = 0; j < userA.Count; j++)
                        {
                            if (userA[numeros[j]][2] == 97 || userA[numeros[j]][2] == 98)
                            {
                                clientSock_clienteW.Send(userA[numeros[j]]);
                                Thread.Sleep(200);
                            }

                        }
                    }
                    else
                    {  /// senão envia os usuarios das turmas a b
                        for (int i = 0; i < userA.Count; i++)
                        {
                            clientSock_clienteW.Send(userA[i]);
                            Thread.Sleep(200);
                        }
                    }
                    // bgAUnoLoad
                    thread_ArduinoWemos.ReportProgress(1, " sucesso! (1/2)");
                    Thread.Sleep(100);
                    //envia as turmas
                    for (int i = 0; i < turma.Count; i++)
                    {
                        if (i < 2)
                        {
                            clientSock_clienteW.Send(turma[i]);
                            Thread.Sleep(200);
                        }
                    }
                    thread_ArduinoWemos.ReportProgress(1, "Compilando Arquivos (2/2)...");
                    //envia os terminais
                    for (int i = 0; i < terminal.Count; i++)
                    {
                        clientSock_clienteW.Send(terminal[i]);
                        //        Console.WriteLine(terminal[i][0] + " - " + terminal[i][1] + " - " + terminal[i][2] + " - " + terminal[i][3]);
                        Thread.Sleep(100);
                    }
                    thread_ArduinoWemos.ReportProgress(1, "sucesso! (2/2)...");
                    byte[] qtds = new byte[] { 113, 116, qtd_Terminais[0], qtd_Terminais[1], amount_User[0], amount_User[1] };
                    clientSock_clienteW.Send(qtds);
                    Thread.Sleep(100);
                    dtSimu = "waitStart";


                }


            }
            #endregion

            if (dtSimu == "waitStart")
            {
                btnSubmitDt.BeginInvoke((MethodInvoker)delegate { btnSubmitDt.Enabled = false; });
                btnRealTimeSimu.BeginInvoke((MethodInvoker)delegate { btnRealTimeSimu.Enabled = true; });
                btnFastSimu.BeginInvoke((MethodInvoker)delegate { btnFastSimu.Enabled = true; });
                log_gif.BeginInvoke((MethodInvoker)delegate { log_gif.Visible = false; });

            }
            while (dtSimu == "waitStart") //aguarda até mudar pra realtimer ou fastsimu
            { Thread.Sleep(1000); }

            #region realtimer_fastSimu
            //2/3 - get dt and app modifyers
            if (dtSimu == "realTimer")
            {

                while (dtSimu == "realTimer")
                {
                    int n = 0;
                    simuA[0] = new byte[] { 83, 115, dayW, 60, periodW, 0, turmW }; //entrada incial
                    simuA[1] = new byte[] { 83, 115, dayW, 61, periodW, 0, turmW }; // c.p
                    simuA[2] = new byte[] { 83, 115, dayW, 62, periodW, 1, turmW }; // saida intervalo
                    simuA[3] = new byte[] { 83, 115, dayW, 63, periodW, 1, turmW }; // volta intervalo
                    simuA[4] = new byte[] { 83, 115, dayW, 64, periodW, 2, turmW }; // c.p
                    simuA[5] = new byte[] { 83, 115, dayW, 62, periodW, 2, turmW }; // saida final

                    SimuStartLD(timeW++, clientSock_clienteW, 0);
                    int i = n;
                    while (rmsgSimu == "pause")
                    {
                        Thread.Sleep(1000);
                    }
                    //startSimu = "realTimer";


                }
            }
            else if (dtSimu == "fastSimu")
            {
                //gerador de dados fastSimu
                int n = 0;
                if (date_begin.Value.DayOfWeek.ToString("d") == "6" || date_begin.Value.DayOfWeek.ToString("d") == "0") dayW = 0;
                else { dayW = Convert.ToByte(date_begin.Value.DayOfWeek.ToString("d"));  dayW--; }
                simuA[0] = new byte[] { 83, 115, dayW, 60, periodW, 0, turmW }; //entrada incial
                simuA[1] = new byte[] { 83, 115, dayW, 61, periodW, 0, turmW }; // c.p
                simuA[2] = new byte[] { 83, 115, dayW, 62, periodW, 1, turmW }; // saida intervalo
                simuA[3] = new byte[] { 83, 115, dayW, 63, periodW, 1, turmW }; // volta intervalo
                simuA[4] = new byte[] { 83, 115, dayW, 64, periodW, 2, turmW }; // c.p
                simuA[5] = new byte[] { 83, 115, dayW, 62, periodW, 2, turmW }; // saida final
                                                                               // % falta
                calcu_Faltas(turmW);

                while (dtSimu == "fastSimu")
                {
                    // % erro
                    erroSistema(periodW);

                    // guarda dados
                    SimuStartLD(timeW++, clientSock_clienteW, 1);

                    for (int i = n; i < SD_listW.Count; i++)
                    {
                        string card = String.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}", SD_listW[i][5], SD_listW[i][6], SD_listW[i][7], SD_listW[i][8]);

                        string tm = String.Format("{0:D2}:{1:D2}:{2:D2}", SD_listW[i][0], SD_listW[i][1], SD_listW[i][2]);

                        int nt = int.Parse(String.Format("{0}", SD_listW[i][4]));
                        char c = Convert.ToChar(SD_listW[i][3]);

                        string term = String.Format("{0}.{1}.{2}.{3}", lstTerminais[nt].Ip[0], lstTerminais[nt].Ip[1], lstTerminais[nt].Ip[2], lstTerminais[nt].Ip[3]);
                        HistoricoTerminal ht = new HistoricoTerminal();
                        ht.Hora = new byte[] { SD_listW[i][0], SD_listW[i][1], SD_listW[i][2] };
                        ht.Card = new byte[] { SD_listW[i][5], SD_listW[i][6], SD_listW[i][7], SD_listW[i][8] };
                        ht.Tipo = SD_listW[i][3];
                        ht.Date = dateVar.Value.ToString("dd/MM/yyyy");
                        lstTerminais[nt].historicoTerminal.Add(ht);

                        if (dgv_HistoricoLog.InvokeRequired)
                        {
                            dgv_HistoricoLog.BeginInvoke((MethodInvoker)delegate
                            {
                                dgv_HistoricoLog.Rows.Add(tm, c, term, card, dateVar.Value.ToString("dd/MM/yyyy"), nt.ToString());
                            });
                        }
                    }

                    n = SD_listW.Count;
                    upToDB();

                    if (timeW > 5)
                    {
                        timeW = 0;
                        Console.WriteLine(timeW);
                        periodW++; turmW++;
                        if (periodW == cbGD_NumTurm.Items.Count)//entra aqui quando passar por todos os periodos do dia...
                        {
                            periodW = turmW = 0;
                            uploadDate(++d);
                            d = 0;
                            calcu_Faltas(turmW);
                        }
                        if (dateVar.Value.ToString("dd/MM/yyyy") == date_end.Value.ToString("dd/MM/yyyy"))
                        {
                            dtSimu = "stop"; //MUDAR PRA VERIFICAR OS 2 ARDUINOS
                        }
                        else
                        {
                            dateTime.BeginInvoke((MethodInvoker)delegate { dateTime.Text = dateVar.Value.ToString("dddd , dd/MM/yyyy"); });
                        }

                        simuA[0] = new byte[] { 83, 115, dayW, 60, periodW, 0, turmW }; //entrada incial
                        simuA[1] = new byte[] { 83, 115, dayW, 61, periodW, 0, turmW }; // c.p
                        simuA[2] = new byte[] { 83, 115, dayW, 62, periodW, 1, turmW }; // saida intervalo
                        simuA[3] = new byte[] { 83, 115, dayW, 63, periodW, 1, turmW }; // volta intervalo
                        simuA[4] = new byte[] { 83, 115, dayW, 64, periodW, 2, turmW }; // c.p
                        simuA[5] = new byte[] { 83, 115, dayW, 62, periodW, 2, turmW }; // saida final
                    }
                    else
                    {
                        dateTime.BeginInvoke((MethodInvoker)delegate { dateTime.Text = dateVar.Value.ToString("dddd , dd/MM/yyyy"); });
                    }

                }
                #endregion

            }
        }
        private void thread_ArduinoWemos_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                logTxt.BeginInvoke((MethodInvoker)delegate
                {
                    logTxt.Text = e.UserState.ToString();
                });
            }
        }

        private void thread_ArduinoWemos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
                logTxt.BeginInvoke((MethodInvoker)delegate
                {
                    logTxt.Text = "Aguardando inicio de simulação.";
                });

        }

        #region calculo de faltas percentual
        List<int> listNumbers = new List<int>();
        List<User> lstUserBlock = new List<User>();

        public void calcu_Faltas(int numTurm)
        {
            lstUserBlock.Clear();
            int Maxfaltas = 0;        
            this.Invoke((MethodInvoker)delegate()
            {
                Maxfaltas = int.Parse(cbPrc_faltas.Text.Substring(0, 2));
            });
            
                      
            Random rand = new Random();
            double nMaxTrm = double.Parse(String.Format("{0:X2}", amount_User[numTurm]));
            int qtt = int.Parse(String.Format("{0:X2}", amount_User[numTurm]));
            double percnt;
            // calcula um valor maximo para as faltas
            if (Maxfaltas == 0 || Maxfaltas == 10)
            {
                percnt = rand.Next(0, Maxfaltas); 
            } else if(Maxfaltas == 20)
            {
                percnt = rand.Next(15, Maxfaltas+5);
            }else
            {
                percnt = rand.Next(25, Maxfaltas+5);
            }
            
            int qtd = int.Parse(String.Format("{0}", Math.Round(nMaxTrm * percnt * 0.01)));
            Console.WriteLine(qtd);
            var select = user.Where(x => x.Ids_turma == (97 + numTurm).ToString())
                             .OrderBy(x => rand.Next(0, amount_User[numTurm]))
                             .Take(qtd)    
                             .ToList();

            lstUserBlock.AddRange(select);
        }
        #endregion
        #region calculo de erros do sistema
        List<int> lstNumTermi = new List<int>();
        List<Terminal> lstTerminErr = new List<Terminal>();


        public void erroSistema(int periodo)
        {
            lstTerminErr.Clear();
            int MaxErro = 0 ;
            this.Invoke((MethodInvoker)delegate()
            {
                MaxErro = int.Parse( cbPrc_falhasSistema.Text.Substring(0,2));
            });
            
            Random rand = new Random();
            double percnt;
            int ini;
            switch(MaxErro)
            {
                case 1:      ini = 0;      break;
                case 10:     ini = 7;      break;
                case 25:     ini = 19;     break;
                case 50:     ini = 40;     break;
                default:     ini = 0;      break;
            }
                percnt = rand.Next(ini, MaxErro);
            
            double nTerms = double.Parse(String.Format("{0}", qtd_Terminais[0])) + double.Parse(String.Format("{0}", qtd_Terminais[1]));
            int qtd = int.Parse(String.Format("{0}", qtd_Terminais[0]));
            int qtdParc = int.Parse(String.Format("{0}",Math.Round(nTerms * percnt * 0.01)));


            var select = lstTerminais.Where(x => x.TipoTerminal == "(C.A) ")
                                     .OrderBy(x => rand.Next(0, qtd))
                                     .Take(qtdParc)
                                     .ToList();
            lstTerminErr.AddRange(select);
            //var select = 
            //lstNumTermi.AddRange(Enumerable.Range(0,qtd).OrderBy( i => rand.Next()).Take(qtdParc));
            //for (int i = 0; i < lstNumTermi.Count; i++)
            //    lstTerminErr.Add(lstTerminais[lstNumTermi[i]]);


        }
        #endregion


        // salva os dados no banco de dados

        public void upToDB()
        {
            MySqlConnection conn = new MySqlConnection(cnnDB);
            try
            {
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "INSERT INTO historico_terminal (hora,data,tipo,cardUser,id_historico_terminal)VALUES";
                int m = dgv_HistoricoLog.Rows.Count - 1;
                for (int i = ct; i < m; i++)
                {
                    comm.CommandText += String.Format("(@hora{0},@data{1},@tipo{2},@cardUser{3},@id_historico_terminal{4})", i, i, i, i, i);
                  //  comm.Parameters.AddWithValue("@id_historico" + i.ToString(), i.ToString());
                    comm.Parameters.AddWithValue("@hora" + i.ToString(), dgv_HistoricoLog.Rows[i].Cells[0].Value.ToString());
                    comm.Parameters.AddWithValue("@data" + i.ToString(), dgv_HistoricoLog.Rows[i].Cells[4].Value.ToString());
                    comm.Parameters.AddWithValue("@tipo" + i.ToString(), dgv_HistoricoLog.Rows[i].Cells[1].Value.ToString());
                    comm.Parameters.AddWithValue("@cardUser" + i.ToString(), dgv_HistoricoLog.Rows[i].Cells[3].Value.ToString());
                    comm.Parameters.AddWithValue("@id_historico_terminal" + i.ToString(), dgv_HistoricoLog.Rows[i].Cells[5].Value.ToString());


                    if (i == m - 1)
                    {
                        comm.CommandText += ";";
                    }
                    else
                    {
                        comm.CommandText += ",";
                    }

                }
              //  Console.WriteLine(comm.CommandText);
                ct = m;
                comm.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySqlException er)
            {
                MessageBox.Show("Error:" + er.ToString());
                Console.WriteLine(er.ErrorCode);
                Console.WriteLine(er.Message);
                Console.WriteLine(er.Source);
                Console.WriteLine(er.StackTrace);
            }

        }

        private void bw_CPU_PCSIMULADOR_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void bw_CPU_PCSIMULADOR_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                logTxt.BeginInvoke((MethodInvoker)delegate
                {
                    logTxt.Text = e.UserState.ToString();
                });
            }
        }

        private void bw_CPU_PCSIMULADOR_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Mn_InfoSimu_Click(object sender, EventArgs e)
        {
            FrmInfo frm = new FrmInfo();
            frm.ShowDialog();
        }

        private void mnIconsFlow_mn_Click(object sender, EventArgs e)
        {
            menu_iconsflow.Visible = !menu_iconsflow.Visible;
            if (!menu_iconsflow.Visible)
                flpnl_main.Dock = DockStyle.Fill;
            else
            {
                flpnl_main.Dock = DockStyle.None;
                flpnl_main.Size = new Size(this.Width, this.Height);
            }
        }

        private void File_Resetar_mn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();

            this.InitializeComponent();
            btnDataBase.BackColor = btnConnEthernet.BackColor = btnConnWifi.BackColor = btnLOAD2DB.BackColor = btnVincularDB.BackColor = btnArduino.BackColor = btnDB_conn.BackColor = Color.Red;
            date_begin.CustomFormat = "dddd dd/MM/yyyy";
            date_end.CustomFormat = "dd/MM/yyyy";


        }


        public void SimuStartLD(byte time, Socket client, int tp)
        {
            client.Send(simuA[time]);
            total_listE = 0;
            total_listW =  0;
            while (client.Available == 0)
            {
                Thread.Sleep(200);
            }
            while (client.Available > 0)
            {
                //012345678
                //HMSXTCARD
                /// hora : minuto  :segundo - tipo entrada - numero terminal - cartão
                Thread.Sleep(200);
                client.Receive(inStream, 9, SocketFlags.None);

                //arrumar isso aqui...
                
                bool uBlk = false;
                for (int i = 0; i < lstUserBlock.Count; i++)
                {
                    if (lstUserBlock[i].Cardb[0] == inStream[5] &&
                        lstUserBlock[i].Cardb[1] == inStream[6] &&
                        lstUserBlock[i].Cardb[2] == inStream[7] &&
                        lstUserBlock[i].Cardb[3] == inStream[8])
                    {
                        uBlk = true;
                        break;
                    }
                    else
                        uBlk = false;
                }
                if (inStream[3] != 80)
                {
                    for (int n = 0; n < lstTerminErr.Count; n++)
                    {

                        if (lstTerminErr[n].IdTerminal == inStream[4])
                        {
                            inStream[3] = 88; break;
                        }
                    }
                }

                if (tp == 0)
                {                  
                    if (uBlk == false)
                    {
                        SD_listE.Add(new byte[] { inStream[0], inStream[1], inStream[2], inStream[3], inStream[4], inStream[5], inStream[6], inStream[7], inStream[8] });
                        total_listE++;
                    }
                }
                else if(tp == 1)
                {
                    if (uBlk == false)
                    {
                        SD_listW.Add(new byte[] { inStream[0], inStream[1], inStream[2], inStream[3], inStream[4], inStream[5], inStream[6], inStream[7], inStream[8] });
                        //Console.WriteLine(SD_listE[total_listE][0] + ":" + SD_listE[total_listE][1] + ":" + SD_listE[total_listE][2] + " " + SD_listE[total_listE][3]);            
                        total_listW++;
                    }
                }

            }
            if (startSimu == "realTimer") { rmsgSimu = "pause"; }
        }



        private void tbNmTpAcss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbNmTpAcss.SelectedIndex == 1)
            {
                cbnTurm.Enabled = cbPeriodo.Enabled = true;
                tb_qtdListTerm.Text = "5";
            }
            else
            {
                cbnTurm.Enabled = cbPeriodo.Enabled = false;

            }
        }








        #endregion
        #region Metodos

        private void vpnl_in_pnl(Control pnl) //verifica se existe painel em painelflow!
        {
            if (flpnl_main.Controls.Contains(pnl))
            {
                flpnl_main.Controls.Remove(pnl);
            }
            else
            { flpnl_main.Controls.Add(pnl); }
        }

        //public void gerarFaltas(int amUs)
        //{
        //    Random rd = new Random();
        //    double qtd = 100 - rd.Next(70, 100);
        //    qtd = qtd / 100;
        //    // Console.WriteLine(qtd);
        //    qtd = Math.Round(qtd * amUs);
        //     Console.WriteLine("qtd :: "+ qtd);
        //    Shuffle(amUs);
        //    for(int i = 0; i < qtd;i++)
        //    {
        //        Console.WriteLine(user[ numeros[i]].Cards);
        //    }


        //}

        public void Shuffle(int qtd)
        {
            Random rd = new Random();
            for (int i = 0; i < qtd; i++)
            {
                int tmp = numeros[i];
                int novoIndice = rd.Next(qtd);
                numeros[i] = numeros[novoIndice];
                numeros[novoIndice] = tmp;
            }
        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        /// <sobre> checa se o valor digitado é maior que 255 se for maior retorna 255</sobre>

        public void CheckIPs(TextBox tbip, EventArgs e)
        {
            int i = 0;
            try
            {
                i = int.Parse(tbip.Text);
                if (i > 255)
                    tbip.Text = "255";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                tbip.Text = "";
            }
        }

        private void cbGD_NumTurm_SelectedIndexChanged(object sender, EventArgs e)
        {

            int tk = 0;
            int i = 0;
            for (; i < (dgvDtSimu.Rows.Count - 1); i++)
            {
                if (cbGD_NumTurm.Text == dgvDtSimu.Rows[i].Cells[2].Value.ToString())
                {
                    tk++;
                }
            }
            Console.WriteLine(tk);
            cbGD_QtdUsers.Items.Clear();
            for (i = 0; i < (40 - tk); i++)
            {
                cbGD_QtdUsers.Items.Add(i);
            }

        }



        private void frm_principal_FormClosed(object sender, FormClosedEventArgs e) //encerra conexões com arduinos e banco de dados!
        {
            try
            {
                if (clientSock_clienteW.Connected)
                {
                    clientSock_clienteW.Close();
                }

                if (clientSock_clienteE.Connected)
                {
                    clientSock_clienteE.Close();
                }
            }
            catch { }
        }

        private void cbGD_QtdUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnClearDB_Click(object sender, EventArgs e)
        {
            tbDB_Server.Text = "localhost";
            tbDB_db.Text = "testesimulador";
            tbDB_User.Text = "root";
            tbDB_Pass.Text = "";
        }
        #region ARDUINO UNO
        private void bgArduinoUno_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void bgAUnoLoad_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

        private void bgAUnoLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        #endregion
        #region ARDUINO WEMOS



        


        //bool realTime = false;
        byte segundo, minuto, hora;

        private void btnGDCard_Click(object sender, EventArgs e)
        {
            cardU = !cardU;
            if(cardU == true)
            {
                btnGDCard.BackColor = Color.Lime;
            }
            else
            {
                btnGDCard.BackColor = Color.FromArgb(0, 122, 204);
            }
        }

        private void btnGDNameUser_Click(object sender, EventArgs e)
        {
            nameU = !nameU;
            if(nameU == true)
            {
                btnGDNameUser.BackColor = Color.Lime;
            }
            else
            {
                btnGDNameUser.BackColor = Color.FromArgb(0, 122, 204);
            }
        }

        private void btnGDupdateDB_Click(object sender, EventArgs e)
        {

            MySqlConnection conn = new MySqlConnection(cnnDB);
            try
            {
                conn.Open();
                MySqlCommand comm = conn.CreateCommand();
                comm.CommandText = "UPDATE aluno SET cardUser = CASE idALUNO ";
                for(int i = 0; i < user.Count;i++)
                {
                    comm.CommandText += String.Format("WHEN @idUser{0} THEN @cardUser{1} ",i,i);
                    comm.Parameters.AddWithValue("@idUser" + i.ToString(), user[i].IdUser);
                    comm.Parameters.AddWithValue("@cardUser" + i.ToString(), user[i].Cards);                
                }
                comm.CommandText += "END";
                comm.ExecuteNonQuery();
                conn.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error ao importar :  \nMensagem do Banco de Dados ::\n" + ex.Message,
                    "ERRO AO IMPORTAR PARA O BANCO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        private void bgAUnoSimu_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (btnRealTimeSimu.InvokeRequired)
            {
                btnRealTimeSimu.BeginInvoke((MethodInvoker)delegate
                {
                    btnRealTimeSimu.Enabled = true;
                    btnStop.Enabled = false;
                });
            }
            
            
        }

        int total_listE, total_listW;


       

/*
        private void bgAUnoSimu_DoWork(object sender, DoWorkEventArgs e)
        {
            int n = 0;
            simuA[0] = new byte[] { 83, 115, day, 60, periodE, 0, turmE }; //entrada incial
            simuA[1] = new byte[] { 83, 115, day, 61, periodE, 0, turmE }; // c.p
            simuA[2] = new byte[] { 83, 115, day, 62, periodE, 1, turmE }; // saida intervalo
            simuA[3] = new byte[] { 83, 115, day, 63, periodE, 1, turmE }; // volta intervalo
            simuA[4] = new byte[] { 83, 115, day, 64, periodE, 2, turmE }; // c.p
            simuA[5] = new byte[] { 83, 115, day, 62, periodE, 2, turmE }; // saida final
            while (startSimu == "snda")
            { Thread.Sleep(1000); }
            while (startSimu == "realTimer")
            {          
                SimuStartLD(timeE++, clientSock_clienteE,0);
                int i = n; 
                while(rmsgSimu == "pause")
                {
                    Thread.Sleep(1000);
                }
                startSimu = "realTimer";
                
            }
            while (startSimu == "fastSimu")
            {
   
                SimuStartLD(timeE++, clientSock_clienteE,0);



                for (int i = n; i < SD_listE.Count; i++)
                {
                    string card = String.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}", SD_listE[i][5], SD_listE[i][6], SD_listE[i][7], SD_listE[i][8]);
                    
                    string tm = String.Format("{0:D2}:{1:D2}:{2:D2}", SD_listE[i][0], SD_listE[i][1], SD_listE[i][2]);
                    
                    int nt = int.Parse(String.Format("{0}", SD_listE[i][4]));
                    char c = Convert.ToChar(SD_listE[i][3]);

                    string term = String.Format("{0}.{1}.{2}.{3}", lstTerminais[nt].Ip[0], lstTerminais[nt].Ip[1], lstTerminais[nt].Ip[2], lstTerminais[nt].Ip[3]);
                    HistoricoTerminal ht = new HistoricoTerminal();
                    ht.Hora = new byte[] { SD_listE[i][0], SD_listE[i][1], SD_listE[i][2] };
                    ht.Card = new byte[] { SD_listE[i][5], SD_listE[i][6], SD_listE[i][7], SD_listE[i][8] };
                    ht.Tipo = SD_listE[i][3];
                    ht.Date = dateVar.Value.ToString("dd/MM/yyyy");
                    lstTerminais[nt].historicoTerminal.Add(ht);

                    if (dgv_HistoricoLog.InvokeRequired)
                    {
                        dgv_HistoricoLog.BeginInvoke((MethodInvoker)delegate
                        {
                            dgv_HistoricoLog.Rows.Add(tm, c, term, card, dateVar.Value.ToString("dd/MM/yyyy"),nt.ToString());
                        });
                    }
                    //Thread.Sleep(100); ?? pq time aqui??

                }
                n = SD_listE.Count;
                upToDB();

                if (timeE > 5)
                {
                    timeE = 0;
                    periodE++; turmE++;
                    if (periodE == cbGD_NumTurm.Items.Count)
                    {
                        day++;
                        periodE = turmE = 0;
                        uploadDate(++d);
                        d = 0;
                    }
                    if (dateVar.Value.ToString("dd-MM-yyyy") == date_end.Value.ToString("dd-MM-yyyy"))
                    {
                        startSimu = "stop";
                    }
                    simuA[0] = new byte[] { 83, 115, day, 60, periodE, 0, turmE }; //entrada incial
                    simuA[1] = new byte[] { 83, 115, day, 61, periodE, 0, turmE }; // c.p
                    simuA[2] = new byte[] { 83, 115, day, 62, periodE, 1, turmE }; // saida intervalo
                    simuA[3] = new byte[] { 83, 115, day, 63, periodE, 1, turmE }; // volta intervalo
                    simuA[4] = new byte[] { 83, 115, day, 64, periodE, 2, turmE }; // c.p
                    simuA[5] = new byte[] { 83, 115, day, 62, periodE, 2, turmE }; // saida final
                }
            }
            
        }





        //    string tm, card, term; int nt; char c;
        /*
        private void bgAUnoSimu_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }
        
        private void bgAWemosSimu_DoWork(object sender, DoWorkEventArgs e)
        {
            int n = 0;
            simuA[0] = new byte[] { 83, 115, day, 60, periodE, 0, turmE }; //entrada incial
            simuA[1] = new byte[] { 83, 115, day, 61, periodE, 0, turmE }; // c.p
            simuA[2] = new byte[] { 83, 115, day, 62, periodE, 1, turmE }; // saida intervalo
            simuA[3] = new byte[] { 83, 115, day, 63, periodE, 1, turmE }; // volta intervalo
            simuA[4] = new byte[] { 83, 115, day, 64, periodE, 2, turmE }; // c.p
            simuA[5] = new byte[] { 83, 115, day, 62, periodE, 2, turmE }; // saida final
            while (startSimu == "snda")
            { Thread.Sleep(1000); }
            while (startSimu == "realTimer")
            {
                if (xE == total_listE)
                {
                    SimuStartLD(timeE++, clientSock_clienteE, 0);
                    realTime = true;
                }
            }
            while (startSimu == "fastSimu")
            {

                if (timeW > 5)
                {
                    timeW = 0;
                    periodW++; turmW++;
                    if (periodW > 1)
                    {
                        day++;
                        periodW = turmW = 0;
                        uploadDate(++d);
                        d = 0;
                    }
                    if (dateVar.Value.ToString("dd-MM-yyyy") == date_end.Value.ToString("dd-MM-yyyy"))
                    {
                        startSimu = "stop";
                    }
                    simuA[0] = new byte[] { 83, 115, day, 60, periodW, 0, turmW }; //entrada incial
                    simuA[1] = new byte[] { 83, 115, day, 61, periodW, 0, turmW }; // c.p
                    simuA[2] = new byte[] { 83, 115, day, 62, periodW, 1, turmW }; // saida intervalo
                    simuA[3] = new byte[] { 83, 115, day, 63, periodW, 1, turmW }; // volta intervalo
                    simuA[4] = new byte[] { 83, 115, day, 64, periodW, 2, turmW }; // c.p
                    simuA[5] = new byte[] { 83, 115, day, 62, periodW, 2, turmW }; // saida final
                }


                SimuStartLD(timeW++, clientSock_clienteW, 0);

                for (int i = n; i < SD_listW.Count; i++)
                {
                    string tm = String.Format("{0:D2}:{1:D2}:{2:D2}", SD_listW[i][0], SD_listW[i][1], SD_listW[i][2]);
                    string card = String.Format("{0:X2}-{1:X2}-{2:X2}-{3:X2}", SD_listW[i][5], SD_listW[i][6], SD_listW[i][7], SD_listW[i][8]);
                    int nt = int.Parse(String.Format("{0}", SD_listW[i][4]));
                    char c = Convert.ToChar(SD_listW[i][3]);
                    string term = String.Format("{0}.{1}.{2}.{3}", lstTerminais[nt].Ip[0], lstTerminais[nt].Ip[1], lstTerminais[nt].Ip[2], lstTerminais[nt].Ip[3]);
                    HistoricoTerminal ht = new HistoricoTerminal();
                    ht.Hora = new byte[] { SD_listW[i][0], SD_listW[i][1], SD_listW[i][2] };
                    ht.Card = new byte[] { SD_listW[i][5], SD_listW[i][6], SD_listW[i][7], SD_listW[i][8] };
                    ht.Tipo = SD_listW[i][3];
                    ht.Date = dateVar.Value.ToString("dd-MM-yyyy");
                    lstTerminais[nt].historicoTerminal.Add(ht);

                    if (dgv_HistoricoLog.InvokeRequired)
                    {
                        dgv_HistoricoLog.BeginInvoke((MethodInvoker)delegate
                        {
                            dgv_HistoricoLog.Rows.Add(tm, c, term, card, dateVar.Value.ToString("dd-MM-yyyy"));
                        });
                    }
                    Thread.Sleep(100);

                }
                n = SD_listW.Count;
            }
        }
        */
        #endregion

        public byte[] strtoBt(string ip)  //convert string para byteArray
        {
            byte[] a = { 0, 0, 0, 0 };
            string x;
            int i = 0;
            int f = ip.IndexOf('.');
            int count = ip.IndexOf('.');
            //3 2 3 . 1 2 3 4 . 1 2  3 . 1234
            //0 1 2 3 4 5 6 7 8 9 10
            for (int n = 0; n <= 3; n++)
            {
                if (n <= 2)
                {
                    x = ip.Substring(i, count);
                    if (int.Parse(x) > 255)
                    {
                        x = (255).ToString();
                    }
                }
                else
                {
                    x = ip.Substring(i);
                }
                i = ++f;
                f = ip.IndexOf('.', i);
                count = ip.IndexOf('.', i) - i;

                a[n] = byte.Parse(x);

            }


            return a;
        }

        #endregion
    }
}

/*
 * 
pra colocar a turma 12
UPDATE sala    INNER JOIN disciplina 
ON sala.Diadasemana = disciplina.Diadasemana 
SET sala.nomeDisciplina = disciplina.nm_disciplina , sala.idDisciplina = disciplina.idDISCIPLINA,sala.Disponibilidade = 'NAO' 
WHERE disciplina.idCurso = 147 
AND sala.perido_aula = 'Noturno' 
AND sala.Disponibilidade = 'SIM' 
AND sala.num_sala = 10 
AND sala.bloco_sala = 'B'

para retirar a turma 12
UPDATE sala SET 
sala.idDisciplina = NULL,
sala.Disponibilidade = 'SIM',
sala.nomeDisciplina = 'A definir'
WHERE sala.num_sala = 10
AND sala.bloco_sala = 'B'
 */


/*
               if (ckb_manha.Checked == true)
               {
                   hrM = HEM_H.Text + HEM_Min.Text + HIM_H.Text + HIM_Min.Text + HSM_H.Text + HSM_Min.Text;
               }
               else hrM = "000000000000";
               if (ckb_tarde.Checked == true)
               {
                   hrT = HET_H.Text + HET_Min.Text + HIT_H.Text + HIT_Min.Text + HST_H.Text + HST_Min.Text;
               }
               else hrT = "000000000000";
               if (ckb_noite.Checked == true)
               {
                   hrN = HEN_H.Text + HEN_Min.Text + HIN_H.Text + HIN_Min.Text + HSN_H.Text + HSN_Min.Text;
               }
               else hrN = "000000000000";*/
