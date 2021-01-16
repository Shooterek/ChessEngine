using System;

public class AI{
    public Evaluator Evaluator { get; set; }
    public AI(Evaluator eval)
    {
        Evaluator = eval;
    }
    public Tuple<int, State> Minimax(State state, int depth, int alpha, int beta, bool maximizingPlayer){
        if(depth == 0){
            return new Tuple<int, State>(Evaluator.Evaluate(state), null);
        }

        if(maximizingPlayer){
            var maxEval = Int32.MinValue;
            State maxState = null;
            var nextStates = state.GetAllStates();
            foreach(var nextState in nextStates){
                var result = Minimax(nextState, depth - 1, alpha, beta, !maximizingPlayer);
                if(result.Item1 > maxEval){
                    maxEval = result.Item1;
                    maxState = nextState;
                }
                if(result.Item1 > alpha){
                    alpha = result.Item1;
                }
                if(beta <= alpha){
                    break;
                }
            }
            return new Tuple<int, State>(maxEval, maxState);
        }
        else{
            var minEval = Int32.MaxValue;
            State minState = null;
            var nextStates = state.GetAllStates();
            foreach(var nextState in nextStates){
                var result = Minimax(nextState, depth - 1, alpha, beta, !maximizingPlayer);
                if(result.Item1 < minEval){
                    minEval = result.Item1;
                    minState = nextState;
                }
                if(result.Item1 < beta){
                    beta = result.Item1;
                }
                if(beta <= alpha){
                    break;
                }
            }
            return new Tuple<int, State>(minEval, minState);
        }
    }
}