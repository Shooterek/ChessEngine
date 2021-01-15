public class Move{
    public Move(short startingLine, short startingFile, short destinationLine, short destinationFile, bool wasPromotion = false, PieceType? promotedTo = null)
    {
        StartingLine = startingLine;
        StartingFile = startingFile;
        DestinationLine = destinationLine;
        DestinationFile = destinationFile;
        WasPromotion = wasPromotion;
        PromotedTo = promotedTo;
    }
    public short StartingLine { get; set; }
    public short StartingFile { get; set; }
    public short DestinationLine { get; set; }
    public short DestinationFile { get; set; }
    public bool WasPromotion { get; set; }
    public PieceType? PromotedTo { get; set; }
}