using System;
using System.Diagnostics;
using System.Linq;

namespace CE
{
    class Program
    {
        static void Main(string[] args)
        {
            //MainGame();

            //RealGame();
            TestSpeed();
        }

        private static void MainGame()
        {
            var depth = 6;
            //var state = new State("r1bqkb1r/pppp1ppp/2n2n2/3Pp3/2P1P3/2N5/PP3PPP/R1BQKBNR b KQkq - 0 1");

            var state = new State("r1b1k1nr/ppp1qppp/8/3p4/1b6/2NB1Q2/PPPB1PPP/R3K2R w KQkq - 1 9");

            var moveConverter = new MoveConverter();
            var moves = state.GetAllStates().Select(p => moveConverter.MoveToString(p.LastMove)).ToList();
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var eval = ai.Minimax(state, depth--, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            Console.WriteLine(moveConverter.MoveToString(eval.Item2.LastMove) + " " + eval.Item1);

            // while (depth >= 0)
            // {
            //     state = eval.Item2;
            //     Console.WriteLine(moveConverter.MoveToString(eval.Item2.LastMove) + " " + eval.Item1);
            //     eval = ai.Minimax(state, depth--, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            // }
        }

        private static void RealGame(){
            var player = Console.ReadLine();
            Console.WriteLine("You've typed " + player);
            var playersWhite = player.Contains('1') ? true : false;

            var state = new State();
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var moveConverter = new MoveConverter();
            var depth = 5;
            var moveCounter = 0;

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
                    var eval = ai.Minimax(state, depth, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
                    Console.WriteLine(eval.Item1 + " " + moveConverter.MoveToString(eval.Item2.LastMove));
                    state = eval.Item2;
                }
                moveCounter++;
                if(moveCounter > 15)
                {
                    depth = 6;
                }
            }
        }

        private static void TestSpeed(){
            var sw = new Stopwatch();
            var state = new State("r1bqkb1r/pppp1ppp/2n2n2/3Pp3/2P1P3/2N5/PP3PPP/R1BQKBNR b KQkq - 0 1");
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var iterations = 1;
            sw.Start();
            for(int i = 0; i < iterations; i++){   
                var eval = ai.Minimax(state, 6, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            }
            Console.WriteLine(sw.ElapsedMilliseconds / iterations);
            sw.Stop();
        }
    }
}
