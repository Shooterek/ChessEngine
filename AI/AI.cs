using System;

public class AI{
    public Evaluator Evaluator { get; set; }
    public AI(Evaluator eval)
    {
        Evaluator = eval;
    }
    public Tuple<double, State> Minimax(State state, int depth, bool maximizingPlayer){
        if(depth == 0){
            return new Tuple<double, State>(Evaluator.Evaluate(state), null);
        }

        if(maximizingPlayer){
            var maxEval = Double.MinValue;
            State maxState = null;
            var nextStates = state.GetAllStates();
            foreach(var nextState in nextStates){
                var result = Minimax(nextState, depth - 1, !maximizingPlayer);
                if(result.Item1 > maxEval){
                    maxEval = result.Item1;
                    maxState = nextState;
                }
            }
            return new Tuple<double, State>(maxEval, maxState);
        }
        else{
            var minEval = Double.MaxValue;
            State minState = null;
            var nextStates = state.GetAllStates();
            foreach(var nextState in nextStates){
                var result = Minimax(nextState, depth - 1, !maximizingPlayer);
                if(result.Item1 < minEval){
                    minEval = result.Item1;
                    minState = nextState;
                }
            }
            return new Tuple<double, State>(minEval, minState);
        }
    }
}