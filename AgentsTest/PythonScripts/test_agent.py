import asyncio
import websockets
import argparse
import time
import json


async def connect_to_dotnet_server(port):
    uri = f"ws://127.0.0.1:{port}"  # Use the provided port in the URI

    async with websockets.connect(uri) as websocket:
        try:
            while True:
                response_from_dotnet = await websocket.recv()
                print(f"Received response from .NET server: {response_from_dotnet}")
                obj = json.loads(response_from_dotnet)
                if obj["Observation"] == "This is a game state" and obj["Reward"] == 0 and obj["Done"] == True:
                    message_to_dotnet = "This is a game action"
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
