using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    class CommunicationBase
    {
        #region Variables privées
        private static bool _handshake = false;
        #endregion

        #region Propriétés
        /// <summary>
        /// Permet de savoir si la première communication est fait
        /// </summary>
        public static bool Handshake
        {
            get
            {
                return _handshake;
            }
            set
            {
                _handshake = value;
            }
        }
        #endregion

        #region Méthodes publiques
        

        #endregion
    }
}
