using System;
using System.IO;
using System.Linq;

public class Evaluator{
    public Evaluator()
    {
        LoadTables();
    }
    private int[,] _whitePawnsTable;
    private int[,] _blackPawnsTable;
    private int[,] _whiteKnightsTable;
    private int[,] _blackKnightsTable;
    private int[,] _whiteBishopsTable;
    private int[,] _blackBishopsTable;
    private int[,] _whiteRooksTable;
    private int[,] _blackRooksTable;
    private int[,] _whiteQueensTable;
    private int[,] _blackQueensTable;
    public int Evaluate(State state)
    {
        int score = 0;
        foreach(var piece in state.PieceLookup){
            int value = 0;
            int bonus = 0;
            switch(piece.Type){
                case PieceType.Pawn:
                    value = 100;
                    bonus = piece.IsWhite ? _whitePawnsTable[piece.Line, piece.File] : _blackPawnsTable[piece.Line, piece.File];
                    break;
                case PieceType.Rook:
                    value = 560;
                    bonus = piece.IsWhite ? _whiteRooksTable[piece.Line, piece.File] : _blackRooksTable[piece.Line, piece.File];
                    break;
                case PieceType.Bishop:
                    value = 330;
                    bonus = piece.IsWhite ? _whiteBishopsTable[piece.Line, piece.File] : _blackBishopsTable[piece.Line, piece.File];
                    break;
                case PieceType.Knight:
                    value = 300;
                    bonus = piece.IsWhite ? _whiteKnightsTable[piece.Line, piece.File] : _blackKnightsTable[piece.Line, piece.File];
                    break;
                case PieceType.Queen:
                    value = 950;
                    bonus = piece.IsWhite ? _whiteQueensTable[piece.Line, piece.File] : _blackQueensTable[piece.Line, piece.File];
                    break;
            }
            score += piece.IsWhite ? value : -value;
            score += piece.IsWhite ? bonus : -bonus;
        }
        return score;
    }

    private void LoadTables(){
        _whitePawnsTable = LoadTable("./pawns-table.txt", true);
        _blackPawnsTable = LoadTable("./pawns-table.txt", false);
        
        _whiteKnightsTable = LoadTable("./knights-table.txt", true);
        _blackKnightsTable = LoadTable("./knights-table.txt", false);
        
        _whiteBishopsTable = LoadTable("./bishops-table.txt", true);
        _blackBishopsTable = LoadTable("./bishops-table.txt", false);
        
        _whiteRooksTable = LoadTable("./rooks-table.txt", true);
        _blackRooksTable = LoadTable("./rooks-table.txt", false);

        _whiteQueensTable = LoadTable("./queens-table.txt", true);
        _blackQueensTable = LoadTable("./queens-table.txt", false);
        
    }

    private int[,] LoadTable(string path, bool isWhite){
        var table = new int[8, 8];
        var data = File.ReadAllLines(path);
        if(isWhite){
            var counter = 0;
            foreach(var row in data){
                var values = row.Split(',');
                for(int i = 0; i < 8; i++){
                    table[counter, i] = Int32.Parse(values[i]);
                }
                counter++;
            }
        }
        else{
            for(int i = 7, a = 0; i >= 0; i--, a++){
                var row = data[i].Split(',');
                for(int j = 7, b = 0; j >= 0; j--, b++){
                    table[a, b] = Int32.Parse(row[j]);
                }
            }
        }

        return table;
    }
}