using System;

public class TypedLobby
{
	public string Name;

	public LobbyType Type;

	public readonly static TypedLobby Default;

	public bool IsDefault
	{
		get
		{
			return (this.Type != LobbyType.Default ? false : string.IsNullOrEmpty(this.Name));
		}
	}

	static TypedLobby()
	{
		TypedLobby.Default = new TypedLobby();
	}

	public TypedLobby()
	{
		this.Name = string.Empty;
		this.Type = LobbyType.Default;
	}

	public TypedLobby(string name, LobbyType type)
	{
		this.Name = name;
		this.Type = type;
	}

	public override string ToString()
	{
		return string.Format("lobby '{0}'[{1}]", this.Name, this.Type);
	}
}