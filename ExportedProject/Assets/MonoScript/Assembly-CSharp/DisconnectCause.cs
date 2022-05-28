public enum DisconnectCause
{
	DisconnectByServerUserLimit = 0,
	ExceptionOnConnect = 1,
	DisconnectByServerTimeout = 2,
	DisconnectByServerLogic = 3,
	Exception = 4,
	InvalidAuthentication = 5,
	MaxCcuReached = 6,
	InvalidRegion = 7,
	SecurityExceptionOnConnect = 8,
	DisconnectByClientTimeout = 9,
	InternalReceiveException = 10,
	AuthenticationTicketExpired = 11
}
