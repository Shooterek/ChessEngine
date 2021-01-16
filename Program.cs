using System;
using System.Diagnostics;
using System.Linq;

namespace CE
{
    class Program
    {
        static void Main(string[] args)
        {
            MainGame();

            // FenTest();
            // RealGame();
            //TestSpeed();
        }

        private static void FenTest()
        {
            var state = new State("r3k2r/3ppp2/8/8/8/3n4/3PPP2/R3K1R1 w");
            // var state = new State("3K4/5k2/8/1B3b2/1P2p3/4P2p/8/2r5 w");
            var states = state.GetAllStates();
            Console.WriteLine(state.GetFen());
        }

        private static void MainGame()
        {
            var state = new State("r1bqkb1r/pppp1ppp/2n2n2/3Pp3/2P1P3/2N5/PP3PPP/R1BQKBNR b");
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var eval = ai.Minimax(state, 3, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            var moveConverter = new MoveConverter();
            Console.WriteLine(moveConverter.MoveToString(eval.Item2.LastMove));
        }

        private static void RealGame(){
            var player = Console.ReadLine();
            Console.WriteLine("You've typed " + player);
            var playersWhite = player.Contains('1') ? true : false;

            var state = new State();
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var moveConverter = new MoveConverter();

            while(true){
                if(state.WhiteToMove == playersWhite){
                    var m = Console.ReadLine();
                    if(m.Contains("res")){
                        break;
                    }
                    var move = moveConverter.StringToMove(m);
                    var allStates = state.GetAllStates();
                    var x = allStates.Select(x => x.LastMove).ToList();
                    if(move.WasLongCastling){
                        state = allStates.First(s => s.LastMove.WasLongCastling);
                    }
                    if(move.WasShortCastling){
                        state = allStates.First(s => s.LastMove.WasShortCastling);
                    }
                    else{
                        state = allStates.First(s => s.LastMove.DestinationFile == move.DestinationFile && s.LastMove.DestinationLine == move.DestinationLine 
                            && s.LastMove.StartingLine == move.StartingLine && s.LastMove.StartingFile == move.StartingFile);
                    }
                }
                else{
                    var eval = ai.Minimax(state, 5, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
                    Console.WriteLine(eval.Item1 + " " + moveConverter.MoveToString(eval.Item2.LastMove));
                    state = eval.Item2;
                }
            }
        }

        private static void TestSpeed(){
            var sw = new Stopwatch();
            var state = new State("rn1qkbnr/ppp1p1p1/5p1p/3p1b2/1P1P4/P1P1P3/5PPP/RNBQKBNR b");
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var iterations = 1;
            sw.Start();
            for(int i = 0; i < iterations; i++){   
                var eval = ai.Minimax(state, 5, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            }
            Console.WriteLine(sw.ElapsedMilliseconds / iterations);
        }
    }
}
