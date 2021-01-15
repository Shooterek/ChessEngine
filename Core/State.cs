using System;
using System.Collections.Generic;

public class State{
    public State()
    {
        WhiteToMove = true;
        InitializeBoard();
    }
    public Piece[,] Board { get; set; }
    public bool WhiteToMove { get; set; }
    public Move LastMove { get; set; }

    public State(State previousState, Move move){
        LastMove = move;
        WhiteToMove = !previousState.WhiteToMove;
        Board = new Piece[8, 8];
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                if(previousState.Board[i, j] != null){
                    Board[i, j] = previousState.Board[i, j].CopyPiece();
                }
            }
        }
        MakeMove(move);
    }

    public List<State> GetAllStates(){
        var states = new List<State>();

        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                var piece = Board[i, j];
                if(piece != null && piece.IsWhite == WhiteToMove 
                && (piece.Type == PieceType.Knight)){
                    states.AddRange(Board[i, j].GetPossibleStates(this));
                }
            }
        }

        return states;
    }

    public void DisplayBoard(){
        for(int i = 0; i < 8; i++){
            for(int j = 0; j < 8; j++){
                Console.Write(Board[i, j] == null ? "O" : Board[i, j].ToString());
            }
            Console.WriteLine();
        }
    }
    
    private void MakeMove(Move move)
    {
        var movedPiece = Board[move.StartingLine, move.StartingFile];
        var capturedPiece = Board[move.DestinationLine, move.DestinationFile];
        if(capturedPiece != null){
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
        for(short i = 0; i < 8; i++){
            Board[1, i] = new Piece(1, i, PieceType.Pawn, false);
            Board[6, i] = new Piece(6, i, PieceType.Pawn, true);
        }
        
        Board[0, 0] = new Piece(0, 0, PieceType.Rook, false);
        Board[7, 0] = new Piece(7, 0, PieceType.Rook, true);
        Board[0, 7] = new Piece(0, 7, PieceType.Rook, false);
        Board[7, 7] = new Piece(7, 7, PieceType.Rook, true);

        Board[0, 1] = new Piece(0, 1, PieceType.Knight, false);
        Board[7, 1] = new Piece(7, 1, PieceType.Knight, true);
        Board[0, 6] = new Piece(0, 6, PieceType.Knight, false);
        Board[7, 6] = new Piece(7, 6, PieceType.Knight, true);

        Board[0, 2] = new Piece(0, 2, PieceType.Bishop, false);
        Board[7, 2] = new Piece(7, 2, PieceType.Bishop, true);
        Board[0, 5] = new Piece(0, 5, PieceType.Bishop, false);
        Board[7, 5] = new Piece(7, 5, PieceType.Bishop, true);

        Board[0, 3] = new Piece(0, 3, PieceType.Queen, false);
        Board[7, 3] = new Piece(7, 3, PieceType.Queen, true);

        Board[0, 4] = new Piece(0, 4, PieceType.King, false);
        Board[7, 4] = new Piece(7, 4, PieceType.King, true);
    }
}