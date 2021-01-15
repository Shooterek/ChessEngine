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
            while(true){
                var states = currState.GetAllStates();
                Console.WriteLine(currState.GetFen());
                if(states.Count == 0 || currState.PieceLookup.Count < 4){
                    break;
                }
                // currState = states.First();
                currState = states.ElementAt(rand.Next(0, states.Count));
                // currState.DisplayBoard();
                Console.WriteLine();
            }
            // sw.Start();
            // for(int i = 0; i < 10000; i++){
            //     currState.GetAllStates();
            // }
            // Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
