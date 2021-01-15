using System;
using System.Diagnostics;
using System.Linq;

namespace CE
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var sw = new Stopwatch();
            var rand = new Random();
            var currState = game.State;
            for(int i = 0; i < 10; i++){
                var states = currState.GetAllStates();
                currState = states.ElementAt(rand.Next(0, states.Count));
                currState.DisplayBoard();
                Console.WriteLine();
            }
            // sw.Start();
            // for(int i = 0; i < 100000; i++){
            //     currState.GetAllStates();
            // }
            // Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
