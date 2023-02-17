using Assets.Scripts.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }

    private Stack<ICommand> commands = new Stack<ICommand>();

    void Start()
    {
        Instance = this;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        commands.Push(command);
    }

    public void Undo()
    {
        var command = commands.Pop();

        if (command != null)
        {
            command.Undo();
        }
    }
}
