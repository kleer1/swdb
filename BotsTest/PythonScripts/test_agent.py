import asyncio
import websockets
import argparse
import time


async def connect_to_dotnet_server(port):
    uri = f"ws://localhost:{port}"  # Use the provided port in the URI

    async with websockets.connect(uri) as websocket:
        try:
            while True:
                state_from_dotnet = await websocket.recv()
                print(f"Received response from .NET server: {state_from_dotnet}")
                if state_from_dotnet == "This is a game state":
                    message_to_dotnet = "This is a game action"
                    await websocket.send(message_to_dotnet)
                    print(f"Sent message to .NET server: {message_to_dotnet}")
                else:
                    message_to_dotnet = "did not receive msg"
                    await websocket.send(message_to_dotnet)
                    print(f"Sent message to .NET server: {message_to_dotnet}")

                reward_from_dotnet = await websocket.recv()
                print(f"Received response from .NET server: {reward_from_dotnet}")

                stop_from_dotnet = await websocket.recv()
                print(f"Received response from .NET server: {stop_from_dotnet}")

                if stop_from_dotnet == "Should stop":
                    message_to_dotnet = "no"
                    await websocket.send(message_to_dotnet)
                    print(f"Sent message to .NET server: {message_to_dotnet}")
                else:
                    message_to_dotnet = "bad msg"
                    await websocket.send(message_to_dotnet)
                    print(f"Sent message to .NET server: {message_to_dotnet}")

                time.sleep(10 / 1000)

                # Process the response as needed (replace with your logic)
        except websockets.exceptions.ConnectionClosedOK:
            print("Connection closed by the .NET server.")


def main():
    parser = argparse.ArgumentParser(description="Python client for .NET WebSocket server")
    parser.add_argument("--port", type=int, default=8765, help="Port number of the .NET WebSocket server")
    args = parser.parse_args()
    print(f"going to start connection on ws://localhost:{args.port}")
    time.sleep(2)
    asyncio.get_event_loop().run_until_complete(connect_to_dotnet_server(args.port))


if __name__ == "__main__":
    main()
