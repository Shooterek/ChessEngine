using System;

public class AI{
    public Tuple<double, State> Minimax(State state, int depth, bool maximizingPlayer){
        if(depth == 0){
            return new Tuple<double, State>(Evaluate(state), null);
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

    private double Evaluate(State state)
    {
        double score = 0;
        foreach(var piece in state.PieceLookup){
            var value = 0;
            switch(piece.Type){
                case PieceType.Pawn:
                    value = 1;
                    break;
                case PieceType.Rook:
                    value = 5;
                    break;
                case PieceType.Bishop:
                    value = 3;
                    break;
                case PieceType.Knight:
                    value = 3;
                    break;
                case PieceType.Queen:
                    value = 9;
                    break;
            }
            score += piece.IsWhite ? value : -value;
        }
        return score;
    }
}