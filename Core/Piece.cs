using System;
using System.Linq;
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

    public void GetPossibleMoves(Piece[,] board, List<Move> moves, bool castleShort, bool castleLong, bool includePawnAdvances = true){
        switch(Type){
            case PieceType.Pawn:
                GetPawnMoves(board, moves, includePawnAdvances);
                break;
            case PieceType.Knight:
                GetKnightMoves(board, moves);
                break;
            case PieceType.Bishop:
                GetBishopMoves(board, moves);
                break;
            case PieceType.Rook:
                GetRookMoves(board, moves);
                break;
            case PieceType.Queen:
                GetQueenMoves(board, moves);
                break;
            case PieceType.King:
                GetKingMoves(board, moves, castleShort, castleLong);
                break;
        }
    }

    private void GetKingMoves(Piece[,] board, List<Move> moves, bool castleShort, bool castleLong)
    {
        for(int i = -1; i <= 1; i++){
            for(int j = -1; j <= 1; j++){
                if(i != 0 || j != 0){
                    if(File + j >=0 && Line + i >= 0 && File + j <= 7 && Line + i <= 7){
                        var pieceOnSquare = board[Line + i, File + j];
                        if(pieceOnSquare == null || pieceOnSquare.IsWhite != IsWhite){
                            moves.Add(new Move(Line, File, (short)(Line + i), (short)(File + j)));
                        }
                    }
                }
            }
        }
        if(castleShort){
            Piece rightRook = null;
            if(File + 3 <= 7){
                rightRook = board[Line, File + 3];
            }
            if(board[Line, File + 1] == null && board[Line, File + 2] == null && rightRook != null){
                moves.Add(new Move(Line, File, Line, (short)(File + 2), false, false, null, true));
            }
        }
        if(castleLong){
            Piece leftRook = null;
            if(File - 4 >= 0){
                leftRook = board[Line, File - 4];
            }
            if(board[Line, File - 1] == null && board[Line, File - 2] == null && board[Line, File - 3] == null && leftRook != null){
                moves.Add(new Move(Line, File, Line, (short)(File - 2), false, false, null, false, true));
            }
        }
    }

    private void GetQueenMoves(Piece[,] board, List<Move> moves)
    {
        CheckLine(Line, File, 1, 0, board, moves);
        CheckLine(Line, File, 0, 1, board, moves);
        CheckLine(Line, File, -1, 0, board, moves);
        CheckLine(Line, File, 0, -1, board, moves);

        CheckLine(Line, File, 1, 1, board, moves);
        CheckLine(Line, File, 1, -1, board, moves);
        CheckLine(Line, File, -1, -1, board, moves);
        CheckLine(Line, File, -1, 1, board, moves);

    }

    private void GetRookMoves(Piece[,] board, List<Move> moves)
    {
        CheckLine(Line, File, 1, 0, board, moves);
        CheckLine(Line, File, 0, 1, board, moves);
        CheckLine(Line, File, -1, 0, board, moves);
        CheckLine(Line, File, 0, -1, board, moves);

    }

    private void GetBishopMoves(Piece[,] board, List<Move> moves)
    {
        CheckLine(Line, File, 1, 1, board, moves);
        CheckLine(Line, File, 1, -1, board, moves);
        CheckLine(Line, File, -1, -1, board, moves);
        CheckLine(Line, File, -1, 1, board, moves);

    }

    private void GetKnightMoves(Piece[,] board, List<Move> moves)
    {
        GetSquareForKnight(Line, File, 2, 1, board, moves);
        GetSquareForKnight(Line, File, 2, -1, board, moves);
        GetSquareForKnight(Line, File, 1, 2, board, moves);
        GetSquareForKnight(Line, File, 1, -2, board, moves);
        GetSquareForKnight(Line, File, -2, 1, board, moves);
        GetSquareForKnight(Line, File, -2, -1, board, moves);
        GetSquareForKnight(Line, File, -1, 2, board, moves);
        GetSquareForKnight(Line, File, -1, -2, board, moves);
    }

    private void GetSquareForKnight(short line, short file, int xStep, int yStep, Piece[,] board, List<Move> moves)
    {
        var xIndex = (short)(file + xStep);
        var yIndex = (short)(line + yStep);
        if(xIndex >= 0 && yIndex >= 0 && xIndex <= 7 && yIndex <= 7){
            var pieceOnSquare = board[yIndex, xIndex];

            if(pieceOnSquare == null){
                moves.Add(new Move(Line, File, yIndex, xIndex));
            }
            else if(pieceOnSquare.IsWhite == IsWhite){
                return;
            }
            else{
                moves.Add(new Move(Line, File, yIndex, xIndex, true));
            }
        }
    }

    private void GetPawnMoves(Piece[,] board, List<Move> moves, bool includePawnAdvances = true)
    {
        if(includePawnAdvances)
        {
            GetPawnAdvanceMoves(board, moves);
        }

        int moveDirection = IsWhite ? -1 : 1;

        var leftAttackedPiece = File - 1 >= 0 ? board[Line + moveDirection, File - 1] : null;
        var rightAttackedPiece = File + 1 <= 7 ? board[Line + moveDirection, File + 1] : null;
        if(leftAttackedPiece != null && leftAttackedPiece.IsWhite != IsWhite){
            if(Line + moveDirection == 0 || Line + moveDirection == 7){
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, true, PieceType.Knight));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, true, PieceType.Bishop));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, true, PieceType.Rook));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, true, PieceType.Queen));
            }
            else{
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true));
            }
        }
        if(rightAttackedPiece != null && rightAttackedPiece.IsWhite != IsWhite){
            if(Line + moveDirection == 0 || Line + moveDirection == 7){
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, true, PieceType.Knight));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, true, PieceType.Bishop));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, true, PieceType.Rook));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, true, PieceType.Queen));
            }
            else{
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true));
            }
        }
    }

    private void GetPawnAdvanceMoves(Piece[,] board, List<Move> moves)
    {
        if(IsWhite){
            if(Line == 6){
                if(board[5, File] == null){
                    moves.Add(new Move(6, File, 5, File));
                    if(board[4, File] == null){
                        moves.Add(new Move(6, File, 4, File));
                    }
                }
            }
            else{
                if(board[Line - 1, File] == null){
                    if(Line - 1 == 0){
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, false, true, PieceType.Knight));
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, false, true, PieceType.Bishop));
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, false, true, PieceType.Rook));
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, false, true, PieceType.Queen));
                    }
                    else{
                        moves.Add(new Move(Line, File, (short)(Line - 1), File));
                    }
                }
            }
        }
        else{
            if(Line == 1){
                if(board[2, File] == null){
                    moves.Add(new Move(Line, File, 2, File));
                    if(board[3, File] == null){
                        moves.Add(new Move(1, File, 3, File));
                    }
                }
            }
            else{
                if(board[Line + 1, File] == null){
                    if(Line + 1 == 7){
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, false, true, PieceType.Knight));
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, false, true, PieceType.Bishop));
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, false, true, PieceType.Rook));
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, false, true, PieceType.Queen));
                    }
                    else{
                        moves.Add(new Move(Line, File, (short)(Line + 1), File));
                    }
                }
            }
        }
    }

    private void CheckLine(short line, short file, int xStep, int yStep, Piece[,] board, List<Move> allMoves)
    {
        for(int i = Line + yStep, j = File + xStep; i <= 7 && j <= 7 && i >= 0 && j >= 0; i += yStep, j += xStep){
            var pieceOnSquare = board[i, j];
            if(pieceOnSquare != null && pieceOnSquare.IsWhite == IsWhite){
                break;
            }
            else if(pieceOnSquare != null && pieceOnSquare.IsWhite != IsWhite){
                allMoves.Add(new Move(Line, File, (short)(i), (short)(j), true));
                break;
            }
            else{
                allMoves.Add(new Move(Line, File, (short)(i), (short)(j)));
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