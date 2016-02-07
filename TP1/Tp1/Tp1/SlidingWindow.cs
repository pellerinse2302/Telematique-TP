using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    public class SlidingWindow
    {
        #region Variables privées
        private Queue<Packet> _window = null;
        #endregion

        #region Constructeur
        public SlidingWindow()
        {
            
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Représente la window de packet
        /// </summary>
        public Queue<Packet> Window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
            }
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// Fait avancer la fenêtre
        /// </summary>
        public void Forward()
        {
            Window.Dequeue();
        }

        /// <summary>
        /// Permet d'avancer et d'ajouter un packet dans la window
        /// </summary>
        /// <param name="nxtPkt"></param>
        public void ForwardAndAddPacket(Packet nxtPkt)
        {
            Forward();
            InsertPacket(nxtPkt);
        }

        /// <summary>
        /// Permet d'ajouter un packet dans la fenêtre
        /// </summary>
        /// <param name="pkt"></param>
        public void InsertPacket(Packet pkt)
        {
            Window.Enqueue(pkt);
        }
        #endregion
    }
}
