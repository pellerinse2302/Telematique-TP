using System;
using System.Collections.Generic;

namespace Tp1
{
  public class SlidingWindow
  {
    #region Variables privées
    private Queue<Packet> _window = null;
    private int _lastAck = 0;
    #endregion

    #region Constructeur
    /// <summary>
    /// Constructeur par défaut
    /// </summary>
    public SlidingWindow()
    {
      this.Window = new Queue<Packet>();
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

    /// <summary>
    /// Permet de savoir le dernier ack
    /// </summary>
    public int LastAck
    {
      get
      {
        return _lastAck;
      }
      set
      {
        _lastAck = value;
      }
    }
    /// <summary>
    /// Permet de savoir si on peut continuer à avancer la fenêtre
    /// </summary>
    public bool CanForward
    {
      get
      {
        return Window.Count < 4;
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
