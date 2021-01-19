using System;
using System.Text;

public class MoveConverter{
    public string MoveToString(Move move){
        var moveBuilder = new StringBuilder();
        moveBuilder.Append(NumberToFile(move.StartingFile));
        moveBuilder.Append(8 - (move.StartingLine));
        moveBuilder.Append(NumberToFile(move.DestinationFile));
        moveBuilder.Append(8 - (move.DestinationLine));

        return moveBuilder.ToString();
    }

    public Move StringToMove(string move){
        if(move.Equals("00")){
            return new Move(0, 0, 0, 0, false, false, null, true);
        }
        if(move.Equals("000")){
            return new Move(0, 0, 0, 0, false, false, null, false, true);
        }
        short startingFile = FileToNumber(move[0]);
        var startingLine = 8 - (int)Char.GetNumericValue(move[1]);
        var destFile = FileToNumber(move[2]);
        var destLine = 8 - (int)Char.GetNumericValue(move[3]);

        return new Move((short)startingLine, startingFile, (short)destLine, destFile);
    }

    private char NumberToFile(int file){
        var aFile = (int)'a';

        return Convert.ToChar(aFile + file);
    }

    private short FileToNumber(char file){
        var aFile = (int)'a';
        var fileIndex = (int)file;

        return (short)Convert.ToChar(file - aFile);
    }
}