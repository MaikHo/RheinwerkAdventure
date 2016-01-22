﻿using System;

namespace RheinwerkAdventure.Model
{
    /// <summary>
    /// Interface für alle angreifbaren Spielelemente
    /// </summary>
    internal interface IAttackable
    {
        /// <summary>
        /// Maximale Anzahl Trefferpunkte im gesunden Zustand.
        /// </summary>
        int MaxHitpoints { get; }

        /// <summary>
        /// Anzahl verfügbarer Trefferpunkte.
        /// </summary>
        int Hitpoints { get; set; }

        /// <summary>
        /// Aufruf bei ankommenden Treffern.
        /// </summary>
        Action<RheinwerkGame, IAttacker, IAttackable> OnHit { get; }
    }
}

