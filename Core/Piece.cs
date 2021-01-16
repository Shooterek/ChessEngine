using System;
using System.Linq;
using System.Collections.Generic;

public class Piece{
    public Piece(short line, short file, PieceType type, bool isWhite, bool hasMoved = false)
    {
        File = file;
        Line = line;
        Type = type;
        IsWhite = isWhite;
        HasMoved = hasMoved;
    }
    public short File { get; set; }
    public short Line { get; set; }
    public PieceType Type { get; set; }
    public bool IsWhite { get; set; }
    public bool HasMoved { get; set; }

    public Piece CopyPiece(){
        return new Piece(Line, File, Type, IsWhite, HasMoved);
    }

    public IEnumerable<Move> GetPossibleMoves(Piece[,] board, bool onlyAttacks = false, bool onlyPawnAdvances = false){
        IEnumerable<Move> moves = null;
        if(onlyPawnAdvances && Type == PieceType.Pawn){
            return GetPawnAdvanceMoves(board);
        }
        switch(Type){
            case PieceType.Pawn:
                moves = GetPawnMoves(board, onlyAttacks);
                break;
            case PieceType.Knight:
                moves = GetKnightMoves(board);
                break;
            case PieceType.Bishop:
                moves = GetBishopMoves(board);
                break;
            case PieceType.Rook:
                moves = GetRookMoves(board);
                break;
            case PieceType.Queen:
                moves = GetQueenMoves(board);
                break;
            case PieceType.King:
                moves = GetKingMoves(board);
                break;
        }

        return moves;
    }

    //TODO bool includeCastling
    private IEnumerable<Move> GetKingMoves(Piece[,] board)
    {
        var moves = new List<Move>();
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
        if(!HasMoved){
            var rightRook = board[Line, File + 3];
            var leftRook = board[Line, File - 4];
            if(board[Line, File + 1] == null && board[Line, File + 2] == null 
                && rightRook != null && rightRook.Type == PieceType.Rook && !rightRook.HasMoved){
                    moves.Add(new Move(Line, File, Line, (short)(File + 2), false, null, true));
                }
            if(board[Line, File - 1] == null && board[Line, File - 2] == null && board[Line, File - 3] == null
                && leftRook != null && leftRook.Type == PieceType.Rook && !leftRook.HasMoved){
                    moves.Add(new Move(Line, File, Line, (short)(File - 2), false, null, false, true));
                }
        }
        return moves;
    }

    private IEnumerable<Move> GetQueenMoves(Piece[,] board)
    {
        var moves = new List<Move>();

        moves.AddRange(CheckLine(Line, File, 1, 0, board));
        moves.AddRange(CheckLine(Line, File, 0, 1, board));
        moves.AddRange(CheckLine(Line, File, -1, 0, board));
        moves.AddRange(CheckLine(Line, File, 0, -1, board));

        moves.AddRange(CheckLine(Line, File, 1, 1, board));
        moves.AddRange(CheckLine(Line, File, 1, -1, board));
        moves.AddRange(CheckLine(Line, File, -1, -1, board));
        moves.AddRange(CheckLine(Line, File, -1, 1, board));

        return moves;
    }

    private IEnumerable<Move> GetRookMoves(Piece[,] board)
    {
        var moves = new List<Move>();

        moves.AddRange(CheckLine(Line, File, 1, 0, board));
        moves.AddRange(CheckLine(Line, File, 0, 1, board));
        moves.AddRange(CheckLine(Line, File, -1, 0, board));
        moves.AddRange(CheckLine(Line, File, 0, -1, board));

        return moves;
    }

    private IEnumerable<Move> GetBishopMoves(Piece[,] board)
    {
        var moves = new List<Move>();

        moves.AddRange(CheckLine(Line, File, 1, 1, board));
        moves.AddRange(CheckLine(Line, File, 1, -1, board));
        moves.AddRange(CheckLine(Line, File, -1, -1, board));
        moves.AddRange(CheckLine(Line, File, -1, 1, board));

        return moves;
    }

    private IEnumerable<Move> GetKnightMoves(Piece[,] board)
    {
        var moves = new List<Move>();

        GetSquareForKnight(Line, File, 2, 1, board, moves);
        GetSquareForKnight(Line, File, 2, -1, board, moves);
        GetSquareForKnight(Line, File, 1, 2, board, moves);
        GetSquareForKnight(Line, File, 1, -2, board, moves);
        GetSquareForKnight(Line, File, -2, 1, board, moves);
        GetSquareForKnight(Line, File, -2, -1, board, moves);
        GetSquareForKnight(Line, File, -1, 2, board, moves);
        GetSquareForKnight(Line, File, -1, -2, board, moves);

        return moves;
    }

    private void GetSquareForKnight(short line, short file, int xStep, int yStep, Piece[,] board, List<Move> moves)
    {
        var xIndex = (short)(file + xStep);
        var yIndex = (short)(line + yStep);
        if(xIndex >= 0 && yIndex >= 0 && xIndex <= 7 && yIndex <= 7){
            var pieceOnSquare = board[yIndex, xIndex];

            if(pieceOnSquare != null && pieceOnSquare.IsWhite == IsWhite){
                return;
            }
            else{
                moves.Add(new Move(Line, File, yIndex, xIndex));
            }
        }
    }

    private IEnumerable<Move> GetPawnMoves(Piece[,] board, bool onlyAttacks = false)
    {
        var moves = new List<Move>();
        if(!onlyAttacks){
            moves.AddRange(GetPawnAdvanceMoves(board));
        }

        int moveDirection = IsWhite ? -1 : 1;

        var leftAttackedPiece = File - 1 >= 0 ? board[Line + moveDirection, File - 1] : null;
        var rightAttackedPiece = File + 1 <= 7 ? board[Line + moveDirection, File + 1] : null;
        if(leftAttackedPiece != null && leftAttackedPiece.IsWhite != IsWhite){
            if(Line + moveDirection == 0 || Line + moveDirection == 7){
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Knight));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Bishop));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Rook));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1), true, PieceType.Queen));
            }
            else{
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File - 1)));
            }
        }
        if(rightAttackedPiece != null && rightAttackedPiece.IsWhite != IsWhite){
            if(Line + moveDirection == 0 || Line + moveDirection == 7){
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Knight));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Bishop));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Rook));
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1), true, PieceType.Queen));
            }
            else{
                moves.Add(new Move(Line, File, (short)(Line + moveDirection), (short)(File + 1)));
            }
        }

        return moves;
    }

    private IEnumerable<Move> GetPawnAdvanceMoves(Piece[,] board)
    {
        var moves = new List<Move>();
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
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, true, PieceType.Knight));
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, true, PieceType.Bishop));
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, true, PieceType.Rook));
                        moves.Add(new Move(Line, File, (short)(Line - 1), File, true, PieceType.Queen));
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
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, true, PieceType.Knight));
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, true, PieceType.Bishop));
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, true, PieceType.Rook));
                        moves.Add(new Move(Line, File, (short)(Line + 1), File, true, PieceType.Queen));
                    }
                    else{
                        moves.Add(new Move(Line, File, (short)(Line + 1), File));
                    }
                }
            }
        }
        return moves;
    }

    private List<Move> CheckLine(short line, short file, int xStep, int yStep, Piece[,] board)
    {
        var allMoves = new List<Move>();
        for(int i = Line + yStep, j = File + xStep; i <= 7 && j <= 7 && i >= 0 && j >= 0; i += yStep, j += xStep){
            var pieceOnSquare = board[i, j];
            if(pieceOnSquare != null && pieceOnSquare.IsWhite == IsWhite){
                break;
            }
            else if(pieceOnSquare != null && pieceOnSquare.IsWhite != IsWhite){
                allMoves.Add(new Move(Line, File, (short)(i), (short)(j)));
                break;
            }
            else{
                allMoves.Add(new Move(Line, File, (short)(i), (short)(j)));
            }
        }

        return allMoves;
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