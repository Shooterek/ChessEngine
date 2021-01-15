using System;
using System.Diagnostics;
using System.Linq;

namespace CE
{
    class Program
    {
        static void Main(string[] args)
        {
            // MainGame();

            FenTest();
        }

        private static void FenTest()
        {
            var state = new State("k7/pp6/pp6/8/8/PP6/PP6/K7 w");
            // var state = new State("3K4/5k2/8/1B3b2/1P2p3/4P2p/8/2r5 w");
            var states = state.GetAllStates();
            Console.WriteLine(state.GetFen());
        }

        private static void MainGame()
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
