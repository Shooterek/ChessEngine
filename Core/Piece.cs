using System;
using System.Collections.Generic;

public class Piece{
    public Piece(short line, short file, PieceType type, bool isWhite)
    {
        File = file;
        Line = line;
        Type = type;
        IsWhite = isWhite;
    }
    public short File { get; set; }
    public short Line { get; set; }
    public PieceType Type { get; set; }
    public bool IsWhite { get; set; }

    public Piece CopyPiece(){
        return new Piece(Line, File, Type, IsWhite);
    }

    public IEnumerable<State> GetPossibleStates(State state){
        switch(Type){
            case PieceType.Pawn:
                return GetPawnMoves(state.Board, state);
            case PieceType.Knight:
                return GetKnightMoves(state.Board, state);
            case PieceType.Bishop:
                return GetBishopMoves(state.Board, state);
            case PieceType.Rook:
                return GetRookMoves(state.Board, state);
            case PieceType.Queen:
                return GetQueenMoves(state.Board, state);
            case PieceType.King:
                return GetKingMoves(state.Board, state);
        }

        throw new Exception("Error in move generation");
    }

    private IEnumerable<State> GetKingMoves(Piece[,] board, State currentState)
    {
        var states = new List<State>();
        for(int i = -1; i <= 1; i++){
            for(int j = -1; j <= 1; j++){
                if(i != 0 || j != 0){
                    if(File + j >=0 && Line + i >= 0 && File + j <= 7 && Line + i <= 7){
                        var pieceOnSquare = board[Line + i, File + j];
                        if(pieceOnSquare == null || pieceOnSquare.IsWhite != IsWhite){
                            states.Add(new State(currentState, new Move(Line, File, (short)(Line + i), (short)(File + j))));
                        }
                    }
                }
            }
        }

        return states;
    }

    private IEnumerable<State> GetQueenMoves(Piece[,] board, State currentState)
    {
        var states = new List<State>();

        CheckLine(Line, File, board, 1, 0, states, currentState);
        CheckLine(Line, File, board, 0, 1, states, currentState);
        CheckLine(Line, File, board, -1, 0, states, currentState);
        CheckLine(Line, File, board, 0, -1, states, currentState);

        CheckLine(Line, File, board, 1, 1, states, currentState);
        CheckLine(Line, File, board, 1, -1, states, currentState);
        CheckLine(Line, File, board, -1, -1, states, currentState);
        CheckLine(Line, File, board, -1, 1, states, currentState);

        return states;
    }

    private IEnumerable<State> GetRookMoves(Piece[,] board, State currentState)
    {
        var states = new List<State>();

        CheckLine(Line, File, board, 1, 0, states, currentState);
        CheckLine(Line, File, board, 0, 1, states, currentState);
        CheckLine(Line, File, board, -1, 0, states, currentState);
        CheckLine(Line, File, board, 0, -1, states, currentState);

        return states;
    }

    private IEnumerable<State> GetBishopMoves(Piece[,] board, State currentState)
    {
        var states = new List<State>();

        CheckLine(Line, File, board, 1, 1, states, currentState);
        CheckLine(Line, File, board, 1, -1, states, currentState);
        CheckLine(Line, File, board, -1, -1, states, currentState);
        CheckLine(Line, File, board, -1, 1, states, currentState);

        return states;
    }

    private IEnumerable<State> GetKnightMoves(Piece[,] board, State currentState)
    {
        var states = new List<State>();

        GetSquareForKnight(Line, File, board, 2, 1, states, currentState);
        GetSquareForKnight(Line, File, board, 2, -1, states, currentState);
        GetSquareForKnight(Line, File, board, 1, 2, states, currentState);
        GetSquareForKnight(Line, File, board, 1, -2, states, currentState);
        GetSquareForKnight(Line, File, board, -2, 1, states, currentState);
        GetSquareForKnight(Line, File, board, -2, -1, states, currentState);
        GetSquareForKnight(Line, File, board, -1, 2, states, currentState);
        GetSquareForKnight(Line, File, board, -1, -2, states, currentState);

        return states;
    }

    private void GetSquareForKnight(short line, short file, Piece[,] board, int xStep, int yStep, List<State> states, State currentState)
    {
        var xIndex = (short)(file + xStep);
        var yIndex = (short)(line + yStep);
        if(xIndex >= 0 && yIndex >= 0 && xIndex <= 7 && yIndex <= 7){
            var pieceOnSquare = board[yIndex, xIndex];

            if(pieceOnSquare != null && pieceOnSquare.IsWhite == IsWhite){
                return;
            }
            else{
                states.Add(new State(currentState, new Move(Line, File, yIndex, xIndex)));
            }
        }
    }

    private IEnumerable<State> GetPawnMoves(Piece[,] board, State currentState)
    {
        var states = new List<State>();
        if(IsWhite){
            if(Line == 6){
                if(board[5, File] == null){
                    states.Add(new State(currentState, new Move(6, File, 5, File)));
                }
                if(board[4, File] == null){
                    states.Add(new State(currentState, new Move(6, File, 4, File)));
                }
            }
            else{
                if(board[Line - 1, File] == null){
                    if(Line - 1 == 0){
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line - 1), File, true, PieceType.Knight)));
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line - 1), File, true, PieceType.Bishop)));
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line - 1), File, true, PieceType.Rook)));
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line - 1), File, true, PieceType.Queen)));
                    }
                    else{
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line - 1), File)));
                    }
                }
            }
        }
        else{
            if(Line == 1){
                if(board[2, File] == null){
                    states.Add(new State(currentState, new Move(Line, File, 2, File)));
                }
                if(board[3, File] == null){
                    states.Add(new State(currentState, new Move(1, File, 3, File)));
                }
            }
            else{
                if(board[Line + 1, File] == null){
                    if(Line + 1 == 7){
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line + 1), File, true, PieceType.Knight)));
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line + 1), File, true, PieceType.Bishop)));
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line + 1), File, true, PieceType.Rook)));
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line + 1), File, true, PieceType.Queen)));
                    }
                    else{
                        states.Add(new State(currentState, new Move(Line, File, (short)(Line + 1), File)));
                    }
                }
            }
        }

        int moveDirection = IsWhite ? -1 : 1;

        var leftAttackedPiece = File - 1 >= 0 ? board[Line + moveDirection, File - 1] : null;
        var rightAttackedPiece = File + 1 <= 7 ? board[Line + moveDirection, File + 1] : null;
        if(leftAttackedPiece != null && leftAttackedPiece.IsWhite != IsWhite){
            if(Line + moveDirection == 0 || Line + moveDirection == 7){
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Knight)));
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Bishop)));
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Rook)));
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Queen)));
            }
            else{
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1))));
            }
        }
        if(rightAttackedPiece != null && rightAttackedPiece.IsWhite != IsWhite){
            if(Line + moveDirection == 0 || Line + moveDirection == 7){
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Knight)));
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Bishop)));
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Rook)));
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Queen)));
            }
            else{
                states.Add(new State(currentState, new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1))));
            }
        }

        return states;
    }

    private void CheckLine(short line, short file, Piece[,] board, int xStep, int yStep, List<State> states, State currentState)
    {
        for(int i = Line + yStep, j = File + xStep; i <= 7 && j <= 7 && i >= 0 && j >= 0; i += yStep, j += xStep){
            var pieceOnSquare = board[i, j];
            if(pieceOnSquare != null && pieceOnSquare.IsWhite == IsWhite){
                return;
            }
            else{
                states.Add(new State(currentState, new Move(Line, File, (short)(i), (short)(j))));
            }
        }
    }

    public override string ToString()
    {
        string letter = " ";
        switch(Type){
            case PieceType.Pawn: 
                letter = "p";
                break;
            case PieceType.Knight: 
                letter = "n";
                break;
            case PieceType.Bishop: 
                letter = "b";
                break;
            case PieceType.Rook: 
                letter = "r";
                break;
            case PieceType.King: 
                letter = "k";
                break;
            case PieceType.Queen: 
                letter = "q";
                break;
        }

        return IsWhite ? letter.ToUpper() : letter;
    }
}