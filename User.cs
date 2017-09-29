using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj_Simulador_B01
{
    class User
    {
        private byte[] cardb;
        private string cards;
        private string nome;
        private string ids_turma;
        private string idUser;
 
        public string Nome
        {
            get
            {
                return nome;
            }

            set
            {
                nome = value;
            }
        }
        public string Ids_turma
        {
            get
            {
                return ids_turma;
            }

            set
            {
                ids_turma = value;
            }
        }

        public byte[] Cardb
        {
            get
            {
                return cardb;
            }

            set
            {
                cardb = value;
            }
        }
        public string Cards
        {
            get
            {
                return cards;
            }

            set
            {
                cards = value;
            }
        }

        public string IdUser
        {
            get
            {
                return idUser;
            }

            set
            {
                idUser = value;
            }
        }

        public User()
        {
            cardb = new byte[4];
        }
        public string getNome(byte[] _card)
        {
            if (_card == cardb)
                return nome;
            else
                return "0";
        }
    }
}
