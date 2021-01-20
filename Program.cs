using System;
using System.Diagnostics;
using System.Linq;

namespace CE
{
    class Program
    {
        //TODO inculde en passant support and checkmate scoring.
        static void Main(string[] args)
        {
            //MainGame();

            RealGame();
            //TestSpeed();
        }

        private static void MainGame()
        {
            var depth = 6;
            var state = new State("r1b1k1nr/ppp1qppp/8/3p4/1b6/2NB1Q2/PPPB1PPP/R3K2R w KQkq - 1 9");

            var moveConverter = new MoveConverter();
            var moves = state.GetAllStates().Select(p => moveConverter.MoveToString(p.LastMove)).ToList();
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var eval = ai.Minimax(state, depth--, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            Console.WriteLine(moveConverter.MoveToString(eval.Item2.LastMove) + " " + eval.Item1);

            while (depth >= 0)
            {
                state = eval.Item2;
                Console.WriteLine(moveConverter.MoveToString(eval.Item2.LastMove) + " " + eval.Item1);
                eval = ai.Minimax(state, depth--, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            }
        }

        private static void RealGame(){
            Console.WriteLine("Enter 1 if you want to play as white or 2 if you want to play as black");
            var player = Console.ReadLine();
            var playersWhite = player.Contains('1') ? true : false;
            var playerSide = playersWhite ? "White" : "Black";
            Console.WriteLine("You're playing as " + playerSide);

            var state = new State();
            var evaluator = new Evaluator();
            var ai = new AI(evaluator);
            var moveConverter = new MoveConverter();
            var depth = 4;
            var moveCounter = 0;

            while(true){
                if(state.WhiteToMove == playersWhite){
                    State nextState = null;
                    var m = Console.ReadLine();
                    if(m.Contains("res")){
                        break;
                    }
                    var move = moveConverter.StringToMove(m);
                    var allStates = state.GetAllStates();
                    var x = allStates.Select(x => x.LastMove).ToList();
                    if(move.WasLongCastling){
                        nextState = allStates.FirstOrDefault(s => s.LastMove.WasLongCastling);
                    }
                    if(move.WasShortCastling){
                        nextState = allStates.FirstOrDefault(s => s.LastMove.WasShortCastling);
                    }
                    else{
                        nextState = allStates.FirstOrDefault(s => s.LastMove.DestinationFile == move.DestinationFile && s.LastMove.DestinationLine == move.DestinationLine 
                            && s.LastMove.StartingLine == move.StartingLine && s.LastMove.StartingFile == move.StartingFile);
                    }
                    if(nextState == null){
                        Console.WriteLine("You've entered an incorrect move. Try again or type 'res' if you want to quit");
                        continue;
                    }
                    else{
                        state = nextState;
                    }
                }
                else{
                    var eval = ai.Minimax(state, depth, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
                    Console.WriteLine("Your opponent has played the move " + moveConverter.MoveToString(eval.Item2.LastMove) + " and the score is " +  (double)eval.Item1/100);
                    state = eval.Item2;
                }
                moveCounter++;
                if(moveCounter > 18)
                {
                    depth = 5;
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
                var eval = ai.Minimax(state, 4, Int32.MinValue, Int32.MaxValue, state.WhiteToMove);
            }
            Console.WriteLine(sw.ElapsedMilliseconds / iterations);
            sw.Stop();
        }
    }
}
