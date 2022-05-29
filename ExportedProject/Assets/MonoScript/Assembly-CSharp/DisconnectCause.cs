using System;

public enum DisconnectCause
{
	DisconnectByServerUserLimit,
	ExceptionOnConnect,
	DisconnectByServerTimeout,
	DisconnectByServerLogic,
	Exception,
	InvalidAuthentication,
	MaxCcuReached,
	InvalidRegion,
	SecurityExceptionOnConnect,
	DisconnectByClientTimeout,
	InternalReceiveException,
	AuthenticationTicketExpired
}