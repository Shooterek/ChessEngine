using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class State{
    public State()
    {
        SetCastlingPossibility();
        WhiteToMove = true;
        InitializeBoard();
    }

    public State(State previousState, Move move){
        SetCastlingPossibility();
        LastMove = move;
        WhiteToMove = !previousState.WhiteToMove;
        Board = new Piece[8, 8];
        PieceLookup = new List<Piece>();
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                if(previousState.Board[i, j] != null){
                    var newPiece = previousState.Board[i, j].CopyPiece();
                    Board[i, j] = newPiece;
                    PieceLookup.Add(newPiece);
                }
            }
        }
        MakeMove(move);
    }

    public State(string fen){
        SetCastlingPossibility();
        Board = new Piece[8, 8];
        PieceLookup = new List<Piece>();
        var parts = fen.Split('/');
        var sideToMove = parts[7].Split(' ')[1];
        WhiteToMove = sideToMove.Equals("w", StringComparison.InvariantCultureIgnoreCase) ? true : false;
        for(short i = 0; i < 8; i++){
            short fileIndex = 0;
            foreach(var c in parts[i].ToCharArray()){
                if(c == ' '){
                    break;
                }
                if(Char.IsDigit(c)){
                    fileIndex += (short)Char.GetNumericValue(c);
                }
                else{
                    var pieceType = GetPieceType(c);
                    var isWhite = Char.IsUpper(c) ? true : false;
                    var piece = new Piece(i, fileIndex, pieceType, isWhite);
                    Board[i, fileIndex] = piece;
                    PieceLookup.Add(piece);
                    fileIndex++;
                }
            }
        }
    }
    public Piece[,] Board { get; set; }
    public List<Piece> PieceLookup { get; set; }
    public bool WhiteToMove { get; set; }
    public Move LastMove { get; set; }
    public bool B00CastlingPossibility { get; set; }
    public bool B000CastlingPossibility { get; set; }
    public bool W00CastlingPossibility { get; set; }
    public bool W000CastlingPossibility { get; set; }

    public List<State> GetAllStates(){
        var states = new List<State>();
        var moves = new List<Move>();
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                var piece = Board[i, j];
                if(piece != null && piece.IsWhite == WhiteToMove){
                    moves.AddRange(Board[i, j].GetPossibleMoves(Board));
                }
            }
        }

        foreach(var move in moves){
            var newState = new State(this, move);
            var attackingMoves = newState.GetAttackingMoves();
            var king = newState.PieceLookup.First(p => p.Type == PieceType.King && p.IsWhite == WhiteToMove);
            if(attackingMoves.Any(m => m.DestinationFile == king.File && m.DestinationLine == king.Line)){

            }else{
                states.Add(newState);
            }
        }

        return states;
    }

    public List<Move> GetAttackingMoves(){
        var moves = new List<Move>();
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                var piece = Board[i, j];
                if(piece != null && piece.IsWhite == WhiteToMove){
                    moves.AddRange(Board[i, j].GetPossibleMoves(Board, false));
                }
            }
        }

        return moves;
    }

    public void DisplayBoard(){
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                Console.Write(Board[i, j] == null ? "O" : Board[i, j].ToString());
            }
            Console.WriteLine();
        }
    }

    public string GetFen(){
        var fenBuilder = new StringBuilder();
        for(int i = 0; i < 8; i++){
            var emptySpaceCounter = 0;
            for(int j = 0; j < 8; j++){
                if(Board[i, j] != null){
                    if(emptySpaceCounter > 0){
                        fenBuilder.Append(emptySpaceCounter);
                        emptySpaceCounter = 0;
                    }
                    fenBuilder.Append(Board[i, j].ToString());
                }
                else{
                    emptySpaceCounter++;
                }
            }
            if(emptySpaceCounter > 0){
                fenBuilder.Append(emptySpaceCounter);
            }
            fenBuilder.Append('/');
        }
        fenBuilder.Remove(fenBuilder.Length - 1, 1);
        // fenBuilder.Append(WhiteToMove ? " w" : " b");

        return fenBuilder.ToString();
    }

    private void SetCastlingPossibility(){
        B000CastlingPossibility = true;
        B00CastlingPossibility = true;
        W000CastlingPossibility = true;
        W00CastlingPossibility = true;
    }

    private PieceType GetPieceType(char c){
        var p = c.ToString().ToLower();
        switch(p){
            case "p":
                return PieceType.Pawn;
            case "k":
                return PieceType.King;
            case "n":
                return PieceType.Knight;
            case "r":
                return PieceType.Rook;
            case "b":
                return PieceType.Bishop;
            case "q":
                return PieceType.Queen;
        }

        throw new Exception("Incorrect piece letter" + p);
    }

    private void MakeMove(Move move)
    {
        var movedPiece = Board[move.StartingLine, move.StartingFile];
        var capturedPiece = Board[move.DestinationLine, move.DestinationFile];
        if(capturedPiece != null){
            PieceLookup.Remove(capturedPiece);
            capturedPiece = null;
        }
        if(movedPiece.Type == PieceType.King){
            if(move.DestinationFile - move.StartingFile == 2){
                Board[move.StartingLine, 7].File = 5;
                Board[move.StartingLine, 5] = Board[move.StartingLine, 7];
                Board[move.StartingLine, 7] = null;
            }
            else if(move.DestinationFile - move.StartingFile == -2){
                Board[move.StartingLine, 0].File = 3;
                Board[move.StartingLine, 3] = Board[move.StartingLine, 0];
                Board[move.StartingLine, 0] = null;
            }
        }
        Board[move.StartingLine, move.StartingFile] = null;
        movedPiece.File = move.DestinationFile;
        movedPiece.Line = move.DestinationLine;
        Board[move.DestinationLine, move.DestinationFile] = movedPiece;
        if(move.WasPromotion){
            movedPiece.Type = (PieceType)move.PromotedTo;
        }
    }

    private void InitializeBoard(){
        Board = new Piece[8, 8];
        PieceLookup = new List<Piece>();
        for(short i = 0; i < 8; i++){
            LoadPiece(1, i, PieceType.Pawn, false);
            LoadPiece(6, i, PieceType.Pawn, true);
        }
        
        LoadPiece(0, 0, PieceType.Rook, false);
        LoadPiece(7, 0, PieceType.Rook, true);
        LoadPiece(0, 7, PieceType.Rook, false);
        LoadPiece(7, 7, PieceType.Rook, true);

        LoadPiece(0, 1, PieceType.Knight, false);
        LoadPiece(7, 1, PieceType.Knight, true);
        LoadPiece(0, 6, PieceType.Knight, false);
        LoadPiece(7, 6, PieceType.Knight, true);

        LoadPiece(0, 2, PieceType.Bishop, false);
        LoadPiece(7, 2, PieceType.Bishop, true);
        LoadPiece(0, 5, PieceType.Bishop, false);
        LoadPiece(7, 5, PieceType.Bishop, true);

        LoadPiece(0, 3, PieceType.Queen, false);
        LoadPiece(7, 3, PieceType.Queen, true);

        LoadPiece(0, 4, PieceType.King, false);
        LoadPiece(7, 4, PieceType.King, true);
    }
    
    private void LoadPiece(short line, short file, PieceType type, bool isWhite){
        var piece = new Piece(line, file, type, isWhite);
        Board[line, file] = piece;
        PieceLookup.Add(piece);
    }
}