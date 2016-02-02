using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    public class Packet
    {
        #region Variables privées
        private Int32 _sequenceNumber;
        private Int32 _ackNumber;
        private bool _fin;
        private bool _sor;
        private Int16 _dataLength;
        private Byte[] _data;
        #endregion

        #region Constructeur
        public Packet(Int32 sequenceNumber, Int32 ackNumber, bool fin, bool sor, Int16 dataLength, Byte[] data)
        {
            this.SequenceNumber = sequenceNumber;
            this.AckNumber = ackNumber;
            this.FIN = fin;
            this.SOR = sor;
            this.DataLength = dataLength;
            this.DATA = data;
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Le numéro de séquence
        /// </summary>
        public Int32 SequenceNumber
        {
            get
            {
                return _sequenceNumber;
            }
            set
            {
                _sequenceNumber = value;
            }
        }

        /// <summary>
        /// Le numero de sequence ack
        /// </summary>
        public Int32 AckNumber
        {
            get
            {
                return _ackNumber;
            }
            set
            {
                _ackNumber = value;
            }
        }

        /// <summary>
        /// Signifie si c'est le dernier packet
        /// </summary>
        public bool FIN
        {
            get
            {
                return _fin;
            }
            set
            {
                _fin = value;
            }
        }

        /// <summary>
        /// Signifie si on doit se mettre en Send ou Receive
        /// </summary>
        public bool SOR
        {
            get
            {
                return _sor;
            }
            set
            {
                _sor = value;
            }
        }

        /// <summary>
        /// Représente la longueur du DATA dans le packet
        /// </summary>
        public Int16 DataLength
        {
            get
            {
                return _dataLength;
            }
            set
            {
                _dataLength = value;
            }
        }

        /// <summary>
        /// Représente le DATA
        /// </summary>
        public Byte[] DATA
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Permet de récupérer le packet
        /// </summary>
        public Byte[] BuildPacket()
        {
            Byte[] packet = new Byte[1036];
            packet[0] = Convert.ToByte(SequenceNumber);
            packet[4] = Convert.ToByte(AckNumber);
            packet[8] = Convert.ToByte(FIN);
            packet[9] = Convert.ToByte(SOR);
            packet[10] = Convert.ToByte(DataLength);
            packet[12] = Convert.ToByte(DATA);
            return packet;
        }
        #endregion
    }
}
