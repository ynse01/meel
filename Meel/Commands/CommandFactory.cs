using System.Collections.Generic;

namespace Meel.Commands
{
    public class CommandFactory
    {
        private static List<ImapCommand> commands = new List<ImapCommand>(30);
        
        public CommandFactory(IMailStation station)
        {
            if (commands.Count == 0)
            {
                var bad = new BadCommand(station);
                // This list needs to be in the same order as the ImapCommands enum.
                commands.Add(bad);
                commands.Add(new NoopCommand(station));
                commands.Add(new CapabilityCommand(station));
                commands.Add(new LoginCommand(station));
                commands.Add(new LogoutCommand(station));
                commands.Add(bad); // Authenticate (not implemented)
                commands.Add(bad); // StartTLS (Handled by ServerSession class)
                commands.Add(new SelectCommand(station));
                commands.Add(new ExamineCommand(station));
                commands.Add(new CreateCommand(station));
                commands.Add(new DeleteCommand(station));
                commands.Add(new RenameCommand(station));
                commands.Add(new SubscribeCommand(station));
                commands.Add(new UnsubscribeCommand(station));
                commands.Add(new ListCommand(station));
                commands.Add(new LSubCommand(station));
                commands.Add(new StatusCommand(station));
                commands.Add(new AppendCommand(station));
                commands.Add(new NoopCommand(station)); // Check is NOOP
                commands.Add(new CloseCommand(station));
                commands.Add(new ExpungeCommand(station));
                commands.Add(new SearchCommand(station));
                commands.Add(new FetchCommand(station));
                commands.Add(new StoreCommand(station));
                commands.Add(new CopyCommand(station));
                commands.Add(bad); // UID
            }
        }

        public ImapCommand GetCommand(ImapCommands request)
        {
            ImapCommand imapCommand = commands[(int)request];
            return imapCommand;
        }
    }
}
