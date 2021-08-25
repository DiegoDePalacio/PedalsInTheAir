using System;

namespace PedalsInANonDescriptivePlace
{
    public enum MessageType
    {
        CRASH,
        PICK,
        POOP
    }

    public static class Messenger
    {
        public static Action<float> onCrash; // Damage as parameter
        public static Action<float> onPick; // Amount of refill of available poop as parameter
        public static Action<float> onPoop; // Damage as parameter

        public static void Send(MessageType messageType, float argument)
        {
            switch (messageType)
            {
                case MessageType.CRASH: onCrash?.Invoke(argument); break;
                case MessageType.PICK: onPick?.Invoke(argument); break;
                case MessageType.POOP: onPoop?.Invoke(argument); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }
        }
    }
}