using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj_Simulador_B01
{
    class HistoricoTerminal
    {
        private byte[] hora;
        private string date;
        private byte tipo;
        private byte[] card;

        public byte[] Hora
        {
            get
            {
                return hora;
            }

            set
            {
                hora = value;
            }
        }

        public byte[] Card
        {
            get
            {
                return card;
            }

            set
            {
                card = value;
            }
        }
        public byte Tipo
        {
            get
            {
                return tipo;
            }

            set
            {
                tipo = value;
            }
        }

        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        public HistoricoTerminal()
        {
            Hora = new byte[3];
            Card = new byte[4];
        }
        
    }
}
