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
            Byte[][] packet = new Byte[6][];
            packet[0] = BitConverter.GetBytes(SequenceNumber);
            packet[1] = BitConverter.GetBytes(AckNumber);
            packet[2] = BitConverter.GetBytes(FIN);
            packet[3] = BitConverter.GetBytes(SOR);
            packet[4] = BitConverter.GetBytes(DataLength);
            packet[5] = DATA;

            return Combine(packet);
        }

        /// <summary>
        /// Permet de défaire le packet
        /// </summary>
        public void UnbuildPacket(Byte[] packet)
        {
            SequenceNumber = BitConverter.ToInt32(SubArray(packet, 0, 4), 0);
            AckNumber = BitConverter.ToInt32(SubArray(packet, 4, 4), 0);
            FIN = BitConverter.ToBoolean(SubArray(packet, 8, 1), 0);
            SOR = BitConverter.ToBoolean(SubArray(packet, 9, 1), 0);
            DataLength = BitConverter.ToInt16(SubArray(packet, 10, 2), 0);
            DATA = SubArray(packet, 12, DataLength);
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] ret = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;
            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, ret, offset, data.Length);
                offset += data.Length;
            }
            return ret;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        #endregion
    }
}
