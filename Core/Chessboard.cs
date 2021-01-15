using System;

public class Game{
    public State State { get; set; }
    public Game()
    {
        State = new State();
    }
}