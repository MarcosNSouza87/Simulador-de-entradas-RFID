using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj_Simulador_B01
{
    class Terminal
    {
        private byte[] ip;
        private string tipoTerminal;
        private string bloco;
        private string num_sala;
        private byte idTerminal;
        
        public List<HistoricoTerminal> historicoTerminal;

        public byte[] Ip
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
            }
        }

        public string TipoTerminal
        {
            get
            {
                return tipoTerminal;
            }

            set
            {
                tipoTerminal = value;
            }
        }

        public string Bloco
        {
            get
            {
                return bloco;
            }

            set
            {
                bloco = value;
            }
        }

        public string Num_sala
        {
            get
            {
                return num_sala;
            }

            set
            {
                num_sala = value;
            }
        }

        public byte IdTerminal
        {
            get
            {
                return idTerminal;
            }

            set
            {
                idTerminal = value;
            }
        }

        public Terminal()
        {
            historicoTerminal = new List<HistoricoTerminal>();
        }
        public void addOnHistory(byte[] item) //adicionar no histórico do terminal
        {

        }
        public void mostrarHistorico()
        {

        }
        public string getIPString()
        {
            string myIPstring = String.Format("{0:D3}.{1:D3}.{2:D3}.{3:D3}", ip[0], ip[1], ip[2], ip[3]);
            return myIPstring;
        }

    }
}
