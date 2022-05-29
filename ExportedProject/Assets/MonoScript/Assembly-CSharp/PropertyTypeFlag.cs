using System;

[Flags]
public enum PropertyTypeFlag : byte
{
	None,
	Game,
	Actor,
	GameAndActor
}