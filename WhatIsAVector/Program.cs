﻿using System;

namespace WhatIsAVector
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using var game = new Game1();
            game.Run();
        }
    }
}
