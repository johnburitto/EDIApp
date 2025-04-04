using EDIApp.Common.Constants;
using EDIApp.Socket.Implementations;

var socket = new CustomWebSocket();

await socket.StartWebsocketAsync(ExitAction.GenerateExitAction("Exit"));
